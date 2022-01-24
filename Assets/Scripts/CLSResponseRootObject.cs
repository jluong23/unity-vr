using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;


public class MediaItem
{
   public string id;
   public string productUrl;
   public string baseUrl;
   public string mimeType;
   public MediaMetadata mediaMetadata;
   public string filename;
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

   public ContentFilter(){
      includedContentCategories = new string[] {"TRAVEL"};
      excludedContentCategories = new string[] {};
   }
}

public class MediaTypeFilter
{
   public string[] mediaTypes;

   public MediaTypeFilter(){
      // Defaults to PHOTO
      // can also be ALL_MEDIA, VIDEO
      mediaTypes = new string[] {"PHOTO"};
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
      // uses defaults for each filter
      // dateFilter = new DateFilter();
      contentFilter = new ContentFilter();
      mediaTypeFilter = new MediaTypeFilter();
      featureFilter = new FeatureFilter();
   }
}

public class MediaItemSearchRequest
{  
   public string albumId;
   public int pageSize;
   public string pageToken;
   [JsonProperty(PropertyName = "filters")]
   public MediaFilter mediaFilter;

   public string orderBy;

   public MediaItemSearchRequest(){
      mediaFilter = new MediaFilter();
      pageSize = 10;
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