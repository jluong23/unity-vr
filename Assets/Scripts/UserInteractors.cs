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
    private ContinuousMoveProviderBase moveProviderBase;


    void Start()
    {
        // stop the user from moving
        moveProviderBase = GetComponent<ContinuousMoveProviderBase>();
    }

    // Update is called once per frame
    void Update()
    {
        var leftHandMoveInput = leftHandMove.action.ReadValue<Vector2>();
        float x = leftHandMoveInput.x;
        float y = leftHandMoveInput.y;

        if(leftHandInteractor.hasSelection){
            moveProviderBase.moveSpeed = 0;

            Transform interactableTransform = leftHandInteractor.interactablesSelected[0].transform;
            interactableTransform.localScale += new Vector3(x,y) / 1000;
        }
        else
        {
            moveProviderBase.moveSpeed = 1;

        }
    }
}
