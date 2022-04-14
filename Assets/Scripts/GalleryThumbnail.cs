using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GalleryThumbnail : MonoBehaviour
{
    public MediaItem mediaItem;
    private ImageInfoPanel imageInfoPanel;

    private void Start() {
        imageInfoPanel = GameObject.Find("Image Info Panel").GetComponent<ImageInfoPanel>();
        GetComponentInChildren<Button>().onClick.AddListener(() => imageInfoPanel.Show(gameObject));
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
                GetComponentInChildren<RawImage>().texture = texture;

            }
        }else{
            GetComponentInChildren<RawImage>().texture = mediaItem.texture;
        }
    } 
}
