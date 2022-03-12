using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColoredToggle : Toggle
{    
    private Image toggleBackground;

    protected override void Awake()
    {
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
