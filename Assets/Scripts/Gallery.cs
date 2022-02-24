using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class Gallery : MonoBehaviour
{
    public GameObject menu;
    public string email = "jluong1@sheffield.ac.uk";
    public bool categorisePhotos = true;
    public Selectable thumbnailPrefab;
    private Selectable newThumbnail; // used to generate instances of thumbnail prefab
    public GameObject content;
    private User user;
    private List<MediaItem> currentPhotos;

    
    void Start()
    {
        user = new User(email);
    }

    // ran when the 'show photos data' button is pressed for the first time, updating category counts in category menu
    public void initPhotos(){
        if(user.photos.getPhotos().Count > 0){
            // update category counts for each category
            menu.GetComponent<CategoryMenu>().setToggles(user.photos.getInitialCategoryCounts());
            // update the start and end date ranges
            menu.GetComponent<DateMenu>().setMaxDateRanges(user.photos);
            populateGrid();
        }
    }

	void populateGrid()
	{
        /// <summary>
        /// Populate gallery with all images
        /// </summary>
        /// <returns></returns>
        currentPhotos = user.photos.getPhotos();
		foreach (var photo in currentPhotos)
		{
			 // Create new instances of our thumbnailPrefab until we've created as many as we specified
			newThumbnail = Instantiate(thumbnailPrefab, content.transform);
			newThumbnail.GetComponent<GalleryThumbnail>().displayTexture(photo);
		}
	}

    public void updateGallery(){
        /// <summary>
        /// update the gallery with photos with the selected categories and date ranges from the category and date menus
        /// </summary>
        if(user != null && user.photos.getPhotos().Count > 0){
            // get photos with selected categories and time
            List<string> selectedCategories = menu.GetComponent<CategoryMenu>().getSelectedCategories();
            Tuple<DateTime, DateTime> currentDateRange = menu.GetComponent<DateMenu>().getCurrentDateRange();
            List<MediaItem> newPhotos = user.photos.getPhotos(selectedCategories, currentDateRange);
            // only update photos if the photos are different
            if(!Enumerable.SequenceEqual(newPhotos, currentPhotos)) {
                clearGallery();
                // load new photos into current
                currentPhotos = new List<MediaItem>(newPhotos);
                foreach (var photo in currentPhotos)
                {
                    // Create new instances of our thumbnailPrefab until we've created as many as we specified
                    newThumbnail = Instantiate(thumbnailPrefab, content.transform);
                    newThumbnail.GetComponent<GalleryThumbnail>().displayTexture(photo);
                }
                // update category menu, reflecting new category counts
                Dictionary<string, int> newCategoryCounts = user.photos.getCategoryCounts(selectedCategories, currentDateRange); 
                menu.GetComponent<CategoryMenu>().setToggles(newCategoryCounts);
            }

        }
    }

    public void clearGallery(){
        currentPhotos = new List<MediaItem>();
        foreach (Transform child in content.transform) {
            Destroy(child.gameObject);
        }
    }
}
