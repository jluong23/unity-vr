using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Collections;

public class RestHelper : MonoBehaviour {
   
   static ClientSecrets clientSecrets;

   public static UserCredential getCredential(String user, String[] scopes){
      UserCredential credential;
      using (var stream = new FileStream("Assets/credentials.json", FileMode.Open, FileAccess.Read))
      {
         // The file token.json stores the user's access and refresh tokens, and is created
         // automatically when the authorization flow completes for the first time.
         string credPath = "Assets/token";
         clientSecrets = GoogleClientSecrets.FromStream(stream).Secrets; 

         credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
               clientSecrets,
               scopes,
               user,
               CancellationToken.None,
               new FileDataStore(credPath, true))
               .Result;
      }
      return credential;
   }

   public static HttpWebRequest createHttpWebRequest(UserCredential credential, String link, String method){
      HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(link);
      httpWebRequest.Method = method;
      httpWebRequest.ContentType = "application/json";
      httpWebRequest.Headers.Add("client_id", clientSecrets.ClientId);
      httpWebRequest.Headers.Add("client_secret", clientSecrets.ClientSecret);
      httpWebRequest.Headers.Add("Authorization:" + credential.Token.TokenType + " " + credential.Token.AccessToken);
      return httpWebRequest;
   }

   public static MediaItemRequestResponse performGetRequest(UserCredential credential, String link){

      MediaItemRequestResponse responseObject = new MediaItemRequestResponse();

      try{
         HttpWebRequest httpWebRequest = createHttpWebRequest(credential, link, "GET");

         // get and return media item response
         HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse;
         using (Stream responseStream = response.GetResponseStream())
         {
            string responseString = new StreamReader(responseStream, Encoding.UTF8).ReadToEnd();
            responseObject = JsonConvert.DeserializeObject<MediaItemRequestResponse>(responseString);
         }
      }         
      catch (Exception ex){
         Debug.Log("Error occured: " + ex.Message);
      }

      return responseObject;
   }


   public static MediaItemRequestResponse performPostRequest(UserCredential credential, String link, String jsonBody){
      MediaItemRequestResponse responseObject = new MediaItemRequestResponse();

      try{
         HttpWebRequest httpWebRequest = createHttpWebRequest(credential, link, "POST");
         using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
         {
            // add json body post data
            streamWriter.Write(jsonBody);
         }
         // get and return media item response
         HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse;
         using (Stream responseStream = response.GetResponseStream())
         {
            string responseString = new StreamReader(responseStream, Encoding.UTF8).ReadToEnd();
            responseObject = JsonConvert.DeserializeObject<MediaItemRequestResponse>(responseString);
         }
      }         
      catch (Exception ex){
         Debug.Log("Error occured: " + ex.StackTrace);
      }

      return responseObject;
   }
   // unity method
   public static IEnumerator createUnityWebRequest(UserCredential credential, string link, string method, string body){
      UnityWebRequest unityWebRequest = new UnityWebRequest(link);
      unityWebRequest.SetRequestHeader("Authorization", string.Format("{0} {1}", credential.Token.TokenType, credential.Token.AccessToken));
      unityWebRequest.SetRequestHeader("Accept", "application/json");
      unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
   
      yield return unityWebRequest.SendWebRequest();
      if(unityWebRequest.result == UnityWebRequest.Result.ConnectionError){
         Debug.Log(unityWebRequest.error);
      }else{
         Debug.Log(unityWebRequest.downloadHandler.text);
      }
   }
}