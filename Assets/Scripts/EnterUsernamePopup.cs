using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterUsernamePopup : MenuPopup
{
    public User user;
    public Text usernameInputText;
    protected override void Start()
    {
        // don't use base.Start()
        base.Start();
        continueButton.onClick.AddListener(Login);
    }

    void Update()
    {
        if(usernameInputText.text == ""){
            continueButton.interactable = false;
        }else{
            continueButton.interactable = true;
        }
    }
    void Login(){
        user.Login(usernameInputText.text);
    }
}
