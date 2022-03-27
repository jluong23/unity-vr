using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ImageInfoPanel : MonoBehaviour
{
    private Text fileNameText;
    private Text bodyText;
    public GameObject parentCanvas;
    public GameObject imageFramePrefab;
    public MediaItem mediaItem;

    public GameObject mainDisplay;

    // Start is called before the first frame update
    void Start()
    {   
        parentCanvas = transform.parent.gameObject;
        fileNameText = transform.Find("File Name Text").GetComponent<Text>();
        bodyText = transform.Find("Body Text").GetComponent<Text>();
        Close();
    }
    public void updateText(MediaItem mediaItem){
        fileNameText.text = mediaItem.filename;
        DateTime dateTime = Convert.ToDateTime(mediaItem.mediaMetadata.creationTime);
        string bodyFormat = "Categories: {0}\nDate: {1}\nLocation: {2}\nDescription: {3}"; 
        bodyText.text = string.Format(bodyFormat, 
            string.Join(",", mediaItem.categories), 
            dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString(),
            "Test",
            mediaItem.description
        );
    }

/// <summary>
/// When the close button is clicked on the image info panel
/// </summary>
    public void Close(){
        // gameObject.SetActive(false);
        // TODO: hide the element by moving it out of the scene
        parentCanvas.transform.position = new Vector3(0,200,0);
    }

/// <summary>
/// When the place button is clicked on the image info panel
/// </summary>
    public void Place()
    {
        GameObject instantiatedImageFrame = Instantiate(imageFramePrefab, mainDisplay.transform.position, Quaternion.identity);
        // rotate the image frame in direction of main display
        instantiatedImageFrame.transform.rotation = mainDisplay.transform.rotation;
        // set the texture of this thumbnails prefab
        instantiatedImageFrame.GetComponent<ImageFrame>().setTexture(mediaItem);

        Close();
    }
}
