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
using System.Threading;
using System.Threading.Tasks;
public class User : MonoBehaviour{

   static private string PHOTOS_SAVE_PATH = "Assets/token/";
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
   public void Login()
   {
      setCredential();
   }

   /// <summary>
   /// Using Assets/credential.json data, creates an oauth2 token asynchronously, storing in credPath.
   /// After credential has been made, sets user data.
   /// </summary>
   /// <returns></returns>
   private async void setCredential(){
      using (var stream = new FileStream("Assets/credentials.json", FileMode.Open, FileAccess.Read))
      {
         // The file token.json stores the user's access and refresh tokens, and is created
         // automatically when the authorization flow completes for the first time.
         string credPath = "Assets/token";
         ClientSecrets clientSecrets = GoogleClientSecrets.FromStream(stream).Secrets;
         CancellationTokenSource cts = new CancellationTokenSource();
         cts.CancelAfter(TimeSpan.FromSeconds(60)); //60 seconds to complete login

         credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            clientSecrets,scopes,"user",cts.Token,new FileDataStore(credPath, true));
      }
      // credential found, set user email and photos
      Debug.Log("Oauth credential created for user, finding email...");
      StartCoroutine(setUserData());
   }

   // function which populates user.photos 
   public void populatePhotosByAPI(){
      StartCoroutine(loadPhotos());
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
   private IEnumerator loadPhotos(){
      //load up to max photos
      string link = "https://photoslibrary.googleapis.com/v1/mediaItems?pageSize=" + UserPhotos.MAX_PHOTOS;
      UnityWebRequest unityWebRequest = createUnityWebRequest(link, "GET", "");

      yield return unityWebRequest.SendWebRequest();
      if(unityWebRequest.result == UnityWebRequest.Result.ConnectionError){
         Debug.Log(unityWebRequest.error);
      }else{
         // successful
         string responseString = unityWebRequest.downloadHandler.text;
         MediaItemRequestResponse responseObject = JsonConvert.DeserializeObject<MediaItemRequestResponse>(responseString);
         // turn list of MediaItems into dictionary from ids to MediaItem
         photos.allPhotos = responseObject.mediaItems.ToDictionary(x => x.id, x => x);
         // start categorising images in photos.allPhotos once they have all loaded in
         StartCoroutine(performCategorisation());
      }
   }

   private IEnumerator performCategorisation(){ 
      string link = "https://photoslibrary.googleapis.com/v1/mediaItems:search";
      if(categorisePhotos){
         // perform categorisation process
         string[] includedCategories = ContentFilter.ALL_CATEGORIES;
         foreach (var category in includedCategories)
         {
            // perform post request for each category (api calls = num categories)
            MediaItemSearchRequest searchReq = new MediaItemSearchRequest(UserPhotos.MAX_PHOTOS_PER_CATEGORY, new string[]{category}, new string[] {}); //no excluded categories
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
                     // UserPhotos.MAX_PHOTOS limit. Only use the loaded images in photos.allPhotos
                     if(photos.allPhotos.ContainsKey(mediaItem.id)){
                        // add category to according photo in allPhotos by id
                        photos.allPhotos[mediaItem.id].categories.Add(category);
                        // increment category count for this category
                        photos.initialCategoryCounts[category]++;
                     }
                  }
               }else{
                  // set category counts to 0 for this category
                  photos.initialCategoryCounts[category] = 0; 
               }
               // categorisation has finished for this category, increment counter
               photos.categoriesLoaded++;

            }
         }
         // finished categorising after all categories have been enumerated
         photos.loaded = true;


      }
      else{
         // categorise = false, all photos have empty category list
         foreach (var mediaItem in photos.allPhotos.Values)
         {
            mediaItem.categories = new List<string> {};
         }
         yield return null;
      }
   }
   /// <summary>
   /// find user email from oauth token and initialise a new, empty UserPhotos 

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
         this.username = email.Split('@')[0];
         photosSavePath = PHOTOS_SAVE_PATH + username + ".json";
         Debug.Log("Email found, username is " + username);
         
         photos = new UserPhotos(this, photosSavePath); // initialise photos as empty
      }
   }         

}