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

public class User : MonoBehaviour{

   static private string PHOTOS_SAVE_PATH = "Assets/token/";

   public string email;
   public bool categorisePhotos;
   public string username;
   private string photosSavePath;
   public UserPhotos photos;

   public void Login(string email)
   {
      this.email = email; 
      username = email.Split('@')[0];
      photosSavePath = PHOTOS_SAVE_PATH + username + ".json";
      photos = new UserPhotos(this, photosSavePath); // initialise photos as empty
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
      UserCredential credential = photos.credential;
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
      string link = "https://photoslibrary.googleapis.com/v1/mediaItems";
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
      int maxPhotos = 10;
      if(categorisePhotos){
         // perform categorisation process
         string[] includedCategories = ContentFilter.ALL_CATEGORIES;
         foreach (var category in includedCategories)
         {
            // perform post request for each category (api calls = num categories)
            MediaItemSearchRequest searchReq = new MediaItemSearchRequest(maxPhotos, new string[]{category}, new string[] {}); //no excluded categories
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
                     // add category to according photo in allPhotos by id
                     photos.allPhotos[mediaItem.id].categories.Add(category);
                  }
                  // set category counts for this category
                  photos.initialCategoryCounts[category] = responseObject.mediaItems.Count;
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
}