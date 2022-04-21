using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using System;

public class UserInteractions : MonoBehaviour
{
    // Start is called before the first frame update
    public XRRayInteractor leftHandInteractor;
    public XRRayInteractor rightHandInteractor;
    public InputActionReference leftHandMove;
    public LocomotionSystem locomotionSystem;
    private ContinuousTurnProviderBase turnProviderBase;
    private ContinuousMoveProviderBase moveProviderBase;
    private float defaultMoveSpeed;
    private float defaultTurnSpeed;
    public float interactableScaleSpeed = 80;

    void Start()
    {
        turnProviderBase = locomotionSystem.GetComponent<ContinuousTurnProviderBase>();
        moveProviderBase = locomotionSystem.GetComponent<ContinuousMoveProviderBase>();
        defaultMoveSpeed = moveProviderBase.moveSpeed;
        defaultTurnSpeed = turnProviderBase.turnSpeed;
    }
    void FixedUpdate()
    {
        // scale interactables (image frames) via left hand, observing change in x axis 
        var leftHandMoveInput = leftHandMove.action.ReadValue<Vector2>();
        if(leftHandInteractor.hasSelection){
            // user can not move or turn
            moveProviderBase.moveSpeed = 0;
            turnProviderBase.turnSpeed = 0;
            // perform scaling
            Transform interactableTransform = leftHandInteractor.interactablesSelected[0].transform;
            interactableTransform.localScale += new Vector3(leftHandMoveInput.x, leftHandMoveInput.x) * interactableScaleSpeed / 1000;
        }
        else
        {
            //reset movement when user has let go of object
            moveProviderBase.moveSpeed = defaultMoveSpeed;
            turnProviderBase.turnSpeed = defaultTurnSpeed;
        }
    }
}
