using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitialMenuPopup : MonoBehaviour
{
    private Button startButton;
    public GameObject nextPopup; 
    public GameObject mainCamera;
    public User user;
    // Start is called before the first frame update
    void Start()
    {
        //move to front of camera
        transform.position = mainCamera.transform.position + mainCamera.transform.forward * 3f + new Vector3(0,1f,0);
        startButton = GetComponentInChildren<Button>();
        startButton.onClick.AddListener(buttonClicked);

        //TODO: hide nextPopup (which should be after clicking logging in)
        nextPopup.transform.position = new Vector3(0,200,0);
    }

    void buttonClicked(){
        // replace next popup with this popup
        nextPopup.transform.position = gameObject.transform.position;
        gameObject.SetActive(false);
        user.Login();
    }
}
