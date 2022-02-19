using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableButtonOnClick : MonoBehaviour
{
    public Button button;

    private void Start() {
        button.onClick.AddListener(disableButton);
    }
    public void disableButton(){
        button.interactable = false;
    }
}
