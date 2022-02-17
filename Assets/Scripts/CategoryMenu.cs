using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class CategoryMenu : MonoBehaviour
{
    // different category buttons as children
    private CategoryToggle[] categoryToggles;
    void Start()
    {
        categoryToggles = transform.Find("Category Panel").GetComponentsInChildren<CategoryToggle>();
        updateCategoryButtonsText();
    }

    // update text on category buttons
    void updateCategoryButtonsText(){
        for (int i = 0; i < categoryToggles.Count(); i++)
        {   
            string category = ContentFilter.ALL_CATEGORIES[i];
            categoryToggles[i].setCategory(category);
        }
    }
    // add category counts to each category button
    public void appendCategoryCounts(Dictionary<string, int> categoryCounts){

        foreach (var categoryToggle in categoryToggles)
        {
            string category = categoryToggle.getCategory();
            // count is 0 if the category does not exist
            int count = categoryCounts.ContainsKey(category) ? categoryCounts[category] : 0;
            categoryToggle.appendCategoryCount(count);
        }
    }

    // return a list of categories which are selected on the menu
    public List<string> getSelectedCategories(){
        return categoryToggles.Where(i => i.GetComponent<Toggle>().isOn).Select(i => i.getCategory()).ToList();
    }

}
