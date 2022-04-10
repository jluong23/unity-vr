using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class CategoryMenu : SideMenu
{
    // different category buttons as children
    private CategoryToggle[] categoryToggles;
    public GameObject categoryTogglePrefab;
    public GameObject content;

    void Awake()
    {
        setToggles();
    }

    /// <summary>
        /// add toggles with category names, excluding the category counts.
        /// Used temporarily before user logs in, showing all categories.
    /// </summary>
    void setToggles(){
        // used
        Clear();
        foreach (var category in ContentFilter.ALL_CATEGORIES)
        {
            GameObject newCategoryObj = Instantiate(categoryTogglePrefab, content.transform);
            // will also update button text element to the category
            newCategoryObj.GetComponentInChildren<CategoryToggle>().setCategory(category);
        }
    }

    /// <summary>
    /// add toggles with category names and counts, ordering by highest category count.
    /// Used when a different category selection has been made.
    /// </summary>
    /// <param name="categoryCounts"></param>
    public void setToggles(Dictionary<string, int> categoryCounts){
        Clean();
        List<string> selectedCategories = getSelectedCategories(); 
        // initialise new categories, starting with all categories
        List<string> newCategories = new List<string>(ContentFilter.ALL_CATEGORIES);
        // filter out categories with count = 0 or categories which are currently selected
        newCategories = new List<string>(newCategories.Where(
            i => categoryCounts[i] > 0 &&
            !selectedCategories.Contains(i)
        ));
        // sort categories by order
        newCategories.Sort( 
            (a,b) => categoryCounts[a].CompareTo(categoryCounts[b])
        );
        // reverse order, categories with lowest counts are pushed to bottom
        // of category selection menu and vice versa
        newCategories.Reverse();

        // start placing category toggles
        foreach (var category in newCategories)
        {
            GameObject newCategoryObj = Instantiate(categoryTogglePrefab, content.transform);
            newCategoryObj.GetComponentInChildren<CategoryToggle>().setCategory(category);
            // append category count
            newCategoryObj.GetComponent<CategoryToggle>().appendCategoryCount(categoryCounts[category]);
        }
    }

    // return a list of categories which are selected on the menu
    public List<string> getSelectedCategories(){
        categoryToggles = transform.Find("Category Scroll Panel").GetComponentsInChildren<CategoryToggle>();
        return categoryToggles.Where(i => i.GetComponent<Toggle>().isOn).Select(i => i.getCategory()).ToList();
    }

    void Clear(){
        foreach (Transform child in content.transform) {
            Destroy(child.gameObject);
        }
    }

    void Clean(){
        /// Clear all category toggles apart from the selected ones
        List<string> selectedCategories = getSelectedCategories(); 
        foreach (Transform toggle in content.transform) {
            // iterate over dropdown list options, remove options which aren't selected
            string toggleCategory = toggle.GetComponent<CategoryToggle>().getCategory();
            if(!selectedCategories.Contains(toggleCategory)){
                Destroy(toggle.gameObject);
            }
        }
    }

}
