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

/// <summary>
/// Spawns object a in front of b with same orientation
/// </summary>
/// <param name="a"></param>
/// <param name="b"></param>
/// <param name="createNewObject">To instantiate new object or translate an existing one</param>
/// <returns></returns>
    public static GameObject Show(GameObject a, GameObject b, bool createNewObject){
        GameObject aObj;
        Vector3 spawnPoint = b.transform.position + b.transform.forward * -2f;
        if(createNewObject){
            aObj = Instantiate(a, spawnPoint, Quaternion.identity);
        }else{
            // a already exists, translate in front of b
            a.transform.position = spawnPoint;
            aObj = a; //set aObj to a for return value
        }
        // orient aObj, same as b rotation
        aObj.transform.rotation = b.transform.rotation;
        return aObj;
    }
}
