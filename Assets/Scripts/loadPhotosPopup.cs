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
    public Button previousPanelButton;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        continueButton.interactable = false;
        gallery = GameObject.Find("Gallery Scroll View").GetComponent<Gallery>();
        //buttons
        loadPhotosButton.onClick.AddListener(loadPhotosButtonClicked);
        previousPanelButton.onClick.AddListener(onDisplay); //when this panel is shown (previous panel moves to this one)
        //slider for max userphotos
        slider = GetComponentInChildren<Slider>();
        slider.onValueChanged.AddListener(delegate { maxPhotosSliderChanged(); });
    }

    void onDisplay()
    {
        // set default value
        maxPhotosSliderChanged(); //run this once to update the starting handle value
        if (user.photos.hasSave)
        {
            // skip the load button and max slider if a save exists
            loadPhotosButtonClicked();
        }
    }

    void maxPhotosSliderChanged()
    {
        //want 0 from 2000 in steps of 20. Multiply value from 0 to 20 by 100
        int newValue = (int)slider.value * 100;
        user.photos.maxPhotos = newValue;
        sliderHandleValue.text = newValue.ToString();
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
