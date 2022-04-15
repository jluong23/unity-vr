using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Networking;
using System;

using UnityEngine.InputSystem;
public class ImageFrame : MonoBehaviour
{

    private GameObject image;
    private ImageInfoPanel imageInfoPanel;
    public MediaItem mediaItem;
    private OffsetInteractable offsetInteractable;
    private XRRayInteractor xrRayInteractor;
    public static float SCALE_DOWN_FACTOR = 1000;

    private void Awake()
    {
        offsetInteractable = GetComponent<OffsetInteractable>();
        offsetInteractable.interactionManager = GameObject.Find("XR Interaction Manager").GetComponent<XRInteractionManager>();
        xrRayInteractor = GameObject.Find("LeftHand Controller").GetComponent<XRRayInteractor>();
        imageInfoPanel = GameObject.Find("Image Info Panel").GetComponent<ImageInfoPanel>();
        //when the user selects an image frame with a trigger, show image panel
        offsetInteractable.activated.AddListener(delegate { showImagePanel(); });
    }

    public void showImagePanel()
    {
        imageInfoPanel.Show(gameObject);
    }
    public IEnumerator setFullTextureCoroutine(MediaItem mediaItem)
    {
        image = transform.Find("Image").gameObject;
        this.mediaItem = mediaItem;

        if(mediaItem.fullTexture == null){
            // append =d to the request to download the full image
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(mediaItem.baseUrl + "=d");
            // yield return: returns to main thread whilst sending the request
            yield return request.SendWebRequest();
            if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                Debug.Log(request.error);
            else{
                var texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
                mediaItem.fullTexture = texture;
            }
        }
        // set the full texture
        image.GetComponent<Renderer>().material.mainTexture = mediaItem.fullTexture;
    }
}
