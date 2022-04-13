using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class LoadSavePopup : MenuPopup
{
    private GameObject[] saveButtons;
    public GameObject loadPhotosPopup;
    public GameObject mainDisplay;
    private Gallery gallery;

    public User user;
    private List<string> usernames;
    protected override void Start()
    {
        gallery = GameObject.Find("Gallery Scroll View").GetComponent<Gallery>();

        //dont call base.Start()
        // assign back button
        backButton.onClick.AddListener(backButtonClicked);
        saveButtons = GameObject.FindGameObjectsWithTag("Save Button");
        // find usernames in oauth folder
        usernames = new List<string>();

        // create save directories if does not exist
        createSaveDirectories();
        // exclude meta files when looking for oauth files
        FileInfo[] oauthFiles = new DirectoryInfo(User.OAUTH_SAVE_PATH).GetFiles().Where(f => !f.Name.EndsWith(".meta")).ToArray();

        //find usernames
        foreach (FileInfo file in oauthFiles)
        {
            string username = file.Name;
            usernames.Add(username.Substring(username.IndexOf("-")+1));
        }
        // map event listeners to the save buttons
        for (int i = 0; i < saveButtons.Length; i++)
        {
            Button saveButton = saveButtons[i].GetComponent<Button>();
            string username = i < usernames.Count ? usernames[i] : null; 
            if(username == null){
                // this button should default to next panel, create a new user
                saveButton.onClick.AddListener(continueButtonClicked);
            }else{
                // this button holds existing user, load save when pressed
                saveButton.GetComponentInChildren<Text>().text = username;
                saveButton.onClick.AddListener(delegate {StartCoroutine(loadSave(username));});
            }
        }
        gameObject.SetActive(false);

    }

    void createSaveDirectories(){
        if(!Directory.Exists(User.PHOTOS_SAVE_PATH)){
            Directory.CreateDirectory(User.PHOTOS_SAVE_PATH);
            Directory.CreateDirectory(User.OAUTH_SAVE_PATH);
        }
    }

    IEnumerator loadSave(string username){
        user.Login(username);
        while(user.loggedIn == false){
            // wait until user has logged in 
            yield return new WaitForSeconds(.1f);
        }
        if(user.oauthRefreshRequired || !user.libraryPhotos.hasSave){
            // two cases, user does not has a save, or the oauth expired
            // with expired oauth, would need to reload the image set download links for images will return FORBIDDEN 403 error.
            nextPopup = loadPhotosPopup;
        }else{
            // open the main display right away
            gallery.initPhotos();
            nextPopup = mainDisplay;
        }
        continueButtonClicked();
    }
}