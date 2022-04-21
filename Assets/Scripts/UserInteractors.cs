using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using System;

public class UserInteractors : MonoBehaviour
{
    // Start is called before the first frame update
    public XRRayInteractor leftHandInteractor;
    public XRRayInteractor rightHandInteractor;
    public InputActionReference leftHandMove;
    public ContinuousTurnProviderBase turnProviderBase;
    public ContinuousMoveProviderBase moveProviderBase;

    public float frameScaleSpeed = 80;


    void FixedUpdate()
    {
        
        var leftHandMoveInput = leftHandMove.action.ReadValue<Vector2>();
        if(leftHandInteractor.hasSelection){
            moveProviderBase.moveSpeed = 0;
            turnProviderBase.turnSpeed = 0;
            Transform interactableTransform = leftHandInteractor.interactablesSelected[0].transform;
            ImageFrame imageFrameComponent = interactableTransform.GetComponent<ImageFrame>();
            interactableTransform.localScale += new Vector3(leftHandMoveInput.x, leftHandMoveInput.x) * frameScaleSpeed / 1000;
        }
        else
        {
            //reset movement when user has let go of object
            moveProviderBase.moveSpeed = 1;
            turnProviderBase.turnSpeed = 60;

        }
    }
}
