using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TranslateObject : MonoBehaviour
{

    public InputActionReference translateXReference = null;
    public InputActionReference translateYReference = null;
    public InputActionReference selectObjectReference = null;

    public float speed = 0.2f;
    private bool holding = false;

    // Update is called once per frame
    void Update()
    {
        if(selectObjectReference.action.triggered){
            holding = !holding;
        }

        if(holding){
            Vector3 dy = speed * transform.up * translateYReference.action.ReadValue<float>();
            Vector3 dx = speed * transform.right * translateXReference.action.ReadValue<float>();
            transform.Translate(dx+dy);
        }
    }
}
