using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPopup : MonoBehaviour
{
    private Button button;
    public GameObject loadPhotosPopup; 
    public InputField emailInput;
    public User user;
    public GameObject mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        //move to front of camera
        transform.position = mainCamera.transform.position + mainCamera.transform.forward * 3f + new Vector3(0,1f,0);
        emailInput.onValueChanged.AddListener(emailInputChanged);
        button = GetComponentInChildren<Button>();
        button.interactable = false;
        button.onClick.AddListener(buttonClicked);
        // TODO: default login for testing
        emailInput.text = "jluong1@sheffield.ac.uk";
        //emailInput.text = "jamesluong@hotmail.co.uk";

        //TODO: hide loadPhotos popup (which should be after clicking logging in)
        loadPhotosPopup.transform.position = new Vector3(0,200,0);
    }

    void buttonClicked(){
        loadPhotosPopup.transform.position = gameObject.transform.position;
        user.Login(emailInput.text);
    }

    void emailInputChanged(string t){
        if (emailInput.text.IndexOf('@') <= 0)
        {
            button.interactable = false;
        }else{
            button.interactable = true;

        }
    }
}
