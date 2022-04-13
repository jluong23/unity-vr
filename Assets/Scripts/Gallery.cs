using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.IO;

public class Gallery : MonoBehaviour
{
    public DateMenu dateMenu;
    public CategoryMenu categoryMenu;
    public GameObject thumbnailPrefab;
    private GameObject newThumbnail; // used to generate instances of thumbnail prefab
    public GameObject content;
    public User user;
    public GalleryScroller galleryScroller;
    private List<MediaItem> currentPhotos;

    /// <summary>
    /// Check if photo variables have finished loading with categorisation every .5 seconds. Once loaded,
    /// update category and date menus and content grid on main display to reflect the loaded data.
    /// </summary>
    /// <returns></returns>
    IEnumerator photosLoadedCheck()
    {
        while(!user.libraryPhotos.loaded){
            // keep waiting until all photos have loaded
            yield return new WaitForSeconds(.5f);
        }
        // update category menu with initial category counts
        categoryMenu.setToggles(user.libraryPhotos.initialCategoryCounts);
        // update the start and end date ranges
        dateMenu.setMaxDateRanges(user.libraryPhotos);
        populateGrid();
        // save the user data
        user.libraryPhotos.saveData();

    }

    // ran when the 'show photos data' button is pressed for the first time
    public void initPhotos(){
        // load the user data if a save does not exist
        if(!user.libraryPhotos.hasSave){
            user.populatePhotosByAPI();
        }
        StartCoroutine(photosLoadedCheck());
    }

	void populateGrid()
	{
        /// <summary>
        /// Populate gallery with all images
        /// </summary>
        /// <returns></returns>
        currentPhotos = user.libraryPhotos.getPhotos();
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
        if(user != null && user.libraryPhotos.getPhotos().Count > 0){
            // get photos with selected categories and time
            List<string> selectedCategories = categoryMenu.getSelectedCategories();
            Tuple<DateTime, DateTime> currentDateRange = dateMenu.getCurrentDateRange();
            // find the new photos given the current category and date selection
            List<MediaItem> newPhotos = user.libraryPhotos.getPhotos(selectedCategories, currentDateRange);
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
                Dictionary<string, int> newCategoryCounts = user.libraryPhotos.getCategoryCounts(selectedCategories, currentDateRange); 
                categoryMenu.setToggles(newCategoryCounts);
                // scroll to top of gallery
                galleryScroller.scrollToTop();
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
