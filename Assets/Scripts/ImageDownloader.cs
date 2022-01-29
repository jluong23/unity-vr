using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class ImageDownloader : MonoBehaviour
{
    private Texture downloadedTexture;
    
    public void setTexture(string mediaPhotoUrl){    
        if(mediaPhotoUrl != ""){
            StartCoroutine(DownloadImage(mediaPhotoUrl));
        }
    }

    IEnumerator DownloadImage(string mediaPhotoUrl)
    {
        // append =d to the request to download the image
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(mediaPhotoUrl + "=d");
        yield return request.SendWebRequest();
        if(request.isNetworkError || request.isHttpError) 
            Debug.Log(request.error);
        else
            downloadedTexture = ((DownloadHandlerTexture) request.downloadHandler).texture;
            // change the texture to the downloaded image
            transform.GetChild(0).GetComponent<Renderer>().material.SetTexture("_MainTex", downloadedTexture);
    } 
}
