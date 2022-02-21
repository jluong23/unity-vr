using System.Collections.Generic;
using System;
using Google.Apis.Auth.OAuth2;
using System.Linq;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class User{

   static private string PHOTOS_SAVE_PATH = "Assets/token/";

   private string email;
   private string username;
   private string photosSavePath;
   private UserCredential credential;
   private UserPhotos photos;

   public User(string email, bool categorisePhotos){
      /// <summary>
      /// Constructor for user
      /// </summary>
      this.email = email;
      this.username = email.Split('@')[0];
      this.photosSavePath = PHOTOS_SAVE_PATH + username + ".json";
      this.credential = RestHelper.getCredential(email, scopes);
      this.photos = null;
   }

}