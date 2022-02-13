using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.InputSystem;

public class MediaFrame : MonoBehaviour
{
    private Texture downloadedTexture;
    private Texture originalTexture;
    public MediaItem mediaItem;
    public InputActionReference selectObjectReference = null;

    void Start() {
        // save the default texture
        originalTexture = transform.GetChild(0).GetComponent<Renderer>().material.GetTexture("_MainTex");
    }
   
    public void restoreTexture() {
        mediaItem = null;
        transform.GetChild(0).GetComponent<Renderer>().material.SetTexture("_MainTex", originalTexture);
    }
    
    public void displayTexture(){    
        // changes the image displayed on the frame
        if(mediaItem != null){
            StartCoroutine(SetTextureCoroutine());
        }
    }

    private IEnumerator SetTextureCoroutine()
    {
        // append =d to the request to download the image
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(mediaItem.baseUrl + "=d");
        // yield return: returns to main thread whilst sending the request
        yield return request.SendWebRequest();
        if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            Debug.Log(request.error);
        else
            downloadedTexture = ((DownloadHandlerTexture) request.downloadHandler).texture;
            // change the texture to the downloaded image (gameObject child is the quad where the texture is displayed)
            transform.GetChild(0).GetComponent<Renderer>().material.SetTexture("_MainTex", downloadedTexture);
    }     
}
