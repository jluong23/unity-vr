using System.Collections;
using System.Collections.Generic;
using System;
using Google.Apis.Auth.OAuth2;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;
using Google.Apis.Util.Store;
using UnityEngine.XR.Interaction.Toolkit;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Responses;
public class User : MonoBehaviour{

   // where photos data is stored in json files
   static public string PHOTOS_SAVE_PATH = "Assets/photos_data/";
   // where oauth tokens are stored
   static public string OAUTH_SAVE_PATH = PHOTOS_SAVE_PATH + "oauth/";
   // in minutes. default is 60mins (1h)
   public static int DEFAULT_OAUTH_EXPIRY_TIME = 60;

   static private string[] scopes = {
      "https://www.googleapis.com/auth/photoslibrary.readonly", "https://www.googleapis.com/auth/userinfo.email"
   };

   public string email;
   public bool categorisePhotos;
   public string username;
   private string photosSavePath;
   public UserPhotos photos;
   // the credential for user
   public UserCredential credential;
   public AuthorizationPopup authorizationMenuPopup;
   // if the user needed to refresh their oauth token, as previous expired, record if refresh took place
   public bool oauthRefreshRequired;

    public void Login(string username)
   {
      setCredential(username);
   }

   /// <summary>
   /// Using Assets/credential.json data, creates an oauth2 token asynchronously, storing in credPath.
   /// After credential has been made, sets user data.
   /// </summary>
   /// <returns></returns>
   private async void setCredential(string username){
      this.username = username;
      using (var stream = new FileStream("Assets/credentials.json", FileMode.Open, FileAccess.Read))
      {
         ClientSecrets clientSecrets = GoogleClientSecrets.FromStream(stream).Secrets;
         CancellationTokenSource cts = new CancellationTokenSource();
         // cts.CancelAfter(TimeSpan.FromSeconds(60)); //60 seconds to complete login

         credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            clientSecrets,scopes,username,cts.Token,new FileDataStore(OAUTH_SAVE_PATH, true));

         //Refresh OAuth token if necessary. Record if an oauth refresh was required
         TimeSpan diff = DateTime.UtcNow.Subtract(credential.Token.IssuedUtc);
         if(diff.Minutes > DEFAULT_OAUTH_EXPIRY_TIME){
            oauthRefreshRequired = true;
            await credential.GetAccessTokenForRequestAsync();
         }else{
            oauthRefreshRequired = false;
         }
      }
      // credential found, set user email and photos
      Debug.Log("Oauth credential created for " + username);
      StartCoroutine(setUserData());
   }

   // function which populates photos 
   public void populatePhotosByAPI(){
      StartCoroutine(loadPhotos(""));
   }

   /// <summary>
   /// Creates a unityWebRequest with supplied authentication headers
   /// </summary>
   /// <param name="link"></param>
   /// <param name="method"></param>
   /// <param name="jsonBody"></param>
   /// <returns></returns>
   private UnityWebRequest createUnityWebRequest(string link, string method, string jsonBody){
      UnityWebRequest unityWebRequest = null;
      if(method == "POST"){
         unityWebRequest = UnityWebRequest.Post(link, "");
         unityWebRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonBody));
      }else if(method == "GET"){
         unityWebRequest = UnityWebRequest.Get(link);
      }
      unityWebRequest.SetRequestHeader("Authorization", string.Format("{0} {1}", credential.Token.TokenType, credential.Token.AccessToken));
      unityWebRequest.SetRequestHeader("Accept", "application/json");
      unityWebRequest.SetRequestHeader("Content-Type", "application/json");
      unityWebRequest.downloadHandler = new DownloadHandlerBuffer();     
      return unityWebRequest;
   }
   /// <summary>
   /// Used to populate user photos via the photos api.
   /// </summary>
   /// <param name="nextPageToken"></param>
   /// <returns></returns>
   private IEnumerator loadPhotos(string nextPageToken){
      string link = string.Format("https://photoslibrary.googleapis.com/v1/mediaItems?pageSize={0}&pageToken={1}", UserPhotos.MAX_PHOTOS_PER_REQUEST, nextPageToken);
      UnityWebRequest unityWebRequest = createUnityWebRequest(link, "GET", "");
      yield return unityWebRequest.SendWebRequest();
      if(unityWebRequest.result == UnityWebRequest.Result.ConnectionError){
         Debug.Log(unityWebRequest.error);
      }else{
         // successful
         string responseString = unityWebRequest.downloadHandler.text;
         MediaItemRequestResponse responseObject = JsonConvert.DeserializeObject<MediaItemRequestResponse>(responseString);
         // turn list of MediaItems into dictionary from ids to MediaItem
         Dictionary<string, MediaItem> responseDict = responseObject.mediaItems.ToDictionary(x => x.id, x => x);
         if(responseDict.Count + photos.allPhotos.Count > photos.maxPhotos){
            // the next response dictionary will take us over the limit, reduce by necessary amount to match photos.maxPhotos
            responseDict = responseDict.Take(photos.maxPhotos - photos.allPhotos.Count).ToDictionary(x => x.Key, x => x.Value);
         }
         // concatenate new photos from request with allPhotos, ignoring duplicate entries
         photos.allPhotos = photos.allPhotos.Concat(responseDict.Where(x => !photos.allPhotos.Keys.Contains(x.Key))).ToDictionary(x => x.Key, x => x.Value);

         if(responseObject.nextPageToken == null || photos.allPhotos.Count >= photos.maxPhotos){
            // all photos loaded or maximum reached, start categorising images in photos.allPhotos
            StartCoroutine(performCategorisation());
         }else{
            // keep loading photos, not all have loaded yet
            StartCoroutine(loadPhotos(responseObject.nextPageToken));
         }
      }
   }

   private IEnumerator performCategorisation(){ 
      if(categorisePhotos){
         // perform categorisation process
         string[] includedCategories = ContentFilter.ALL_CATEGORIES;
         foreach (var category in includedCategories)
         {
            bool categoryLoaded = false;
            string nextPageToken = "";

            while(!categoryLoaded){
               string link = "https://photoslibrary.googleapis.com/v1/mediaItems:search";
               // perform post request for each category (api calls = num categories)
               MediaItemSearchRequest searchReq = new MediaItemSearchRequest(UserPhotos.MAX_PHOTOS_PER_CATEGORY, nextPageToken, new string[]{category}, new string[] {}); //no excluded categories
               string jsonBody = searchReq.getJson(); 
               // perform post request
               UnityWebRequest unityWebRequest = createUnityWebRequest(link, "POST", jsonBody);
               yield return unityWebRequest.SendWebRequest();
               if(unityWebRequest.result == UnityWebRequest.Result.ConnectionError){
                  Debug.Log(unityWebRequest.error);
               }else{
                  // successful
                  string responseString = unityWebRequest.downloadHandler.text;
                  MediaItemRequestResponse responseObject = JsonConvert.DeserializeObject<MediaItemRequestResponse>(responseString);
                  // if photos exist for this category
                  if(responseObject.mediaItems != null && responseObject.mediaItems.Count > 0){
                     foreach (var mediaItem in responseObject.mediaItems)
                     {
                        // categorisation may retrieve images which are not part of photos.allPhotos due to the 
                        // UserPhotos.maxPhotos limit. Only use the loaded images in photos.allPhotos
                        if(photos.allPhotos.ContainsKey(mediaItem.id)){
                           // add category to according photo in allPhotos by id
                           photos.allPhotos[mediaItem.id].categories.Add(category);
                           // increment category count for this category
                           photos.initialCategoryCounts[category]++;
                        }
                     }
                  }
                  if(responseObject.nextPageToken == null){
                     // should be last page of mediaItems for this category, therefore last iteration of while loop
                     categoryLoaded = true;
                     // categorisation has finished for this category, increment counter
                     photos.categoriesLoaded++;
                  }else{
                     nextPageToken = responseObject.nextPageToken;
                  }
               }
            }
         }
         // finished categorising after all categories have been enumerated
         photos.loaded = true;


      }
      else{
         // categorisePhotos = false, all photos have empty category list
         foreach (var mediaItem in photos.allPhotos.Values)
         {
            mediaItem.categories = new List<string> {};
         }
         yield return null;
      }
   }
   /// <summary>
   /// find user email from oauth token and initialise a new, empty UserPhotos.
   /// Allow the user to progress the authorization page after completed.
   /// </summary>
   /// <returns></returns>
   private IEnumerator setUserData(){
      string link = "https://www.googleapis.com/oauth2/v3/tokeninfo?access_token=" + credential.Token.AccessToken;
      UnityWebRequest unityWebRequest = createUnityWebRequest(link, "GET", "");

      yield return unityWebRequest.SendWebRequest();
      if(unityWebRequest.result == UnityWebRequest.Result.ConnectionError){
         Debug.Log(unityWebRequest.error);
      }else{
         // successful, token data found
         string tokenDataJson = unityWebRequest.downloadHandler.text;
         Dictionary<string, string> tokenData = JsonConvert.DeserializeObject<Dictionary<string, string>>(tokenDataJson);
         this.email = tokenData["email"]; 
         photosSavePath = PHOTOS_SAVE_PATH + username + ".json";
         photos = new UserPhotos(this, photosSavePath); // initialise photos as empty
         // allow user to progress the auth menu popup
         authorizationMenuPopup.allowProgress(this.email);
      }
   }         

}