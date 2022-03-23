using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Gameobject needs to have a ColoredToggle component and a text component in children.
/// Stores which side menu to open once clicked
/// </summary>
public class NavigationOption : MonoBehaviour
{
    private Text textComponent;
    private ColoredToggle toggleComponent;
    public SideMenu sideMenu; 

    void Awake()
    {
        toggleComponent = gameObject.GetComponent<ColoredToggle>();
        textComponent = gameObject.GetComponentInChildren<Text>();
        toggleComponent.onValueChanged.AddListener(onToggleValueChanged);
    }

    private void onToggleValueChanged(bool isOn){
        // show the side menu which is attached to this option
        if(isOn){
            sideMenu.Show();
        }else{
            sideMenu.Close();
        }
    }

}
