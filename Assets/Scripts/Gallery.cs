using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Gallery : MonoBehaviour
{
    public GameObject menu;
    // array of mediaItem ids for currently shown photos on gallery
    List<string> currentPhotoIds;
    private UserPhotos userPhotos;
    public string email = "jluong1@sheffield.ac.uk";
    public bool categorisePhotos = true;

    // ran when the 'show photos data' button is pressed for the first time, updating category counts in category menu
    public void initPhotos(){
        currentPhotoIds = new List<string>();
        userPhotos = new UserPhotos(email, true);
        if(userPhotos.allPhotos.Count > 0){
            // update category counts for each category
            menu.GetComponent<CategoryMenu>().appendCategoryCounts(userPhotos.categoryCounts);
            // update the start and end date ranges
            menu.GetComponent<DateMenu>().setMaxDateRanges(userPhotos);
        }
    }

    public void updatePhotos(){
        /// <summary>
        /// update the gallery with photos with the selected categories from the category and date menus
        /// </summary>

        // get ids of the photos with selected categories
        List<string> selectedCategories = menu.GetComponent<CategoryMenu>().getSelectedCategories();
        
        List<string> newPhotoIds = userPhotos.getPhotoIds(selectedCategories);

        // only update photos if the ids are different
        if(!Enumerable.SequenceEqual(newPhotoIds, currentPhotoIds)) {
            // clear current gallery
            clearGallery();
            // update currentPhotoIds
            currentPhotoIds = new List<string>(newPhotoIds);
            int i = 0;

            foreach (var photoId in currentPhotoIds)
            {
                // start setting the media frames
                MediaItem mediaItem = userPhotos.allPhotos[photoId];
                MediaFrame mediaFrameComponent = transform.GetChild(i).GetComponent<MediaFrame>();
                // set the mediaItem attribute for the mediaFrame component
                mediaFrameComponent.mediaItem = mediaItem;
                // set the texture of frame, showing the image itself
                mediaFrameComponent.displayTexture();
                i+=1;
            }
            
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
