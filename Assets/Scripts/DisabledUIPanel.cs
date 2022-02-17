using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// disable all selectable ui elements in the panel until a button is clicked
public class DisabledUIPanel : MonoBehaviour
{
    public Button activateButton;
    Selectable[] uiElements;
    void Start()
    {
        uiElements = GetComponentsInChildren<Selectable>();
        foreach (var uiElement in uiElements)
        {
            uiElement.interactable = false;
        }       

        activateButton.onClick.AddListener(activateUIElements);
    }

    void activateUIElements(){
        foreach (var uiElement in uiElements)
        {
            uiElement.interactable = true;
        }   
    }
}
