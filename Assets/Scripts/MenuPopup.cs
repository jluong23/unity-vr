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
        gameObject.SetActive(false);

        if(continueButton != null){
            continueButton.onClick.AddListener(continueButtonClicked);
        }
    }

    protected virtual void continueButtonClicked(){
        if(nextPopup != null){
            // replace next popup with this popup
            nextPopup.transform.position = gameObject.transform.position; 
            nextPopup.SetActive(true);
        }
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Set and show the next popup, hiding this popup.
    /// </summary>
    /// <param name="nextPopup"></param>
    protected virtual void continueButtonClicked(GameObject nextPopup){
        this.nextPopup = nextPopup;
        continueButtonClicked();
    }
}
