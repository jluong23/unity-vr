using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;


public class MediaItem
{
   public static int THUMBNAIL_WIDTH = 200;
   public static int THUMBNAIL_HEIGHT = 200;
   public string id;
   public string description;
   public string productUrl;
   public string baseUrl;
   public string mimeType;
   public MediaMetadata mediaMetadata;
   public string filename;
   public List<string> categories = new List<string>();
   [JsonIgnore]
   public Texture texture;
}

public class MediaItemRequestResponse
{
   public List<MediaItem> mediaItems;
   public string nextPageToken;
}

public class MediaMetadata
{
   public string creationTime;
   public string width;
   public string height;
   public Photo photo;
}

public class Photo
{
   public string cameraMake;
   public string cameraModel;
   public double focalLength;
   public double apertureFNumber;
   public int isoEquivalent;
}

// filters for media item search
public class DateFilter
{
   public Date[] dates;
   public DateRange[] ranges;

   public DateFilter(){
      // TODO: Defaults to no dates or ranges
      dates = new Date[] {};
      ranges = new DateRange[] {};

   }
}

public class ContentFilter
{
   public string[] includedContentCategories;
   public string[] excludedContentCategories;
   public static string[] ALL_CATEGORIES = new string[] {"Landscapes", "Receipts", "Cityscapes", "Landmarks", "Selfies", "People", 
   "Pets", "Weddings", "Documents", "Travel", "Animals", "Food", "Sport", "Night", "Performances",
   "Whiteboards", "Screenshots", "Utility", "Arts", "Crafts", "Fashion", "Houses", "Gardens", "Flowers", "Holidays"};


   public ContentFilter(){
      includedContentCategories = new string[] {};
      excludedContentCategories = new string[] {};
   }

   public ContentFilter(string[] includedCategories, string[] excludedCategories){
      includedContentCategories = includedCategories;
      excludedContentCategories = excludedCategories;
   }
}

public class MediaTypeFilter
{
   public string[] mediaTypes;

   /// <summary>
   /// photos and videos if includeVideos is true, only photos if includeVideos is false.
   /// </summary>
   /// <param name="includeVideos"></param>
   public MediaTypeFilter(bool includeVideos){
      // Defaults to PHOTO
      // can also be ALL_MEDIA, VIDEO
      mediaTypes = includeVideos ? new string[] {"ALL_MEDIA"} : new string[] {"PHOTO"};

   }
}

public class FeatureFilter
{
   public string[] includedFeatures;

   public FeatureFilter(){
      // Defaults to NONE
      // can also be FAVOURITES
      includedFeatures = new string[] {"NONE"};
   }
}

// collection of all filters
public class MediaFilter
{
   public DateFilter dateFilter;
   public ContentFilter contentFilter;
   public MediaTypeFilter mediaTypeFilter;
   public FeatureFilter featureFilter;

   public MediaFilter(){
      // uses defaults for each filter, default content is "NONE" 
      // dateFilter = new DateFilter();
      contentFilter = new ContentFilter();
      mediaTypeFilter = new MediaTypeFilter(false);
      featureFilter = new FeatureFilter();
   }

   public MediaFilter(string[] includedCategories, string[] excludedCategories, bool includeVideos){
      // uses defaults for each filter, but use included and excluded categories arrays
      // dateFilter = new DateFilter();
      contentFilter = new ContentFilter(includedCategories, excludedCategories);
      mediaTypeFilter = new MediaTypeFilter(includeVideos);
      featureFilter = new FeatureFilter();
   }
}

public class MediaItemSearchRequest
{  
   public string albumId;
   public int pageSize;
   public string pageToken;
   // json key to 'filters' for post request 
   [JsonProperty(PropertyName = "filters")]
   public MediaFilter mediaFilter;
   public string orderBy;

   public MediaItemSearchRequest(int pageSize, string pageToken, string[] includedCategories, string[] excludedCategories, bool includeVideos){
      this.mediaFilter = new MediaFilter(includedCategories, excludedCategories, includeVideos);
      this.pageSize = pageSize;
      this.pageToken = pageToken;
   }

   public string getJson(){
      // string json = JsonUtility.ToJson(this);
      string json = JsonConvert.SerializeObject(this, 
         Formatting.Indented, 
         new JsonSerializerSettings{
               NullValueHandling = NullValueHandling.Ignore
         });
      return json;
   }
}

public class Date
{
   public int year {get; set;}
   public int month {get; set;}
   public int day {get; set;}

   public Date(int year, int month, int day){
      this.year = year;
      this.month = month;
      this.day = day;
   }
}

public class DateRange
{
   public Date startDate {get; set;}
   public Date endDate {get; set;}

   public DateRange(Date startDate, Date endDate){
      this.startDate = startDate;
      this.endDate = endDate;
   }

}