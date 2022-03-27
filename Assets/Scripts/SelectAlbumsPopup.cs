using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectAlbumsPopup : MonoBehaviour
{    
    public Button closeButton;
    public Button loadPhotosButton;
    private Gallery gallery;

    // Start is called before the first frame update
    void Start()
    {
        closeButton.interactable = false;        
        gallery = GameObject.Find("Gallery Scroll View").GetComponent<Gallery>();
        loadPhotosButton.onClick.AddListener(loadPhotosButtonClicked);

    }

    void loadPhotosButtonClicked()
    {
        gallery.initPhotos();
        loadPhotosButton.interactable = false;
        closeButton.interactable = true;
    }
}
