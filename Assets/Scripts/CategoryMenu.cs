using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class CategoryMenu : MonoBehaviour
{
    // different category buttons as children
    private CategoryToggle[] categoryToggles;
    public GameObject categoryTogglePrefab;
    public GameObject content;
    private Gallery gallery;

    void Awake()
    {
        gallery = GameObject.Find("Gallery Scroll View").GetComponent<Gallery>();
        addToggles();
    }

    void addToggles(){
        /// add toggle with category names, excluding the category counts
        Clear();
        foreach (var category in ContentFilter.ALL_CATEGORIES)
        {
            GameObject newCategoryObj = Instantiate(categoryTogglePrefab, content.transform);
            // will also update button text element to the category
            newCategoryObj.GetComponentInChildren<CategoryToggle>().setCategory(category);
        }
    }

    public void addToggles(Dictionary<string, int> categoryCounts){
        /// add toggle with category names and counts, ordering by highest category count
        Clear();
        List<string> orderedCategories = new List<string>(ContentFilter.ALL_CATEGORIES);
        orderedCategories.Sort( 
            (a,b) => categoryCounts[a].CompareTo(categoryCounts[b])
        );
        orderedCategories.Reverse();

        foreach (var category in orderedCategories)
        {
            GameObject newCategoryObj = Instantiate(categoryTogglePrefab, content.transform);
            newCategoryObj.GetComponentInChildren<CategoryToggle>().setCategory(category);
            // append category count
            int count = categoryCounts.ContainsKey(category) ? categoryCounts[category] : 0;
            newCategoryObj.GetComponent<CategoryToggle>().appendCategoryCount(count);
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

}
