using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainDisplay : MonoBehaviour
{
    public User user;
    public InputActionReference openMenuInput;
    public GameObject navigationPanel;
    private SideMenu[] sideMenus;

    private bool showing;
    private bool previousClick;
    // Start is called before the first frame update
    void Start()
    {
        sideMenus = GetComponentsInChildren<SideMenu>();
        previousClick = false;
        Close();
    }

    void Update(){
        if(openMenuInput.action.IsPressed() && openMenuInput.action.IsPressed() != previousClick){
            // onkeydown for menu
            if(showing){
                Close();
            }else{
                Show();
            }
        }
        previousClick = openMenuInput.action.IsPressed(); 
    }


    public void Show(){
        // appear in front of user, 
        showing = true;
        // deselect navigation options
        foreach (var toggle in navigationPanel.GetComponentsInChildren<ColoredToggle>())
        {
            toggle.isOn = false;
        }
        user.appearObject(gameObject, 2f, 0.2f, false);
        
        closeSideMenus();
    }

    public void Close(){
        showing = false;
        // TODO: hide the element by moving it out of the scene
        transform.position = new Vector3(0,200,0);
    }

    void closeSideMenus(){
        foreach (SideMenu sideMenu in sideMenus)
        {
            sideMenu.Close();
        }
    }
}
