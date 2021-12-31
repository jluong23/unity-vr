using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// object which will follow the camera position at a given distance from the camera
public class CameraFollower : MonoBehaviour
{
    public Camera mainCamera;
    public float distanceFromCamera;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Transform camTrans= mainCamera.transform;
        Vector3 newPos = camTrans.position + camTrans.forward * distanceFromCamera;
        transform.position = newPos;
    }
}
