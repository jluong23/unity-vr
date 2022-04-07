using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
public class ImageFrame : MonoBehaviour
{

    private GameObject image;
    public MediaItem mediaItem;
    public InputActionReference leftHandSelect;
    public InputActionReference rightHandSelect;
    private ImageInfoPanel imageInfoPanel;
    void Start()
    {
        imageInfoPanel = GameObject.Find("Image Info Panel").GetComponent<ImageInfoPanel>();

    }
    void Update()
    {
        if (leftHandSelect.action.IsPressed() || rightHandSelect.action.IsPressed())
        {
            imageInfoPanel.Show(gameObject);
        }
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
