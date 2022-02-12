using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// a single category toggle button
public class CategoryToggle : MonoBehaviour
{
    private Toggle toggle;
    private Image toggleBackground;
    private string category;

    private void Start()
     {
         toggle = GetComponent<Toggle>();
         category = GetComponentInChildren<Text>().text;
        //  toggle.isOn = false;
         toggle.interactable = false;  // disabled until user has loaded their images 
         toggleBackground = GetComponentInChildren<Image>();
         toggle.onValueChanged.AddListener(OnToggleValueChanged);
     }
 
    private void OnToggleValueChanged(bool isOn)
     {
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
}
