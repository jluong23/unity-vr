using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPopup : MonoBehaviour
{
    private Button button;
    public GameObject selectAlbumsPopup; 
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(buttonClicked);
    }

    void buttonClicked(){
        selectAlbumsPopup.transform.position = gameObject.transform.position;
    }
}
