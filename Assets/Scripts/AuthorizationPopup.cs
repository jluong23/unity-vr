using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuthorizationPopup : MenuPopup
{
    private Button startButton;
    public Text progressText;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        // transform of popup is positioned by MenuPopup
        startButton = GetComponentInChildren<Button>();
        startButton.interactable = false;
    }

    /// <summary>
    /// Authorization complete, allow the user to progress from the authorization popup (button clickable)
    /// </summary>
    /// <param name="email"></param>
    public void allowProgress(string email){
        progressText.text = string.Format("Status: Completed\nEmail: {0}", email);
        startButton.interactable = true;
    }
}
