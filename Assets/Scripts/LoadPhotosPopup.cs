    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LoadPhotosPopup : MenuPopup
{    
    public Button loadPhotosButton;
    private Gallery gallery;
    public User user;
    public Text bodyText;
    public Text sliderHandleValue;
    public Button loadOrderButton;
    public Text loadOrderText;
    public Toggle loadVideosToggle;
    private Slider slider;
    private bool defaultSliderValueSet = false;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        gallery = GameObject.Find("Gallery Scroll View").GetComponent<Gallery>();
        //buttons
        loadOrderButton.onClick.AddListener(loadOrderButtonClicked);
        loadVideosToggle.onValueChanged.AddListener(loadVideosToggleClicked);
        loadPhotosButton.onClick.AddListener(loadPhotosButtonClicked);
        //slider for max userphotos
        slider = GetComponentInChildren<Slider>();
        slider.onValueChanged.AddListener(delegate { maxPhotosSliderChanged(); });
        maxPhotosSliderChanged(); // run once to update slider value
    }

    private void Update()
    {
        if(!defaultSliderValueSet && user.loggedIn)
        {
            //used set user.libraryPhotos.maxPhotos to the default slider value

            defaultSliderValueSet = true;
            maxPhotosSliderChanged();
        }
    }

    void loadOrderButtonClicked(){
        string[] toggleLabels = {"Newest First", "Oldest First"};
        int newIndex = (Array.IndexOf(toggleLabels, loadOrderText.text) + 1) % 2;
        loadOrderText.text = toggleLabels[newIndex];
        user.libraryPhotos.loadOrder = newIndex == 0 ? UserPhotos.LoadOrder.NEWEST_FIRST : UserPhotos.LoadOrder.OLDEST_FIRST;

    }

    void loadVideosToggleClicked(bool toggleVal){
        user.libraryPhotos.loadVideos = toggleVal;
    }
    void maxPhotosSliderChanged()
    {
        //Slider is currently 1 to x scale, multiply by 100 for 100-100*x scale, with steps of 100
        int newValue = (int)slider.value * 100;
        sliderHandleValue.text = newValue.ToString();
        if(user.libraryPhotos != null){
            user.libraryPhotos.maxPhotos = (int)newValue; // update maxphotos variable in user.libraryPhotos
        }
    }

    void loadPhotosButtonClicked()
    {
        // load button pressed, swap slider and other options with bodyText for load progress
        loadOrderButton.gameObject.SetActive(false);
        loadVideosToggle.gameObject.SetActive(false);
        slider.gameObject.SetActive(false);
        updateBodyText();
        backButton.interactable = false;
        // start loading gallery images
        gallery.initPhotos();
        loadPhotosButton.interactable = false;
        StartCoroutine(whilstImagesLoading());
    }

    IEnumerator whilstImagesLoading()
    {
        while(!user.libraryPhotos.loaded){
            // wait until photos have loaded, updating progress on categories and photos loaded
            updateBodyText();
            yield return new WaitForSeconds(.5f);
        }
        // although this panel has no continue button, simulate pressing continue from MenuPopup parent class,
        // moving on to main display
        continueButtonClicked();
    }   

    void updateBodyText(){
        int numTotalCategories = ContentFilter.ALL_CATEGORIES.Length;
        bodyText.text = string.Format("Photos loaded: {0}\nCategorisation: {1}/{2}", user.libraryPhotos.allPhotos.Count, user.libraryPhotos.categoriesLoaded, numTotalCategories);

    }
}
