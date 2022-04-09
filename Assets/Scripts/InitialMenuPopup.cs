using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitialMenuPopup : MenuPopup
{
    private Button startButton;
    public GameObject mainCamera;
    // Start is called before the first frame update
    protected override void Start()
    {
        //move to front of camera
        base.Start();
        transform.position = mainCamera.transform.position + mainCamera.transform.forward * 3f + new Vector3(0,1f,0);
        gameObject.SetActive(true);
    }
}

