using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPopup : MonoBehaviour
{

    public Button continueButton;
    public GameObject nextPopup;

    protected virtual void Start()
    {
        Close();
        if(continueButton != null){
            continueButton.onClick.AddListener(continueButtonClicked);
        }
    }

    protected virtual void continueButtonClicked(){
        if(nextPopup != null){
            // replace next popup with this popup
            nextPopup.transform.position = gameObject.transform.position; 
        }
        Close();
    }

    /// <summary>
    /// Set and show the next popup, hiding this popup.
    /// </summary>
    /// <param name="nextPopup"></param>
    protected virtual void continueButtonClicked(GameObject nextPopup){
        this.nextPopup = nextPopup;
        continueButtonClicked();
    }


    public void Close() {
        // gameObject.SetActive(false);
        // TODO: hide the element by moving it out of the scene
        transform.position = new Vector3(0, 200, 0);
    }
}
