using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gallery : MonoBehaviour
{
    private List<string> currentPhotoIds;

    public void showPhotos(List<string> newPhotoIds){
        // update currentPhotosIds
        currentPhotoIds = newPhotoIds;
        int i = 0;
        foreach (var mediaPhotoId in currentPhotoIds)
        {
            MediaItem mediaPhoto = GoogleMainProgram.allPhotos[mediaPhotoId];
            MediaFrame mediaFrameComponent = transform.GetChild(i).GetComponent<MediaFrame>();
            // set the mediaItem attribute for the mediaFrame component
            mediaFrameComponent.mediaItem = mediaPhoto;
            // set the texture of frame, showing the image itself
            mediaFrameComponent.displayTexture();
            i+=1;
        }
    }

    public void clearGallery(){
        currentPhotoIds = new List<string>();
        foreach (var mediaFrame in GetComponentsInChildren<MediaFrame>())
        {
            mediaFrame.restoreTexture();
        }
    }
}
