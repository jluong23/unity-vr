using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Networking;
using System;
public class ImageFrame : MonoBehaviour
{
    //image frame can be scaled n times bigger or smaller for constraint = n
    public static float WIDTH_CONSTRAINT_MULTIPLIER = 2;
    private GameObject image;
    private ImageInfoPanel imageInfoPanel;
    public MediaItem mediaItem;
    private OffsetInteractable offsetInteractable;
    public static float SCALE_DOWN_FACTOR = 1000;
    public Tuple<Vector3, Vector3> localScaleRange;

    private void Awake()
    {
        offsetInteractable = GetComponent<OffsetInteractable>();
        offsetInteractable.interactionManager = GameObject.Find("XR Interaction Manager").GetComponent<XRInteractionManager>();
        imageInfoPanel = GameObject.Find("Image Info Panel").GetComponent<ImageInfoPanel>();
        offsetInteractable.activated.AddListener(delegate { Activated(); });
        localScaleRange = new Tuple<Vector3, Vector3>(transform.localScale / WIDTH_CONSTRAINT_MULTIPLIER, transform.localScale * WIDTH_CONSTRAINT_MULTIPLIER);

    }


    void Activated()
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
