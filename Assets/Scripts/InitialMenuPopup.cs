using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitialMenuPopup : MenuPopup
{
    private Button startButton;
    public Camera mainCamera;
    public User user;
    // Start is called before the first frame update
    protected override void Start()
    {
        //move to front of camera
        base.Start();
        user.appearObject(gameObject, 2.5f, .8f, false);
        gameObject.SetActive(true);
    }
}

