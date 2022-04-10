    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPhotosPopup : MenuPopup
{    
    public Button loadPhotosButton;
    private Gallery gallery;
    public User user;
    public Text bodyText;
    public Text sliderHandleValue;
    private Slider slider;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        continueButton.interactable = false;
        gallery = GameObject.Find("Gallery Scroll View").GetComponent<Gallery>();
        //buttons
        loadPhotosButton.onClick.AddListener(loadPhotosButtonClicked);
        //slider for max userphotos
        slider = GetComponentInChildren<Slider>();
        slider.onValueChanged.AddListener(delegate { maxPhotosSliderChanged(); });
        maxPhotosSliderChanged(); // run once to update slider value
    }
    void maxPhotosSliderChanged()
    {
        //Slider is currently 1-40, multiply by 100 for 100-4000 with steps of 100
        int newValue = (int)slider.value * 100;
        sliderHandleValue.text = newValue.ToString();
        if(user.photos != null){
            user.photos.maxPhotos = (int)newValue; // update maxphotos variable in user.photos
        }
    }

    void loadPhotosButtonClicked()
    {
        slider.gameObject.SetActive(false);
        gallery.initPhotos();
        loadPhotosButton.interactable = false;
        StartCoroutine(activateContinueButton());
    }

    IEnumerator activateContinueButton()
    {
        while(!user.photos.loaded){
            // wait until photos have loaded, updating progress on categories and photos loaded
            updateBodyText();
            yield return new WaitForSeconds(.5f);
        }
        updateBodyText();
        // activate continue button after all photos have loaded
        continueButton.interactable = true;
    }

    void updateBodyText(){
        int numTotalCategories = ContentFilter.ALL_CATEGORIES.Length;
        bodyText.text = string.Format("Photos loaded: {0}\nCategorisation: {1}/{2}", user.photos.allPhotos.Count, user.photos.categoriesLoaded, numTotalCategories);

    }
}
