using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DateMenu : MonoBehaviour
{
    private Transform datePanel;
    private Tuple<Transform, Transform> dateOptions;
    private Tuple<Dropdown, Dropdown> yearPanels;
    private Tuple<DateTime, DateTime> currentDateRange;
    private Gallery gallery;
    void Start()
    {
        gallery = GameObject.Find("Gallery Scroll View").GetComponent<Gallery>();
        datePanel = transform.Find("Date Panel");
        dateOptions = new Tuple<Transform, Transform>(datePanel.Find("Start Date Panel"), datePanel.Find("End Date Panel")); 
        yearPanels = new Tuple<Dropdown, Dropdown>(dateOptions.Item1.GetComponentInChildren<Dropdown>(), dateOptions.Item2.GetComponentInChildren<Dropdown>());
    }

    public Tuple<DateTime, DateTime> getCurrentDateRange(){
        return currentDateRange;
    }

    /// <summary>
    /// Given the set of photos, set the dates to the newest and oldest dates
    /// for the date UI selector.
    /// </summary>
    /// <param name="userPhotos"></param>
    public void setMaxDateRanges(UserPhotos userPhotos){
        Tuple<DateTime, DateTime> maxDateRange = userPhotos.getDateRange();
        DateTime maxStartDate = maxDateRange.Item1;
        DateTime maxEndDate = maxDateRange.Item2;

        // set year dropdown menu for both start and end date panels 
        foreach (var yearPanel in new List<Dropdown>{yearPanels.Item1, yearPanels.Item2})
        {
            // clear options first
            yearPanel.options.Clear();
            for (int i=0; i <= maxEndDate.Year - maxStartDate.Year; i++)
            {
                // add the year options for the drop down menu
                // between start and end years 
                string newYearOption = (maxStartDate.Year+i).ToString();
                yearPanel.options.Add(new Dropdown.OptionData(newYearOption));
            }
            if(yearPanel == yearPanels.Item1){
                // start year
                dateOptions.Item1.GetComponentInChildren<MonthSelector>().changeMonth(maxStartDate.Month);
                yearPanel.value = 0;
            }else{
                // end year
                dateOptions.Item2.GetComponentInChildren<MonthSelector>().changeMonth(maxEndDate.Month);
                yearPanel.value = maxEndDate.Year - maxStartDate.Year;

            }
            // update button on year panel
            yearPanel.RefreshShownValue();
            // update current date range to max
            currentDateRange = maxDateRange;
        }
    }

    public void updateSelectedDates(){
        /// <summary>
        ///  When submit button is clicked for date filter, update attributes of currently selected dates
        ///  and update gallery to show images within start and end dates
        /// </summary>
        /// <returns></returns>
        
        DateTime currentStartDate = new DateTime(); 
        DateTime currentEndDate = new DateTime();
        
        foreach (var yearPanel in new List<Dropdown>{yearPanels.Item1, yearPanels.Item2})
        {
            int year, month, day;
            year = int.Parse(yearPanel.options[yearPanel.value].text); 
            if(yearPanel == yearPanels.Item1){
                // start date
                month = dateOptions.Item1.GetComponentInChildren<MonthSelector>().monthIndex;
                day = 1;
                currentStartDate = new DateTime(year, month, day);
            }else{
                // end date
                month = dateOptions.Item2.GetComponentInChildren<MonthSelector>().monthIndex;
                day = DateTime.DaysInMonth(year, month);
                currentEndDate = new DateTime(year, month, day);
            }
        }
        currentDateRange = new Tuple<DateTime, DateTime>(currentStartDate, currentEndDate);
        // update gallery shown
        gallery.updateGallery();
    }

}
