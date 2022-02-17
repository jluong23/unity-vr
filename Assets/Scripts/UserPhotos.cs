using System.Collections.Generic;
using System;
using Google.Apis.Auth.OAuth2;
using System.Linq;

public class UserPhotos
{

    static private int MAX_PHOTOS = 12;
    static private string[] scopes = {
        "https://www.googleapis.com/auth/photoslibrary.readonly"
    };

    // dictionary from id to mediaItem object
    public Dictionary<string, MediaItem> allPhotos;
    // dictionary from category to count in allPhotos
    public Dictionary<string, int> categoryCounts;

    private UserCredential credential;
    private string email;
    private bool categorisePhotos;

    public Tuple<DateTime, DateTime> getPhotosRange(){
        return null;
    }
    /**
    Constructor for userPhotos.
    categorisePhotos: If photos should be categorised. 

    allPhotos should be sorted by newest to oldest, assuming elements are not deleted.

    **/
    public UserPhotos(string email, bool categorisePhotos){
        this.email = email;
        this.categorisePhotos = categorisePhotos;
        credential = RestHelper.getCredential(email, scopes);
        categoryCounts = new Dictionary<string, int>();
        allPhotos = new Dictionary<string, MediaItem>();
        populateAllPhotos(); // updates categoryCounts and allPhotos
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
         // string[] includedCategories = {"None", "TRAVEL", "PEOPLE", "SPORT"};
         string[] includedCategories = ContentFilter.ALL_CATEGORIES;
         // string[] includedCategories = {"None"};
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
               categoryCounts[category] = categoryResponseObject.mediaItems.Count;
            }else{
               // set category counts to 0 for this category
               categoryCounts[category] = 0; 

            }
         }
      }
      else{
         // categorise = false, all photos have 'None' category
         foreach (var mediaItem in allPhotos.Values)
         {
            mediaItem.categories = new List<string> {"None"};
         }
         // set category counts 
         categoryCounts["None"] = allPhotos.Count;
      }
   }

    // from allPhotos, retrieve a subset of allPhotos.keys (photo ids) which have the given categories
    public List<string> getPhotoIds(List<string> includedCategories){
        List<string> foundPhotoIds = new List<string>();
        
        if(includedCategories.Count == 0){
            return foundPhotoIds;
        }

        if(allPhotos.Count > 0){
            // count of intersection between photos categories and includedCategories == count of included categories
            // ie. photo's categories contains all includedCategories
            var foundPhotos = allPhotos.Where(i => 
                i.Value.categories.Intersect(includedCategories).ToList().Count == includedCategories.Count);
            foreach (var photo in foundPhotos)
            {
                foundPhotoIds.Add(photo.Key);
            }
        }
        return foundPhotoIds;    
   }

}