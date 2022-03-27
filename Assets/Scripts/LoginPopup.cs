using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPopup : MonoBehaviour
{
    private Button button;
    public GameObject selectAlbumsPopup; 
    public InputField emailInput;
    public User user;
    // Start is called before the first frame update
    void Start()
    {
        emailInput.onValueChanged.AddListener(emailInputChanged);
        button = GetComponentInChildren<Button>();
        button.interactable = false;
        button.onClick.AddListener(buttonClicked);
    }

    void buttonClicked(){
        selectAlbumsPopup.transform.position = gameObject.transform.position;
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
