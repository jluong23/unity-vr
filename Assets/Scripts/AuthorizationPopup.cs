using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuthorizationPopup : MenuPopup
{
    public Text progressText;
    public LoadSavePopup loadSavePopup;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        // transform of popup is positioned by MenuPopup
        continueButton.interactable = false;
    }

    /// <summary>
    /// Authorization complete, allow the user to progress from the authorization popup (button clickable)
    /// </summary>
    /// <param name="email"></param>
    public void allowProgress(string email){
        progressText.text = string.Format("Status: Completed\nEmail: {0}", email);
        continueButton.interactable = true;
        //update savebuttons to show the new user just authorised
        loadSavePopup.updateSaveButtons();
    }
}
