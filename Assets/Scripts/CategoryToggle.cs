using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// a single category toggle button
public class CategoryToggle : MonoBehaviour
{
    private ColoredToggle toggle;
    private Gallery gallery; 
    private string category;
    private Text textElement;

    private void Awake()
    {

        category = "";
        textElement = GetComponentInChildren<Text>();
        gallery = GameObject.Find("Gallery Scroll View").GetComponent<Gallery>();
        toggle = GetComponent<ColoredToggle>();
        toggle.onValueChanged.AddListener(onToggleValueChanged);
    }

    private void onToggleValueChanged(bool isOn){
        gallery.updateGallery();
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
}
