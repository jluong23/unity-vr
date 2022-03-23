using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used for CategoryToggles in category menu and side panel, changing to green when selected and white when not selected
/// </summary>
public class ColoredToggle : Toggle
{    
    private Image toggleBackground;

    protected override void Awake()
    {
        base.Start();
        onValueChanged.AddListener(OnToggleValueChanged);        
        toggleBackground = GetComponentInChildren<Image>();
        isOn = false;

    }
    private void setColour(bool isOn){
        // set the colour to green when on
        Color cb;
        if (isOn)
        {
            cb = Color.green;
        }
        else
        {
            cb = Color.white;
        }
        toggleBackground.color = cb;
    }
 
    private void OnToggleValueChanged(bool isOn)
    {
        setColour(isOn);

    }
}
