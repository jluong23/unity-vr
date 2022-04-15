using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GalleryScroller : ContentScroller
{
    protected override void Start()
    {
        content = contentLayout.gameObject;
        rowUpButton.onClick.AddListener(delegate{scrollButtonClicked(ScrollDirection.UP_ROW);});
        rowDownButton.onClick.AddListener(delegate{scrollButtonClicked(ScrollDirection.DOWN_ROW);});
        toTopButton.onClick.AddListener(delegate{moveMonth(false);});
        toBottomButton.onClick.AddListener(delegate{moveMonth(true);});
    }   

    /// <summary>
    /// to top and bottom buttons are remapped to moving up and down months
    /// </summary>
    /// <param name="moveDown"></param>
    void moveMonth(bool moveDown){
        float newYPos = 0;
        List<float> headerYPositions = content.GetComponentsInChildren<Text>()
            .Select(i => -i.GetComponent<Transform>().localPosition.y - i.GetComponent<RectTransform>().rect.height)
            .ToList();
        try
        {
            if(moveDown){
                //find the next y pos for the content panel, which is the next header after the currently visible one (computed by comparing y values)
                newYPos = headerYPositions.Where(x => x > content.transform.localPosition.y).Min(); 
            }else{
                //find the previous y pos for the content panel
                newYPos = headerYPositions.Where(x => x < content.transform.localPosition.y).Max(); 
            }
            content.transform.localPosition = new Vector3(0,newYPos,0);
        }
        catch (System.InvalidOperationException)
        {
            // max or min will throw an exception when the streams from .Where() are empty
            // this means the user is trying to leave the extremities of the scroll. return with no change to content position.
            return;
        }
    }
}
