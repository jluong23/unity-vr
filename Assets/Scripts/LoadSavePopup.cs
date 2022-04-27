using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class LoadSavePopup : MenuPopup
{
    private GameObject[] saveButtons;
    public MenuPopup loadPhotosPopup;
    public MenuPopup enterUsernamePopup;
    public GameObject mainDisplay;
    private Gallery gallery;
    public User user;
    private List<string> usernames;
    protected override void Start()
    {
        //dont call base.Start()
        gallery = GameObject.Find("Gallery Scroll View").GetComponent<Gallery>();

        // assign back button
        backButton.onClick.AddListener(backButtonClicked);
        saveButtons = GameObject.FindGameObjectsWithTag("Save Button");

        // assign continue button
        continueButton.onClick.AddListener(createNewUsername);

        // create save directories if does not exist
        createSaveDirectories();
        updateSaveButtons();
        Close();
    }

    void createSaveDirectories(){
        if(!Directory.Exists(User.photos_save_path)){
            Directory.CreateDirectory(User.photos_save_path);
            Directory.CreateDirectory(User.oauth_save_path);
        }
    }

    public List<string> getSavedUsernames(){
        // find usernames in oauth folder
        usernames = new List<string>();
        // exclude meta files when looking for oauth files
        FileInfo[] oauthFiles = new DirectoryInfo(User.oauth_save_path).GetFiles().Where(f => !f.Name.EndsWith(".meta")).ToArray();

        //find usernames
        foreach (FileInfo file in oauthFiles)
        {
            string username = file.Name;
            usernames.Add(username.Substring(username.IndexOf("-")+1));
        }
        return usernames;
    }

    public void updateSaveButtons(){
        List<string> usernames = getSavedUsernames();
        // map event listeners to the save buttons
        for (int i = 0; i < saveButtons.Length; i++)
        {
            Button saveButton = saveButtons[i].GetComponent<Button>();
            saveButton.onClick.RemoveAllListeners();

            string username = i < usernames.Count ? usernames[i] : null; 
            if(username != null){
                saveButton.interactable = true;
                // this button holds existing user, load save when pressed
                saveButton.GetComponentInChildren<Text>().text = username;
                saveButton.onClick.AddListener(delegate {StartCoroutine(loadSave(username));});
            }else{
                // no save exists
                saveButton.GetComponentInChildren<Text>().text = string.Format("Save {0}", i+1);
                saveButton.interactable = false;
            }
        }   

        if(usernames.Count == saveButtons.Length){
            // no more save slots, can't click new save
            continueButton.interactable = false;
        }else{
            continueButton.interactable = true;
        }
    }

    void createNewUsername(){
        // button listener for clicking an empty save button which does not contain a user
        nextPopup = enterUsernamePopup.gameObject;
        continueButtonClicked();
    }
    IEnumerator loadSave(string username){
        Debug.Log(username + " being logged in..");
        user.Login(username);

        buttonsInteractable(false); // turn off interaction for all buttons when logging in
        while(user.loggedIn == false){
            // wait until user has logged in 
            yield return new WaitForSeconds(.5f);
        }
        updateSaveButtons(); // user logged in, change buttons back to previous

        if(user.oauthRefreshRequired || !user.libraryPhotos.hasSave){
            // two cases, user does not has a save, or the oauth expired
            // with expired oauth, would need to reload the image set download links for images will return FORBIDDEN 403 error.
            nextPopup = loadPhotosPopup.gameObject;
        }else{
            // open the main display right away
            gallery.initPhotos();
            nextPopup = mainDisplay;
        }
        continueButtonClicked();
    }

    /// <summary>
    /// Sets all buttons on this panel to off and on based on parameter
    /// </summary>
    void buttonsInteractable(bool isInteractable){
        continueButton.interactable = isInteractable;
        backButton.interactable = isInteractable;
        for (int i = 0; i < saveButtons.Length; i++)
        {
            Button saveButton = saveButtons[i].GetComponent<Button>();
            saveButton.interactable = isInteractable;

        }
    }
}
