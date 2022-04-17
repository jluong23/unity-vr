using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class UserInteractors : MonoBehaviour
{
    // Start is called before the first frame update
    public XRRayInteractor leftHandInteractor;
    public XRRayInteractor rightHandInteractor;
    public InputActionReference leftHandMove;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(leftHandMove.action.ReadValue<Vector2>());
        if(leftHandInteractor.firstInteractableSelected != null){
            
        }
    }
}
