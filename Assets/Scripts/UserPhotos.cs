using System.Collections.Generic;
using System;
using Google.Apis.Auth.OAuth2;
using System.Linq;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
public class UserPhotos{

   static private int MAX_PHOTOS = 12;
   static private string[] scopes = {
      "https://www.googleapis.com/auth/photoslibrary.readonly"
   };
   // dictionary from id to mediaItem object, used for categorisation after retrieving media photos
   [JsonProperty]
   private Dictionary<string, MediaItem> allPhotos;
   // dictionary from category to count in allPhotos
   [JsonProperty]
   private Dictionary<string, int> initialCategoryCounts;

   // the credential for user photos
   private UserCredential credential;
   private User user;
   private bool categorisePhotos;

   public UserPhotos(User user, bool categorisePhotos){
      /// <summary>
      /// Constructor for userPhotos, fetching from API.
      /// categorisePhotos: If photos should be categorised if local save does not exist
      /// allPhotos should be sorted by newest to oldest, assuming elements are not deleted.
      /// </summary>
      this.user = user;
      this.categorisePhotos = categorisePhotos;
      credential = RestHelper.getCredential(user.email, scopes);
      if(File.Exists(user.photosSavePath)){
         // read stored data file if it exists
         Debug.Log("Loading data from " + user.photosSavePath);
         StreamReader reader = new StreamReader(user.photosSavePath);
         UserPhotos loadedData = JsonConvert.DeserializeObject<UserPhotos>(reader.ReadToEnd());
         reader.Close();
         this.allPhotos = loadedData.allPhotos;
         this.initialCategoryCounts = loadedData.initialCategoryCounts;
      }else{
         Debug.Log("Could not find an existing save, loading files via Google Photos API...");
         initialCategoryCounts = new Dictionary<string, int>();
         allPhotos = new Dictionary<string, MediaItem>();
         
         // time how long it takes to populate all photos
         UnityStopwatch.start();
         populateAllPhotos(); // updates initialCategoryCounts and allPhotos
         Debug.Log("Populating photos runtime: " + UnityStopwatch.stop());
         this.saveData();
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

   public Dictionary<string, int> getInitialCategoryCounts(){
      return initialCategoryCounts;
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
      if(getPhotos(selectedCategories, dateRange).Count == allPhotos.Count) return getInitialCategoryCounts();
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
      /// Saves user data in user.photosSavePath, does not overwrite. 
      /// </summary>
      Debug.Log("Saving data to " + user.photosSavePath);
      StreamWriter writer = new StreamWriter(user.photosSavePath);
      writer.WriteLine(JsonConvert.SerializeObject(this));
      writer.Close();
   }

   // function which updates the dictionaries categoryCounts and allPhotos
   private void populateAllPhotos(){
      string link = "https://photoslibrary.googleapis.com/v1/mediaItems";
      MediaItemRequestResponse responseObject = RestHelper.performGetRequest(credential, link);
      // turn list of MediaItems into dictionary from ids to MediaItem
      allPhotos = responseObject.mediaItems.ToDictionary(x => x.id, x => x);
      
      if(categorisePhotos){
         // perform categorisation process
         link = "https://photoslibrary.googleapis.com/v1/mediaItems:search";
         string[] includedCategories = ContentFilter.ALL_CATEGORIES;
         foreach (var category in includedCategories)
         {
            // perform post request for each category (api calls = num categories)
            MediaItemSearchRequest searchReq = new MediaItemSearchRequest(MAX_PHOTOS, new string[]{category}, new string[] {}); //no excluded categories
            string jsonBody = searchReq.getJson();
            // perform post request
            MediaItemRequestResponse categoryResponseObject = RestHelper.performPostRequest(credential, link, jsonBody);
               // if photos exist for this category
            if(categoryResponseObject.mediaItems != null && categoryResponseObject.mediaItems.Count > 0){
               foreach (var mediaItem in categoryResponseObject.mediaItems)
               {
                  // add category to according photo in allPhotos by id
                  allPhotos[mediaItem.id].categories.Add(category);
               }
               // set category counts for this category
               initialCategoryCounts[category] = categoryResponseObject.mediaItems.Count;
            }else{
               // set category counts to 0 for this category
               initialCategoryCounts[category] = 0; 

            }
         }
      }
      else{
         // categorise = false, all photos have empty category list
         foreach (var mediaItem in allPhotos.Values)
         {
            mediaItem.categories = new List<string> {};
         }
      }
   }

}