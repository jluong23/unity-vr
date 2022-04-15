using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ImageInfoPanel : MonoBehaviour
{
    private Text fileNameText;
    private Text bodyText;
    private GameObject instantiatedImageFrame;
    public GameObject parentCanvas;
    public GameObject imageFramePrefab;
    public MediaItem mediaItem;
    public GameObject placeImageFrameButton;
    public GameObject removeImageFrameButton;
    public GameObject closeButton;
    public GameObject mainDisplay;

    // Start is called before the first frame update
    void Start()
    {
        parentCanvas = transform.parent.gameObject;
        fileNameText = transform.Find("File Name Text").GetComponent<Text>();
        bodyText = transform.Find("Body Text").GetComponent<Text>();
        // button objects
        placeImageFrameButton.GetComponent<Button>().onClick.AddListener(placeButtonClicked);
        removeImageFrameButton.GetComponent<Button>().onClick.AddListener(removeButtonClicked);
        closeButton.GetComponent<Button>().onClick.AddListener(Close);
        //Start closed
        Close();
    }
    public void updateText(MediaItem mediaItem) {
        // Show file name in header
        fileNameText.text = mediaItem.filename;
        // create body info of panel
        DateTime dateTime = Convert.ToDateTime(mediaItem.mediaMetadata.creationTime);
        // dictionary from field string (date, albums...) to value string (the date taken, albums attached to...)
        Dictionary<string, string> bodyDict = new Dictionary<string, string>();
        bodyDict.Add("Categories", string.Join(",", mediaItem.categories));
        bodyDict.Add("Date Taken", dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString());

        if(mediaItem.description != null && mediaItem.description != ""){
            bodyDict.Add("Description", mediaItem.description);
        }
        string output = "";
        foreach (var item in bodyDict)
        {
            output += item.Key + ": " + item.Value + "\n";
        }
        bodyText.text = output;
    }

    /// <summary>
    /// When the close button is clicked on the image info panel
    /// </summary>
    public void Close() {
        // gameObject.SetActive(false);
        // TODO: hide the element by moving it out of the scene
        parentCanvas.transform.position = new Vector3(0, 200, 0);
    }

    /// <summary>
    /// When the place button is clicked on the image info panel
    /// </summary>
    public void placeButtonClicked()
    {
        instantiatedImageFrame = Instantiate(imageFramePrefab, mainDisplay.transform.position - 0.2f * mainDisplay.transform.forward, Quaternion.identity);
        // rotate the image frame in direction of main display
        instantiatedImageFrame.transform.rotation = mainDisplay.transform.rotation;
        // scale the image frame to match the aspect ratio of the full texture
        float xScale = float.Parse(mediaItem.mediaMetadata.width) / ImageFrame.SCALE_DOWN_FACTOR;
        float yScale = float.Parse(mediaItem.mediaMetadata.height) / ImageFrame.SCALE_DOWN_FACTOR;
        instantiatedImageFrame.transform.localScale = new Vector3(xScale,yScale,transform.localScale.z);

        // set the texture of this image frame prefab
        ImageFrame imageFrameComponent = instantiatedImageFrame.GetComponent<ImageFrame>();
        StartCoroutine(imageFrameComponent.setFullTextureCoroutine(mediaItem));
        //close the main display and this info panel when a frame is spawned in
        Close();
        mainDisplay.GetComponent<MainDisplay>().Close();
    }

    public void removeButtonClicked()
    {
        Close();
        //remove the associated image frame
        GameObject.Destroy(instantiatedImageFrame);
    }

    public void Show(GameObject objectClicked)
    {
        ImageFrame imageFrameComponent = objectClicked.GetComponent<ImageFrame>();
        GalleryThumbnail galleryThumbnailComponent = objectClicked.GetComponent<GalleryThumbnail>();
        
        if (imageFrameComponent != null)
        {
            mediaItem = imageFrameComponent.mediaItem;
            //Show the remove button, hide the place button
            removeImageFrameButton.SetActive(true);
            placeImageFrameButton.SetActive(false);
        }
        else if(galleryThumbnailComponent != null)
        {
            mediaItem = galleryThumbnailComponent.mediaItem;
            //Show the place button, hide the remove button
            placeImageFrameButton.SetActive(true);
            removeImageFrameButton.SetActive(false);
        }
        if(mediaItem != null)
        {
            //spawn the image info panel in front of the object
            Transform parentTransform = parentCanvas.transform;
            parentTransform.position = objectClicked.transform.position + objectClicked.transform.forward*-1f;
            parentTransform.rotation = objectClicked.transform.rotation;
            // update the text for the selected thumbnail
            updateText(mediaItem);
        }
    }
}
