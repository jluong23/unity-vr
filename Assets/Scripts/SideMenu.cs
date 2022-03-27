using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMenu : MonoBehaviour
{
    protected Gallery gallery;
    public GameObject navigationMenu; 
    // Start is called before the first frame update
    void Start()
    {
        gallery = GameObject.Find("Gallery Scroll View").GetComponent<Gallery>();
        Close();
    }

    public void Show(){
        this.transform.position = navigationMenu.transform.position + 4*navigationMenu.transform.right;
    }
    public void Close(){
        // TODO: hide the element by moving it out of the scene
        transform.position = new Vector3(0,200,0);
    }
}
