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
    public Selectable thumbnailPrefab;
    private Selectable newThumbnail; // used to generate instances of thumbnail prefab
    public GameObject content;
    public User user;
    private List<MediaItem> currentPhotos;

    /// <summary>
    /// Check if photo variables have finished loading with categorisation every .5 seconds. Once loaded,
    /// update category and date menus and content grid on main display to reflect the loaded data.
    /// </summary>
    /// <returns></returns>
    IEnumerator photosLoadedCheck()
    {
        while(!user.photos.loaded){
            // keep waiting until all photos have loaded
            yield return new WaitForSeconds(.5f);
        }
        // update category menu with initial category counts
        categoryMenu.setToggles(user.photos.initialCategoryCounts);
        // update the start and end date ranges
        dateMenu.setMaxDateRanges(user.photos);
        populateGrid();
        // save the user data if a save does not exist
        if(!File.Exists(user.photos.savePath)){
            // wait a second, coroutines are funky at times
            yield return new WaitForSeconds(1f);
            user.photos.saveData();

        }
    }

    // ran when the 'show photos data' button is pressed for the first time
    public void initPhotos(){
        if(!user.photos.hasSave){
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
            List<string> selectedCategories = categoryMenu.getSelectedCategories();
            Tuple<DateTime, DateTime> currentDateRange = dateMenu.getCurrentDateRange();
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
                categoryMenu.setToggles(newCategoryCounts);
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
