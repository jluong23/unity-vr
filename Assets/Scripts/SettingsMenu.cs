using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SettingsMenu : SideMenu
{
    public Button changeUserButton;
    public Button quitButton;
    public User user;
    public LoadSavePopup loadSavePopup;

    protected override void Start()
    {
        base.Start();
        quitButton.onClick.AddListener(quitButtonClicked);
        changeUserButton.onClick.AddListener(changeUserButtonClicked);
    }

    void changeUserButtonClicked(){
        loadSavePopup.transform.position = mainDisplay.transform.position;
        loadSavePopup.backButton.gameObject.SetActive(false); //hide the backbutton when load save popup is reused
        mainDisplay.Close();

    }

    void quitButtonClicked(){
        Application.Quit();
    }
}
