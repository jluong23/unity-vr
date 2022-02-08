using Google.Apis.Auth.OAuth2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GoogleMainProgram : MonoBehaviour {

   public GameObject menu;
   public GameObject gallery;
   static public int maxPhotos = 12;


   static private Dictionary<string, MediaItem> allPhotos = new Dictionary<string, MediaItem>();
   static private Dictionary<string, int> categoryCounts = new Dictionary<string, int>();

   // coroutine which updates the dictionaries categoryCounts and allPhotos
   private static IEnumerator populatePhotos(UserCredential credential, bool categorise){
      string link = "https://photoslibrary.googleapis.com/v1/mediaItems";
      MediaItemRequestResponse responseObject = GoogleHelper.performGetRequest(credential, link);
      // turn list of MediaItems into dictionary from ids to MediaItem
      allPhotos = responseObject.mediaItems.ToDictionary(x => x.id, x => x);
      
      if(categorise){
         // perform categorisation process
         link = "https://photoslibrary.googleapis.com/v1/mediaItems:search";
         // string[] includedCategories = {"NONE", "TRAVEL", "PEOPLE", "SPORT"};
         string[] includedCategories = ContentFilter.ALL_CATEGORIES;
         // string[] includedCategories = {"NONE"};
         foreach (var category in includedCategories)
         {
            // perform post request for each category (api calls = num categories)
            MediaItemSearchRequest searchReq = new MediaItemSearchRequest(maxPhotos, new string[]{category}, new string[] {}); //no excluded categories
            string jsonBody = searchReq.getJson();
            // perform post request

            MediaItemRequestResponse categoryResponseObject = GoogleHelper.performPostRequest(credential, link, jsonBody);
               // if photos exist for this category
            if(categoryResponseObject.mediaItems != null && categoryResponseObject.mediaItems.Count > 0){
               foreach (var mediaItem in categoryResponseObject.mediaItems)
               {
                  // add category to according photo in allPhotos by id
                  allPhotos[mediaItem.id].categories.Add(category);
               }
               // set category counts for this category
               categoryCounts[category] = categoryResponseObject.mediaItems.Count;
            }else{
               // set category counts to 0 for this category
               categoryCounts[category] = 0; 

            }
         }
      }
      else{
         // categorise = false, all photos have 'NONE' category
         foreach (var mediaItem in allPhotos.Values)
         {
            mediaItem.categories = new List<string> {"NONE"};
         }
         // set category counts 
         categoryCounts["NONE"] = allPhotos.Count;
      }
      yield return null;
   }


   // ran when the 'show photos data' button is pressed
   public void updatePhotosWall(){
      bool categorise = true;
      string user = "jluong1@sheffield.ac.uk";
      string[] scopes = {
         "https://www.googleapis.com/auth/photoslibrary.readonly"
      };
      UserCredential credential = GoogleHelper.getCredential(user, scopes);

      // populate allPhotos
      StartCoroutine(populatePhotos(credential, categorise));

      if(allPhotos.Count > 0){
         // update category counts for each category
         Debug.Log(menu.transform.Find("Category Panel"));
         menu.GetComponent<ShowCategories>().appendCategoryCounts(categoryCounts);

         // update the gallery of frames for this category with all images
         int i = 0;
         foreach (var mediaPhoto in allPhotos.Values)
         {
            MediaFrame mediaFrameComponent = gallery.transform.GetChild(i).GetComponent<MediaFrame>();
            // set the MediaItem attribute for the mediaFrame component
            mediaFrameComponent.mediaItem = mediaPhoto;
            // set the texture of frame, showing the image itself
            mediaFrameComponent.displayTexture();
            i+=1;
         }
      }

   }
}


