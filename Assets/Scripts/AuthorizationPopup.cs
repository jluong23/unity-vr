using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuthorizationPopup : MonoBehaviour
{
    private Button startButton;
    public GameObject nextPopup; 
    public Text progressText;

    // Start is called before the first frame update
    void Start()
    {
        // transform of popup is positioned by MenuPopup
        startButton = GetComponentInChildren<Button>();
        startButton.interactable = false;
        startButton.onClick.AddListener(buttonClicked);

        //TODO: hide nextPopup (which should be after clicking logging in)
        nextPopup.transform.position = new Vector3(0,200,0);
    }

    void buttonClicked(){
        // replace next popup with this popup
        nextPopup.transform.position = gameObject.transform.position;
        gameObject.SetActive(false);
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
