using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Linq;

public class SettingsMenu : SideMenu
{
    public Button changeUserButton;
    public Button deleteSaveButton;
    public Button quitButton;
    public User user;
    public LoadSavePopup loadSavePopup;
    public Text infoText;

    protected override void Start()
    {
        base.Start();
        quitButton.onClick.AddListener(quitButtonClicked);
        changeUserButton.onClick.AddListener(changeUserButtonClicked);
        deleteSaveButton.onClick.AddListener(delegate {StartCoroutine(deleteSaveButtonClicked());});

    }

    void changeUserButtonClicked(){
        loadSavePopup.transform.position = mainDisplay.transform.position;
        loadSavePopup.backButton.gameObject.SetActive(false); //hide the backbutton when load save popup is reused
        mainDisplay.Close();

    }

    IEnumerator deleteSaveButtonClicked(){
        if(user != null && user.username != ""){
            user.libraryPhotos.deleteSave(true, true);
            loadSavePopup.updateSaveButtons();
            yield return new WaitForSeconds(1f);
            changeUserButtonClicked();
        }
    }

    public void setInfoText(int numImages, string loadTimeString){
        infoText.text = string.Format("Load Time: {0}\n# Images: {1}", loadTimeString, numImages);
    }

    void quitButtonClicked(){
        Application.Quit();
    }
}
