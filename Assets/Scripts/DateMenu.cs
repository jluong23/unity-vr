using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DateMenu : MonoBehaviour
{
    // different category buttons as children
    private Transform startDatePanel;
    private Transform endDateDatePanel;
    

    void Start()
    {
        startDatePanel = transform.Find("Start Date Panel");
        endDateDatePanel = transform.Find("End Date Panel");
    }

}
