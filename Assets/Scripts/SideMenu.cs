using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMenu : MonoBehaviour
{
    protected Gallery gallery;
    protected MainDisplay mainDisplay;
    public GameObject navigationMenu; 
    // Start is called before the first frame update
    protected virtual void Start()
    {
        mainDisplay = GameObject.Find("Main Display").GetComponent<MainDisplay>();   
        gallery = GameObject.Find("Gallery Scroll View").GetComponent<Gallery>();
        Close();
    }

    public void Show(){
        this.transform.position = navigationMenu.transform.position + 1.5f*navigationMenu.transform.right;
    }
    public void Close(){
        // TODO: hide the element by moving it out of the scene
        transform.position = new Vector3(0,200,0);
    }
}
