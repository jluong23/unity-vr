using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPopup : MonoBehaviour
{

    private static Stack<GameObject> panelHistory = new Stack<GameObject>();
    public Button continueButton;
    public GameObject nextPopup;
    public Button backButton;

    protected virtual void Start()
    {
        if(continueButton != null){
            continueButton.onClick.AddListener(continueButtonClicked);
        }
        if(backButton != null){
            backButton.onClick.AddListener(backButtonClicked);
        }
        Close();
    }

    protected virtual void continueButtonClicked(){
        panelHistory.Push(gameObject);
        if(nextPopup != null){
            // replace next popup with this popup
            nextPopup.transform.position = gameObject.transform.position; 
        }
        Close();
    }
    protected virtual void backButtonClicked(){
        GameObject previousPopup = panelHistory.Pop();
        if(previousPopup != null){
            // replace previous popup with this popup
            previousPopup.transform.position = gameObject.transform.position; 
        }
        Close();
    }

    public void Close() {
        // gameObject.SetActive(false);
        // TODO: hide the element by moving it out of the scene
        transform.position = new Vector3(0, 200, 0);
    }
}
