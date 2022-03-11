using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// object which will be in front of the camera at a given distance
public class Popup : MonoBehaviour
{
    public Camera mainCamera;
    public float distanceFromCamera;


    // Start is called before the first frame update
    void Start()
    {
        Transform camTrans = mainCamera.transform;
        Vector3 newPos = camTrans.position + camTrans.forward * distanceFromCamera;
        transform.position = newPos;
        transform.rotation = camTrans.rotation;
    }
}
