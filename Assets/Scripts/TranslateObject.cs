using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TranslateObject : MonoBehaviour
{

    public InputActionReference translateReference = null;
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
	    Vector3 pointerPos = translateReference.action.ReadValue<Vector2>();
            transform.position = pointerPos;
        }
    }
}
