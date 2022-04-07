using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
public class ImageFrame : MonoBehaviour
{

    private GameObject image;
    private ImageInfoPanel imageInfoPanel;
    public MediaItem mediaItem;
    private OffsetInteractable offsetInteractable;
    private XRRayInteractor xrRayInteractor;


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
    public void setTexture(MediaItem mediaItem)
    {
        image = transform.Find("Image").gameObject;
        if (mediaItem != null)
        {
            this.mediaItem = mediaItem;
            image.GetComponent<Renderer>().material.mainTexture = mediaItem.texture;
        }
    }
}
