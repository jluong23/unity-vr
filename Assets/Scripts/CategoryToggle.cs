using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// a single category toggle button
public class CategoryToggle : MonoBehaviour
{
    public Toggle toggle;
    private Image toggleBackground;
    private string category;
    private Text textElement;

    private void Start()
    {
        toggle = GetComponent<Toggle>();
        category = "";
        textElement = GetComponentInChildren<Text>();
        //  toggle.isOn = false;
        toggle.interactable = false;  // disabled until user has loaded their images 
        toggleBackground = GetComponentInChildren<Image>();
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    public string getCategory(){
        return category;
    }

    public void setCategory(string newCategory){
        category = newCategory;
        // update text element of toggle
        textElement.text = category;
    }

    public void appendCategoryCount(int count){
        textElement.text += string.Format(" <b>({0})</b>", count);
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
        // update the gallery 

    }
}
