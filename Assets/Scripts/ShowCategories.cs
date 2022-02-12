using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class ShowCategories : MonoBehaviour
{
    private Text[] buttonTextElements;
    void Start()
    {
        buttonTextElements = transform.Find("Category Panel").GetComponentsInChildren<Text>();
        updateCategoryButtonsText();
    }

    // update text on category buttons
    void updateCategoryButtonsText(){
        for (int i = 0; i < buttonTextElements.Count(); i++)
        {   
            string category = ContentFilter.ALL_CATEGORIES[i];

            buttonTextElements[i].text = category[0] + category.Substring(1).ToLower();
        }
    }
    // add category counts to each category button
    public void appendCategoryCounts(Dictionary<string, int> categoryCounts){
        foreach (var buttonText in buttonTextElements)
        {
            string category = buttonText.text;
            buttonText.text += string.Format(" ({0})", categoryCounts[category.ToUpper()]);
        }
    }

}
