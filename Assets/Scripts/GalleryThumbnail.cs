using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GalleryThumbnail : MonoBehaviour
{
    public MediaItem mediaItem;
    private GameObject imageInfoPanel;
    private void Start() {
        imageInfoPanel = GameObject.Find("Image Info Panel");
        GetComponent<Button>().onClick.AddListener(showImageInfoPanel);
    }
    private void showImageInfoPanel()
    {
        ImageInfoPanel imageInfoComponent = imageInfoPanel.GetComponent<ImageInfoPanel>();
        // pass through media item object to info panel
        imageInfoComponent.mediaItem = mediaItem;
        // show info panel in front of this gallery thumbnail
        Transform infoPanelParentTransform = imageInfoComponent.parentCanvas.transform;
        infoPanelParentTransform.position = transform.position;
        infoPanelParentTransform.rotation = transform.rotation;
        // update the text for the selected thumbnail
        imageInfoComponent.updateText(mediaItem);
    }


    public void displayTexture(MediaItem mediaItem){    
        // changes the image displayed on the frame
        this.mediaItem = mediaItem;
        if(this.mediaItem != null){
            StartCoroutine(SetTextureCoroutine());
        }
    }

    private IEnumerator SetTextureCoroutine()
    {
        if(mediaItem.texture == null){
            // append =d to the request to download the image.
            // width and height constraints for thumbnail also requested
            
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(mediaItem.baseUrl + string.Format("=w{0}-h{1}-c-d", MediaItem.THUMBNAIL_WIDTH, MediaItem.THUMBNAIL_HEIGHT));
            // yield return: returns to main thread whilst sending the request
            yield return request.SendWebRequest();
            if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                Debug.Log(request.error);
            else{
                var texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
                mediaItem.texture = texture;
                GetComponent<RawImage>().texture = texture;

            }
        }else{
            GetComponent<RawImage>().texture = mediaItem.texture;
        }
    } 
}
