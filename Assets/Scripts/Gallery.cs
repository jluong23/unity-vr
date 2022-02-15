using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Google.Apis.Auth.OAuth2;


public class Gallery : MonoBehaviour
{
    public GameObject menu;
    static public int maxPhotos = 12;
    public bool categorisePhotos = true;
    public string user = "jluong1@sheffield.ac.uk";
    public string[] scopes = {
        "https://www.googleapis.com/auth/photoslibrary.readonly"
    };


    // dictionary from id to mediaItem object
    static public Dictionary<string, MediaItem> allPhotos = new Dictionary<string, MediaItem>();
    // dictionary from category to count in allPhotos
    static private Dictionary<string, int> categoryCounts = new Dictionary<string, int>();
    
    // array of mediaItem ids for currently shown photos, subset of allPhotos
    List<string> currentPhotoIds = new List<string>();
   
    // coroutine which updates the dictionaries categoryCounts and allPhotos
    private static IEnumerator populateAllPhotos(UserCredential credential, bool categorise){
      string link = "https://photoslibrary.googleapis.com/v1/mediaItems";
      MediaItemRequestResponse responseObject = RestHelper.performGetRequest(credential, link);
      // turn list of MediaItems into dictionary from ids to MediaItem
      allPhotos = responseObject.mediaItems.ToDictionary(x => x.id, x => x);
      
      if(categorise){
         // perform categorisation process
         link = "https://photoslibrary.googleapis.com/v1/mediaItems:search";
         // string[] includedCategories = {"None", "TRAVEL", "PEOPLE", "SPORT"};
         string[] includedCategories = ContentFilter.ALL_CATEGORIES;
         // string[] includedCategories = {"None"};
         foreach (var category in includedCategories)
         {
            // perform post request for each category (api calls = num categories)
            MediaItemSearchRequest searchReq = new MediaItemSearchRequest(maxPhotos, new string[]{category}, new string[] {}); //no excluded categories
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
      yield return null;
   }

    // from allPhotos, retrieve a subset of allPhotos which have the given categories
    private List<string> getPhotoIds(List<string> includedCategories){
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

    // ran when the 'show photos data' button is pressed for the first time, showing all photos 
   // from the user's library
    public void initPhotos(){
        UserCredential credential = RestHelper.getCredential(user, scopes);

        // populate allPhotos
        StartCoroutine(populateAllPhotos(credential, categorisePhotos));
        
        if(allPhotos.Count > 0){
            // update category counts for each category
            menu.GetComponent<CategoryMenu>().appendCategoryCounts(categoryCounts);
        }
    }

    // update the gallery with photos with the selected categories from the category menu
    public void updatePhotos(){
        // clear current gallery
        clearGallery();

        // get ids of the photos with selected categories
        List<string> selectedCategories = menu.GetComponent<CategoryMenu>().getSelectedCategories();
        List<string> newPhotoIds = getPhotoIds(selectedCategories);
        Debug.Log(string.Join(",", selectedCategories));
        // update currentPhotoIds
        currentPhotoIds = newPhotoIds;
        int i = 0;
        foreach (var photoId in currentPhotoIds)
        {
            // start setting the media frames
            MediaItem mediaItem = allPhotos[photoId];
            MediaFrame mediaFrameComponent = transform.GetChild(i).GetComponent<MediaFrame>();
            // set the mediaItem attribute for the mediaFrame component
            mediaFrameComponent.mediaItem = mediaItem;
            // set the texture of frame, showing the image itself
            mediaFrameComponent.displayTexture();
            i+=1;
        }
    }

    public void clearGallery(){
        currentPhotoIds = new List<string>();
        foreach (var mediaFrame in GetComponentsInChildren<MediaFrame>())
        {
            mediaFrame.restoreTexture();
        }
    }
}
