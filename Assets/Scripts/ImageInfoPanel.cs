using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageInfoPanel : MonoBehaviour
{
    private Text fileNameText;
    private Text bodyText;
    private GameObject parentCanvas;
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
        string bodyFormat = "Categories: {0}\nDate: {1}\nLocation: {2}\nDescription: {3}"; 
        bodyText.text = string.Format(bodyFormat, 
            string.Join(",", mediaItem.categories), 
            mediaItem.mediaMetadata.creationTime,
            "Test",
            mediaItem.description
        );
    }

    public void Close(){
        // gameObject.SetActive(false);
        // TODO: hide the element by moving it out of the scene
        parentCanvas.transform.position = new Vector3(0,200,0);
    }

    public void Place()
    {
        // spawn an image frame in front of the main display menu
        //GameObject instantiatedImageFrame = Popup.Show(imageFramePrefab, mainDisplay, true, 0.1f);

        //// set the texture of this thumbnails prefab
        //instantiatedImageFrame.GetComponent<ImageFrame>().setTexture(mediaItem);

        //Close();
    }
}
