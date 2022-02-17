using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MonthSelector : MonoBehaviour
{
    Button button;
    Text buttonText;
    private List<string> months = new List<string>{"Jan", "Feb", "Mar", "Apr", "May", "Jun", "July", "Aug", "Sept", "Oct", "Nov", "Dec"};

    // Start is called before the first frame update
    void Start()
    {
        buttonText = GetComponentInChildren<Text>();
        button = GetComponent<Button>();
        button.onClick.AddListener(changeMonth);
    }

    void changeMonth(){
        int nextMonthIndex = (months.IndexOf(buttonText.text)+1) % 12;
        buttonText.text = months[nextMonthIndex];
    }

}
