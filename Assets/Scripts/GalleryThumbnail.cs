using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GalleryThumbnail : MonoBehaviour
{
    public MediaItem mediaItem;
    private GameObject imageInfoPanel;
    private ImageInfoPanel imageInfoComponent;
    private void Start() {
        imageInfoPanel = GameObject.Find("Image Info Panel");
        imageInfoComponent = imageInfoPanel.GetComponent<ImageInfoPanel>();
        GetComponent<Button>().onClick.AddListener(showImageInfoPanel);
    }
    private void logPhotoDetails(){
        string output = string.Format("{0}, taken {1}, categories: {2}",
        mediaItem.filename, mediaItem.mediaMetadata.creationTime, string.Join(",", mediaItem.categories));
        Debug.Log(output);
    }

    private void showImageInfoPanel()
    {
        GameObject parentCanvas = imageInfoComponent.parentCanvas;
        imageInfoPanel = Popup.Show(parentCanvas, gameObject, false);
        // pass on the mediaitem object
        imageInfoComponent.mediaItem = mediaItem;
        //set rotation as main display
        parentCanvas.transform.rotation = GameObject.Find("Main Display").transform.rotation;
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
            // append =d to the request to download the image
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(mediaItem.baseUrl + "=d");
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
