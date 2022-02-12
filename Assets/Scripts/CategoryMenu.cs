using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class CategoryMenu : MonoBehaviour
{
    private Text[] buttonTextElements;
    private Toggle[] buttonToggleComponents;
    void Start()
    {
        buttonTextElements = transform.Find("Category Panel").GetComponentsInChildren<Text>();
        buttonToggleComponents = transform.Find("Category Panel").GetComponentsInChildren<Toggle>();
        updateCategoryButtonsText();
    }

    // makes all category toggles interactable or non interactable, opposite from current state
    public void toggleCategoryButtons()
    {
        foreach (var toggle in buttonToggleComponents)
        {
            toggle.interactable = !toggle.interactable;
        }
    }

    // update text on category buttons
    void updateCategoryButtonsText(){
        for (int i = 0; i < buttonTextElements.Count(); i++)
        {   
            string category = ContentFilter.ALL_CATEGORIES[i];

            // from full caps category, capitalise first letter, lowercase the rest
            buttonTextElements[i].text = category[0] + category.Substring(1).ToLower();
        }
    }
    // add category counts to each category button
    public void appendCategoryCounts(Dictionary<string, int> categoryCounts){

        foreach (var buttonText in buttonTextElements)
        {
            string category = buttonText.text.ToUpper();
            if(categoryCounts[category] > 0){
                buttonText.text += string.Format(" <b>({0})</b>", categoryCounts[category]);
            }
            else{
                buttonText.text += " <b>(0)</b>";
            }
        }
    }

}
