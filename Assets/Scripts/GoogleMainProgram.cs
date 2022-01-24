using Google.Apis.Auth.OAuth2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoogleMainProgram : MonoBehaviour {

   public Text photosWallText;
   public Button showPhotosButton;
   public int maxPhotos = 10;

   static List<MediaItem> getPhotos(UserCredential credential){
      string link = "https://photoslibrary.googleapis.com/v1/mediaItems";
      MediaItemRequestResponse responseObject = GoogleHelper.performGetRequest(credential, link);
      return responseObject.mediaItems;
   }

   static Dictionary<string,List<MediaItem>> getPhotosByCategories(UserCredential credential, int maxImages){
      string link = "https://photoslibrary.googleapis.com/v1/mediaItems:search";
      string[] includedCategories = {"NONE", "TRAVEL", "PEOPLE", "SPORT"};
      // string[] includedCategories = {"NONE"};
      Dictionary<string,List<MediaItem>> categoriesToPhotos = new Dictionary<string,List<MediaItem>>();
      foreach (var category in includedCategories)
      {
         // perform post request for each category (api calls = num categories)
         MediaItemSearchRequest searchReq = new MediaItemSearchRequest(maxImages, new string[]{category}, new string[] {}); //no excluded categories
         string jsonBody = searchReq.getJson();
         Debug.Log(jsonBody);
         // perform post request
         MediaItemRequestResponse responseObject = GoogleHelper.performPostRequest(credential, link, jsonBody);
         // store list of images in dictionary for given category
         categoriesToPhotos.Add(category, responseObject.mediaItems);
      }
      return categoriesToPhotos;
   }

   public void updatePhotosWall(){
      string user = "jluong1@sheffield.ac.uk";
      string[] scopes = {
         "https://www.googleapis.com/auth/photoslibrary.readonly"
      };
      UserCredential credential = GoogleHelper.getCredential(user, scopes);
      Dictionary<string,List<MediaItem>> categoryPhotos = getPhotosByCategories(credential, maxPhotos);

      // update the id wall
      photosWallText.text = "Photos Data \n";
      // print category info
      foreach(var entry in categoryPhotos)
      {
         if(entry.Key == "NONE"){
            photosWallText.text += "Total: " + categoryPhotos["NONE"].Count;
         }else{
            photosWallText.text += entry.Key + ": " + categoryPhotos[entry.Key].Count;
         }
         photosWallText.text += "\n";
      }

   }


   // // use this to auto press the show photos button
   // private void Start() {
   //    showPhotosButton.onClick.Invoke();
   // }
}


