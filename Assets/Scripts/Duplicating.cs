using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duplicating : MonoBehaviour
{
    public GameObject rootObj;


    public void Duplicate(){
        GameObject duplicate = Instantiate(rootObj);
    }
}
