using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class LoadSavePopup : MenuPopup
{
    private GameObject[] saveButtons;
    public GameObject newSavePopup;
    public GameObject existingSavePopup;
    public User user;
    private List<string> usernames;
    protected override void Start()
    {
        //dont call base.Start()
        // assign back button
        backButton.onClick.AddListener(backButtonClicked);
        saveButtons = GameObject.FindGameObjectsWithTag("Save Button");
        // find usernames in oauth folder
        usernames = new List<string>();
        // exclude meta files
        FileInfo[] oauthFiles = new DirectoryInfo(User.OAUTH_SAVE_PATH).GetFiles().Where(f => !f.Name.EndsWith(".meta")).ToArray();
        foreach (FileInfo file in oauthFiles)
        {
            string username = file.Name;
            usernames.Add(username.Substring(username.IndexOf("-")+1));
        }
        // map event listeners to save buttons
        for (int i = 0; i < saveButtons.Length; i++)
        {
            Button saveButton = saveButtons[i].GetComponent<Button>();
            string username = i < usernames.Count ? usernames[i] : null;
            if(username == null){
                saveButton.onClick.AddListener(newSave);
            }else{
                saveButton.GetComponentInChildren<Text>().text = username;
                saveButton.onClick.AddListener(delegate {loadSave(username);});
            }
        }
        gameObject.SetActive(false);

    }

    void loadSave(string username){
        user.Login(username);
        nextPopup = existingSavePopup;
        continueButtonClicked();
    }

    void newSave(){
        nextPopup = newSavePopup;
        continueButtonClicked();
    }
}
