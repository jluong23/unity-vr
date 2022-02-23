using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// a single category toggle button
public class CategoryToggle : MonoBehaviour
{
    private Toggle toggle;
    private Gallery gallery; 
    private Image toggleBackground;
    private string category;
    private Text textElement;

    private void Awake()
    {

        category = "";
        textElement = GetComponentInChildren<Text>();
        gallery = GameObject.Find("Gallery Scroll View").GetComponent<Gallery>();
        toggle = GetComponent<Toggle>();
        toggle.isOn = false;
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

    public void updateCategoryCount(int count){
        textElement.text = string.Format(" {0} <b>({1})</b>", category, count);
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
        // update the gallery with the newly selected categories
        gallery.updateGallery();

    }
}
