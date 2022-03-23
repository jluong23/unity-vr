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
        gallery = GameObject.Find("Gallery Scroll View").GetComponent<Gallery>();
        setToggles();
    }

    void setToggles(){
        /// add toggle with category names, excluding the category counts
        Clear();
        foreach (var category in ContentFilter.ALL_CATEGORIES)
        {
            GameObject newCategoryObj = Instantiate(categoryTogglePrefab, content.transform);
            // will also update button text element to the category
            newCategoryObj.GetComponentInChildren<CategoryToggle>().setCategory(category);
        }
    }

    public void setToggles(Dictionary<string, int> categoryCounts){
        /// add toggle with category names and counts, ordering by highest category count
        Clean();
        List<string> orderedCategories = new List<string>(ContentFilter.ALL_CATEGORIES);
        // filter out categories with count = 0
        orderedCategories = new List<string>(orderedCategories.Where(
            i => categoryCounts[i] > 0 
        ));
        // sort categories by order
        orderedCategories.Sort( 
            (a,b) => categoryCounts[a].CompareTo(categoryCounts[b])
        );
        // reverse order, categories with lowest counts are pushed to bottom
        // of category selection menu and vice versa
        orderedCategories.Reverse();

        // start placing category toggles
        foreach (var category in orderedCategories)
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
            string toggleCategory = toggle.GetComponent<CategoryToggle>().getCategory();
            if(!selectedCategories.Contains(toggleCategory)){
                Destroy(toggle.gameObject);
            }
        }
    }

}
