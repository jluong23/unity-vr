using System.Collections.Generic;
using System;
using Google.Apis.Auth.OAuth2;
using System.Linq;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Google.Apis.Util.Store;
using System.Threading;
using Google.Apis.Services;

[JsonObject(MemberSerialization.OptIn)] // only serialize fields with jsonProperty. In this case, allPhotos and initialCateogryCounts are stored

public class UserPhotos{

   // dictionary from id to mediaItem object, used for categorisation after retrieving media photos
   [JsonProperty]
   public Dictionary<string, MediaItem> allPhotos;
   [JsonProperty]
   // dictionary from category to count in allPhotos
   public Dictionary<string, int> initialCategoryCounts;
   //max photos per request when loading images, max is 100
   public static int MAX_PHOTOS_PER_REQUEST = 100;
   // max number in each category. max is 100 
   public static int MAX_PHOTOS_PER_CATEGORY = 100;
   // max number of photos in library to load
   public int maxPhotos;
   // if the user photo variables (allPhotos and initialCategoryCounts) are fully loaded in with all albums, useful for unity coroutine conditions
   public bool loaded;
   // if there exists a json save for the user
   public bool hasSave;
   private User user;
   private bool categorisePhotos;
   public string savePath;
   // used for User.performCategorisation coroutine, incrementing after categorisation has completed for the given category
   public int categoriesLoaded;
   public bool loadVideos;
   public enum LoadOrder {OLDEST_FIRST, NEWEST_FIRST}; 
   public LoadOrder loadOrder; 

   public UserPhotos(User user, string savePath){
      /// <summary>
      /// Constructor for userPhotos
      /// </summary>
      this.user = user;
      this.savePath = savePath;
      this.maxPhotos = 500;
      this.loadOrder = LoadOrder.NEWEST_FIRST;
      this.loadVideos = false;

      if(File.Exists(savePath) && !user.oauthRefreshRequired){
         // read stored data file if it exists
         // save exists and variables will be fully loaded in
         this.loaded = true;
         this.hasSave = true; 
         Debug.Log("Loading user photos data from " + savePath);
         StreamReader reader = new StreamReader(savePath);
         UserPhotos loadedData = JsonConvert.DeserializeObject<UserPhotos>(reader.ReadToEnd());
         reader.Close();
         this.allPhotos = loadedData.allPhotos;
         this.initialCategoryCounts = loadedData.initialCategoryCounts;
         this.categoriesLoaded = initialCategoryCounts.Count;

      }else{
         if(user.oauthRefreshRequired && File.Exists(savePath)){
            Debug.Log("File found at " + savePath + " but oauth expired, overwriting...");
         }else{
            Debug.Log("Could not find an existing user photos save for " + user.username);
         }
         // initialise category counts, all categories to a count of 0 images
         initialCategoryCounts = ContentFilter.ALL_CATEGORIES.ToDictionary(x => x, x => 0);
         allPhotos = new Dictionary<string, MediaItem>();
         this.loaded = false; // not loaded, initialises as empty photos and category counts
         this.hasSave = false;
         this.categoriesLoaded = 0;
      }

   }

   [JsonConstructor]
   // used when serialising an object, loading save data
   private UserPhotos(Dictionary<string, MediaItem> allPhotos, Dictionary<string, int> initialCategoryCounts){
      this.allPhotos = allPhotos;
      this.initialCategoryCounts = initialCategoryCounts;
   }

   public List<MediaItem> getPhotos(){
      ///
      /// Returns all photos
      /// 
      return new List<MediaItem>(allPhotos.Values);
   } 

   public Dictionary<string, int> getCategoryCounts(List<string> selectedCategories, Tuple<DateTime, DateTime> dateRange){
      /// <summary>
      /// Given a list of selected categories and date ranges from the UI menu,
      /// return a subset of the initial category counts. The value for each category indicates
      /// further images which contain the selectedCategories and an additional category.
      /// 
      /// If the entire dataset is currently being shown on menu, the initial category counts for the dataset is returned.
      /// </summary>
      /// 
      
      // original data set is shown, return initial counts
      if(getPhotos(selectedCategories, dateRange).Count == allPhotos.Count) return initialCategoryCounts;
      Dictionary<string, int> subCategoryCounts = new Dictionary<string, int>();
      foreach (string category in ContentFilter.ALL_CATEGORIES)
      {
         // count the number of photos for the selected categories + the additional category
         List<string> totalCategories = new List<string>(selectedCategories);
         totalCategories.Add(category);
         int count = getPhotos(totalCategories, dateRange).Count;
         // add to subCategoryCount
         subCategoryCounts.Add(category, count);
      }
      
      return subCategoryCounts;
   }

   public List<MediaItem> getPhotos(List<string> includedCategories, Tuple<DateTime, DateTime> dateRange){
      /// <summary>
      /// from all photos, retrieve subset which have the given categories 
      /// and creation time attributes within the given date range
      /// </summary>
      List<MediaItem> foundPhotos = new List<MediaItem>();
      List<MediaItem> allPhotosList = getPhotos();

      if(allPhotosList.Count > 0){
         // count of intersection between photos categories and includedCategories == count of included categories
         // ie. photo's categories contains all includedCategories
         foundPhotos = new List<MediaItem>(allPhotosList.Where(i => 
            i.categories.Intersect(includedCategories).ToList().Count == includedCategories.Count &&
            // within the date range
            Convert.ToDateTime(i.mediaMetadata.creationTime) >= dateRange.Item1 && 
            Convert.ToDateTime(i.mediaMetadata.creationTime) <= dateRange.Item2
         ));
      }
      return foundPhotos;    
   }

   public Tuple<DateTime, DateTime> getDateRange(){
      List<MediaItem> photos = allPhotos.Values.ToList();  
      // allPhotos and photos is from newest to oldest. So first element is newest photo
      DateTime endDate = Convert.ToDateTime(photos[0].mediaMetadata.creationTime);
      DateTime startDate = Convert.ToDateTime(photos[photos.Count-1].mediaMetadata.creationTime);
      return new Tuple<DateTime, DateTime>(startDate, endDate);
   }

   public void saveData()
   {
      /// <summary>
      /// Saves user data in savePath, does not overwrite. 
      /// </summary>
      Debug.Log("Saving data to " + savePath);
      StreamWriter writer = new StreamWriter(savePath);
      writer.WriteLine(JsonConvert.SerializeObject(this));
      writer.Close();
   }
}