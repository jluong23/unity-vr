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
    private Dropdown endDateYearPanel;

    /// <summary> DateTime of oldest image in the data set </summary>
    public DateTime maxStartDate; 
    /// <summary> DateTime of latest image in the data set </summary>
    public DateTime maxEndDate;

    /// <summary> DateTime of start date currently selected in Date Filter UI </summary>
    public DateTime currentStartDate;

    /// <summary> DateTime of end date currently selected in Date Filter UI </summary>
    public DateTime currentEndDate;

    void Start()
    {
        datePanel = transform.Find("Date Panel");
        startDatePanel = datePanel.Find("Start Date Panel");
        endDatePanel = datePanel.Find("End Date Panel");

        startDateYearPanel = startDatePanel.GetComponentInChildren<Dropdown>();
        endDateYearPanel = endDatePanel.GetComponentInChildren<Dropdown>();
    }

    /// <summary>
    /// Given the set of photos, set the dates to the newest and oldest dates
    /// for the date UI selector.
    /// </summary>
    /// <param name="userPhotos"></param>
    public void setMaxDateRanges(UserPhotos userPhotos){
        Tuple<DateTime, DateTime> dateRange = userPhotos.getDateRange();
        maxStartDate = dateRange.Item1;
        maxEndDate = dateRange.Item2;

        // set year dropdown menu for both start and end date panels 
        // clear options first
        startDateYearPanel.options.Clear(); 
        endDateYearPanel.options.Clear(); 
        for (int i=0; i <= maxEndDate.Year - maxStartDate.Year; i++)
        {
            // add the year options for the drop down menu
            // between start and end years 
            string newYearOption = (maxStartDate.Year+i).ToString();
            startDateYearPanel.options.Add(new Dropdown.OptionData(newYearOption));
            endDateYearPanel.options.Add(new Dropdown.OptionData(newYearOption));
        }

        // change start date buttons
        startDatePanel.GetComponentInChildren<MonthSelector>().changeMonth(maxStartDate.Month);
        startDateYearPanel.value = 0;
        startDateYearPanel.RefreshShownValue();

        // change end date buttons
        endDatePanel.GetComponentInChildren<MonthSelector>().changeMonth(maxEndDate.Month);
        endDateYearPanel.value = maxEndDate.Year - maxStartDate.Year;
        endDateYearPanel.RefreshShownValue();

        // update currently selected dates
        currentStartDate = maxStartDate;
        currentEndDate = maxEndDate;

    }

    public void updateSelectedDates(){
        // TODO:
        ///
        /// on value change, update variables of currently selected dates.
        ///
        // currentEndDate = new DateTime();

    }

}
