using Google.Apis.Auth.OAuth2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoogleMainProgram : MonoBehaviour {

   public Text photosWallText;
   public Button showPhotosButton;
   public GameObject gallery;
   static public int maxPhotos = 12;

   static private Dictionary<string,List<MediaItem>> categorisedPhotos = new Dictionary<string, List<MediaItem>>();
   static private List<MediaItem> allPhotos = new List<MediaItem>();

   static private void setAllPhotos(UserCredential credential){
      string link = "https://photoslibrary.googleapis.com/v1/mediaItems";
      MediaItemRequestResponse responseObject = GoogleHelper.performGetRequest(credential, link);
      allPhotos = responseObject.mediaItems;
   }

   static private void setCategorisedPhotos(UserCredential credential){
      string link = "https://photoslibrary.googleapis.com/v1/mediaItems:search";
      string[] includedCategories = {"NONE", "TRAVEL", "PEOPLE", "SPORT"};
      // string[] includedCategories = {"NONE"};
      foreach (var category in includedCategories)
      {
         // perform post request for each category (api calls = num categories)
         MediaItemSearchRequest searchReq = new MediaItemSearchRequest(maxPhotos, new string[]{category}, new string[] {}); //no excluded categories
         string jsonBody = searchReq.getJson();
         // perform post request
         MediaItemRequestResponse responseObject = GoogleHelper.performPostRequest(credential, link, jsonBody);
         // store list of images in dictionary for given category
         categorisedPhotos.Add(category, responseObject.mediaItems);
      }
   }

   // coroutine which updates the dictionaries categorisedPhotos and allPhotos, calling their setter methods
   private static IEnumerator populatePhotos(UserCredential credential){
      setAllPhotos(credential);
      setCategorisedPhotos(credential);
      yield return null;
   }

   public void updatePhotosWall(){
      string user = "jluong1@sheffield.ac.uk";
      string[] scopes = {
         "https://www.googleapis.com/auth/photoslibrary.readonly"
      };
      UserCredential credential = GoogleHelper.getCredential(user, scopes);
      // populate photos variables
      StartCoroutine(populatePhotos(credential));

      if(allPhotos.Count > 0){

         // update the gallery of frames for this category with all images
         int i = 0;
         foreach (var mediaPhoto in allPhotos)
         {
            gallery.transform.GetChild(i).GetComponent<ImageDownloader>().setTexture(mediaPhoto.baseUrl);
            i+=1;
         }

         // update the photos data wall (category info)
         photosWallText.text = "Photos Data \n";
         foreach(var entry in categorisedPhotos)
         {
            if(entry.Key == "NONE"){
               photosWallText.text += "Total: " + categorisedPhotos["NONE"].Count;
            }else{
               photosWallText.text += entry.Key + ": " + entry.Value.Count;
            }
            photosWallText.text += "\n";
         }

      }

   }
}


