using System.Collections.Generic;
using System;
using Google.Apis.Auth.OAuth2;
using System.Linq;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class User{

   static private string PHOTOS_SAVE_PATH = "Assets/token/";

   public string email;
   private string username;
   public string photosSavePath;
   public UserPhotos photos;

   public User(string email){
      /// <summary>
      /// Constructor for user
      /// </summary>
      this.email = email;
      this.username = email.Split('@')[0];
      this.photosSavePath = PHOTOS_SAVE_PATH + username + ".json";
      this.photos = new UserPhotos(this, true);
   }
}