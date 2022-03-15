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
    private Popup imageInfoPopup;

    private void Start() {
        imageInfoPanel = GameObject.Find("Image Info Panel");
        imageInfoPopup = imageInfoPanel.GetComponentInParent<Popup>();
        imageInfoComponent = imageInfoPanel.GetComponent<ImageInfoPanel>();
        GetComponent<Button>().onClick.AddListener(showImageInfoPanel);
    }
    private void showImageInfoPanel()
    {
        // pass through media item object to info panel
        imageInfoComponent.mediaItem = mediaItem;
        // show info panel in front of this gallery thumbnail
        imageInfoPopup.coveredObject = gameObject;
        imageInfoPopup.Show(false);
        ////set rotation of info panel, same as main display
        //parentCanvas.transform.rotation = GameObject.Find("Main Display").transform.rotation;
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
