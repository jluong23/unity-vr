using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitialPopup : MonoBehaviour
{
    private Button startButton;
    public GameObject loadPhotosPopup; 
    public User user;
    public GameObject mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        //move to front of camera
        transform.position = mainCamera.transform.position + mainCamera.transform.forward * 3f + new Vector3(0,1f,0);
        startButton = GetComponentInChildren<Button>();
        startButton.onClick.AddListener(buttonClicked);

        //TODO: hide loadPhotos popup (which should be after clicking logging in)
        loadPhotosPopup.transform.position = new Vector3(0,200,0);
    }

    void buttonClicked(){
        loadPhotosPopup.transform.position = gameObject.transform.position;
        user.Login();
    }
}
