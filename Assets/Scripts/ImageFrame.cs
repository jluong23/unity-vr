using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageFrame : MonoBehaviour
{

    private GameObject image;
    private MediaItem mediaItem;

    public void setTexture(MediaItem mediaItem)
    {
        image = transform.Find("Image").gameObject;
        if (mediaItem != null)
        {
            this.mediaItem = mediaItem;
            image.GetComponent<Renderer>().material.mainTexture = mediaItem.texture;
        }
    }
}
