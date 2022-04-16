using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EnterUsernamePopup : MenuPopup
{
    public User user;
    public InputField usernameInput;
    public Text usernameInputText;
    public VRKeyboard keyboard;
    protected override void Start()
    {
        // don't use base.Start()
        base.Start();
        continueButton.onClick.AddListener(Login);
        continueButton.interactable = false;
        usernameInput.onValueChanged.AddListener(usernameInputChanged);
    }

    private void Update()
    {
        if (usernameInput.isFocused)
        {
            keyboard.Open(usernameInput);
        }
    }

    void usernameInputChanged(string currentInputText)
    {
        if (currentInputText == ""){
            continueButton.interactable = false;
        }else{
            continueButton.interactable = true;
        }
    }
    void Login(){
        user.Login(usernameInputText.text);
    }
}
