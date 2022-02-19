using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MonthSelector : MonoBehaviour
{
    Button button;
    Text buttonText;
    private List<string> months = new List<string>{"Jan", "Feb", "Mar", "Apr", "May", "Jun", "July", "Aug", "Sept", "Oct", "Nov", "Dec"};
    public int monthIndex;
    // Start is called before the first frame update

    void Start()
    {
        monthIndex = 1;
        buttonText = GetComponentInChildren<Text>();
        button = GetComponent<Button>();
        button.onClick.AddListener(changeMonth);
    }

    private void changeMonth(){
        int nextMonthIndex = (months.IndexOf(buttonText.text)+1) % 12;
        buttonText.text = months[nextMonthIndex];
        monthIndex = nextMonthIndex+1; // based 1 index
    }

    /// <summary>
    /// 1-based (from 1-12 inclusive)
    /// </summary>
    /// <param name="monthIndex"></param>
    public void changeMonth(int monthIndex){
        buttonText.text = months[monthIndex-1];
        this.monthIndex = monthIndex;
    }

}
