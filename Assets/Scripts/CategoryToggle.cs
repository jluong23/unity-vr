using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryToggle : MonoBehaviour
{
     private Toggle toggle;
     private Image toggleBackground;
     private void Start()
     {
         toggle = GetComponent<Toggle>();
         toggle.isOn = false;
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
