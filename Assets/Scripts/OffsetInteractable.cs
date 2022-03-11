using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;


public class OffsetInteractable : XRGrabInteractable
{
    // class of interactables which is grabbed on the spot (not pulled to hand).
    // performed by moving attachTransform to hand when grabbing and back when unselected
    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

    }
    private void FixedUpdate()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        attachTransform.position = args.interactorObject.GetAttachTransform(this).position;
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);

        attachTransform.localPosition = transform.position;
    }

}
