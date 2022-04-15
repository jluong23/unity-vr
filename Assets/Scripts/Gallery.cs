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
    public GameObject monthSectionPrefab;
    public GameObject monthTextPrefab;
    private GameObject newThumbnail; // used to generate instances of thumbnail prefab
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
        while(!user.libraryPhotos.loaded){
            // keep waiting until all photos have loaded
            yield return new WaitForSeconds(.5f);
        }
        // update category menu with initial category counts
        categoryMenu.setToggles(user.libraryPhotos.initialCategoryCounts);
        // update the start and end date ranges
        dateMenu.setMaxDateRanges(user.libraryPhotos);
        populateGrid(user.libraryPhotos.getPhotos());
        // save the user data
        user.libraryPhotos.saveData();

    }

    // ran when the 'show photos data' button is pressed for the first time
    public void initPhotos(){
        clearGallery();
        // load the user data if a save does not exist
        if(!user.libraryPhotos.hasSave){
            user.populatePhotosByAPI();
        }
        StartCoroutine(photosLoadedCheck());
    }

	void populateGrid(List<MediaItem> newPhotos)
	{
        /// <summary>
        /// Populate gallery with the new photos (use user.libraryPhotos.getPhotos() to show all..)
        /// </summary>
        /// <returns></returns>
        currentPhotos = newPhotos;
        int prevMonth = 0;
        GameObject monthSection = null;
		foreach (var photo in currentPhotos)
		{
            DateTime photoDate = Convert.ToDateTime(photo.mediaMetadata.creationTime);

            if(photoDate.Month != prevMonth){
                // this picture has a different month then last seen. Create a date header
                GameObject monthText = Instantiate(monthTextPrefab, content.transform);
                monthText.GetComponent<Text>().text = photoDate.ToString("MMMM yyyy");
                // start a new gallery section for this month
                monthSection = Instantiate(monthSectionPrefab, content.transform);
            }
			 // Create new instances of our thumbnailPrefab under month section
			newThumbnail = Instantiate(thumbnailPrefab, monthSection.transform);
			newThumbnail.GetComponent<GalleryThumbnail>().displayTexture(photo);
            // update prevMonth
            prevMonth = photoDate.Month;
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
                // load new photos into gallery grid
                populateGrid(newPhotos);
                // update category menu, reflecting new category counts
                Dictionary<string, int> newCategoryCounts = user.libraryPhotos.getCategoryCounts(selectedCategories, currentDateRange); 
                categoryMenu.setToggles(newCategoryCounts);
                // scroll to top of gallery
                GetComponentInChildren<GalleryScroller>().scrollToTop();
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
