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
    private int numTotalCategories;

    // Start is called before the first frame update
    void Start()
    {
        numTotalCategories = ContentFilter.ALL_CATEGORIES.Length;
        closeButton.interactable = false;        
        gallery = GameObject.Find("Gallery Scroll View").GetComponent<Gallery>();
        loadPhotosButton.onClick.AddListener(loadPhotosButtonClicked);
        closeButton.onClick.AddListener(closeButtonClicked);
    }

    void loadPhotosButtonClicked()
    {
        gallery.initPhotos();
        loadPhotosButton.interactable = false;
        StartCoroutine(activateCloseButton());
    }

    IEnumerator activateCloseButton()
    {
        while(!user.photos.loaded){
            // wait until photos have loaded, updating progress
            updateBodyText();
            yield return new WaitForSeconds(.5f);
        }
        updateBodyText();
        // activate close button after all photos have loaded
        closeButton.interactable = true;
    }

    void updateBodyText(){
        bodyText.text = string.Format("Photos loaded: {0}\nCategorisation: {1}/{2}", user.photos.allPhotos.Count, user.photos.categoriesLoaded, numTotalCategories);

    }

    void closeButtonClicked(){
        mainDisplay.Show();
        gameObject.SetActive(false);
    }
}
