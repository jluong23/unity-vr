using Google.Apis.Auth.OAuth2;

using UnityEngine;

public class GoogleMainProgram : MonoBehaviour {

   static void printMediaItems(UserCredential credential){
      string link = "https://photoslibrary.googleapis.com/v1/mediaItems";
      MediaItemRequestResponse responseObject = GoogleHelper.performGetRequest(credential, link);
      foreach (var item in responseObject.mediaItems){ 
         Debug.Log(item);
      }
   }

   static void printCategories(UserCredential credential){
      string link = "https://photoslibrary.googleapis.com/v1/mediaItems:search";
      MediaItemSearchRequest searchReq = new MediaItemSearchRequest();
      string jsonBody = searchReq.getJson();
      MediaItemRequestResponse responseObject = GoogleHelper.performPostRequest(credential, link, jsonBody);
      foreach (var item in responseObject.mediaItems){ 
         Debug.Log(item);
      }
   }

   private void Start() {
      string user = "jluong1@sheffield.ac.uk";
      string[] scopes = {
         "https://www.googleapis.com/auth/photoslibrary.readonly"
      };
      UserCredential credential = GoogleHelper.getCredential(user, scopes);
      printCategories(credential);
      // printMediaItems(credential);
      
   }
}


