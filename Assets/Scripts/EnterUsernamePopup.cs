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
        gameObject.SetActive(false);
        continueButton.onClick.AddListener(Login);
    }

    void Login(){
        user.Login(usernameInputText.text);
        base.continueButtonClicked(); //close this menu and open the next page
    }
}
