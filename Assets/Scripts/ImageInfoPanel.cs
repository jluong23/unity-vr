using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageInfoPanel : MonoBehaviour
{
    private Text fileNameText;
    private Text bodyText;

    // Start is called before the first frame update
    void Start()
    {
        fileNameText = transform.Find("File Name Text").GetComponent<Text>();
        bodyText = transform.Find("Body Text").GetComponent<Text>();
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
}
