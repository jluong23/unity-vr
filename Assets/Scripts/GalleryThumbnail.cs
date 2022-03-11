using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GalleryThumbnail : MonoBehaviour
{
    public MediaItem mediaItem;
    public GameObject imageFramePrefab;
    private void Start() {
        GetComponent<Button>().onClick.AddListener(spawnImageFrame);

    }

    private void logPhotoDetails(){
        string output = string.Format("{0}, taken {1}, categories: {2}",
        mediaItem.filename, mediaItem.mediaMetadata.creationTime, string.Join(",", mediaItem.categories));
        Debug.Log(output);
    }

    private void spawnImageFrame()
    {
        //spawn the image frame in front of the thumbnail
        Vector3 spawnPoint = transform.position + transform.forward * -2f;
        GameObject instantiatedImageFrame = Instantiate(imageFramePrefab, spawnPoint, Quaternion.identity);
        //rotate the image frame in same direction as ui display direction
        instantiatedImageFrame.transform.rotation = GameObject.Find("Main Display").transform.rotation;

        // set the texture of this thumbnails prefab
        instantiatedImageFrame.GetComponent<ImageFrame>().setTexture(mediaItem);
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
