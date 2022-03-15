using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// object which will be in front of another object at a given distance
public class Popup : MonoBehaviour
{
    public GameObject coveredObject;
    public float distance;


    // Start is called before the first frame update
    void Start()
    {
        Transform coveredObjectTrans = coveredObject.transform;
        Vector3 newPos = coveredObjectTrans.position + coveredObjectTrans.forward * distance;
        transform.position = newPos;
        transform.rotation = coveredObjectTrans.rotation;
    }

/// <summary>
/// Spawns object in front of covered object with same orientation.
/// </summary>
/// <param name="createNewObject">To instantiate new object or translate an existing one</param>
/// <returns></returns>
    public GameObject Show(bool createNewObject){
        GameObject obj;
        Vector3 spawnPoint = transform.position + coveredObject.transform.forward * -distance;
        if(createNewObject){
            obj = Instantiate(gameObject, spawnPoint, Quaternion.identity);
        }else{
            // this game object already exists, translate instead
            transform.position = spawnPoint;
            obj = gameObject;
        }
        // orient obj, same as coveredObject rotation
        obj.transform.rotation = coveredObject.transform.rotation;
        return obj;
    }
}
