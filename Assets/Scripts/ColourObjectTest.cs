using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ColourObjectTest : MonoBehaviour
{
    public InputActionReference colorReference = null;
    private MeshRenderer meshRenderer = null;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>(); 
    }

    void Update()
    {
        float value = colorReference.action.ReadValue<float>();
        UpdateColor(value);
    }

    void UpdateColor(float value)
    {
        meshRenderer.material.color = new Color(value, value, value);
    }
}
