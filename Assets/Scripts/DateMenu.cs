using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DateMenu : MonoBehaviour
{
    private Transform datePanel;
    private Transform startDatePanel;
    private Transform endDatePanel;

    private Dropdown startDateYearPanel;
    private MonthSelector startDateMonthButton;
    private Dropdown endDateYearPanel;
    private MonthSelector endDateMonthButton;

    void Start()
    {
        datePanel = transform.Find("Date Panel");
        startDatePanel = datePanel.Find("Start Date Panel");
        endDatePanel = datePanel.Find("End Date Panel");

        startDateYearPanel = startDatePanel.GetComponentInChildren<Dropdown>();
        startDateMonthButton = startDatePanel.GetComponentInChildren<MonthSelector>();

        endDateYearPanel = endDatePanel.GetComponentInChildren<Dropdown>();
        endDateMonthButton = endDatePanel.GetComponentInChildren<MonthSelector>();

    }

    /// <summary>
    /// Given the set of photos, set the dates to the newest and oldest dates
    /// for the date UI selector.
    /// </summary>
    /// <param name="userPhotos"></param>
    public void setDateRanges(UserPhotos userPhotos){
        Tuple<DateTime, DateTime> dateRange = userPhotos.getDateRange();
        DateTime startDate = dateRange.Item1;
        DateTime endDate = dateRange.Item2;

        // set year dropdown menu for both start and end date panels 
        // clear options first
        startDateYearPanel.options.Clear(); 
        endDateYearPanel.options.Clear(); 
        for (int i=0; i <= endDate.Year - startDate.Year; i++)
        {
            // add the year options
            string newYearOption = (startDate.Year+i).ToString();
            startDateYearPanel.options.Add(new Dropdown.OptionData(newYearOption));
            endDateYearPanel.options.Add(new Dropdown.OptionData(newYearOption));
        }

        // change start date buttons
        startDateMonthButton.changeMonth(startDate.Month);
        startDateYearPanel.value = 0;
        startDateYearPanel.RefreshShownValue();

        // change end date buttons
        endDatePanel.GetComponentInChildren<MonthSelector>().changeMonth(endDate.Month);
        endDateYearPanel.value = endDate.Year - startDate.Year;
        endDateYearPanel.RefreshShownValue();


    }


}
