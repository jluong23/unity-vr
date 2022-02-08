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
        // // print categories on available buttons
        buttonTextElements = transform.Find("Category Panel").GetComponentsInChildren<Text>();
        for (int i = 0; i < buttonTextElements.Count(); i++)
        {   
            buttonTextElements[i].text = ContentFilter.ALL_CATEGORIES[i];
        }
    }

    // add category counts to each category button
    public void appendCategoryCounts(Dictionary<string, int> categoryCounts){
        for (int i = 0; i < buttonTextElements.Count(); i++)
        {   
            Text currentButton = buttonTextElements[i];
            string category = currentButton.text;
            currentButton.text += string.Format(" ({0})", categoryCounts[category]);
        }
    }

}
