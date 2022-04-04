using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPhotosPopup : MonoBehaviour
{    
    public Button closeButton;
    public Button loadPhotosButton;
    private Gallery gallery;
    public MainDisplay mainDisplay;
    public User user;
    public Text bodyText;
    public Text sliderHandleValue;
    private Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        closeButton.interactable = false;
        gallery = GameObject.Find("Gallery Scroll View").GetComponent<Gallery>();
        //buttons
        loadPhotosButton.onClick.AddListener(loadPhotosButtonClicked);
        closeButton.onClick.AddListener(closeButtonClicked);
        //slider for max userphotos
        slider = GetComponentInChildren<Slider>();
        slider.onValueChanged.AddListener(delegate { maxPhotosSliderChanged(); });
    }
    void maxPhotosSliderChanged()
    {
        int newValue = (int)slider.value * 100;
        user.photos.maxPhotos = newValue;
        sliderHandleValue.text = newValue.ToString();
    }

    void loadPhotosButtonClicked()
    {
        slider.gameObject.SetActive(false);
        gallery.initPhotos();
        loadPhotosButton.interactable = false;
        StartCoroutine(activateCloseButton());
    }

    IEnumerator activateCloseButton()
    {
        while(!user.photos.loaded){
            // wait until photos have loaded, updating progress on categories and photos loaded
            updateBodyText();
            yield return new WaitForSeconds(.5f);
        }
        updateBodyText();
        // activate close button after all photos have loaded
        closeButton.interactable = true;
    }

    void updateBodyText(){
        int numTotalCategories = ContentFilter.ALL_CATEGORIES.Length;
        bodyText.text = string.Format("Photos loaded: {0}\nCategorisation: {1}/{2}", user.photos.allPhotos.Count, user.photos.categoriesLoaded, numTotalCategories);

    }

    void closeButtonClicked(){
        mainDisplay.Show();
        gameObject.SetActive(false);
    }
}
