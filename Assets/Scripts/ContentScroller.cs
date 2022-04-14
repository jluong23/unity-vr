using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ContentScroller : MonoBehaviour
{
    protected enum ScrollDirection {TOP, UP_ROW, DOWN_ROW, BOTTOM};
    public LayoutGroup contentLayout;
    public Button toTopButton;
    public Button rowUpButton;
    public Button rowDownButton;
    public Button toBottomButton;
    protected GameObject content;
    protected virtual void Start()
    {
        content = contentLayout.gameObject;
        toTopButton.onClick.AddListener(delegate{scrollButtonClicked(ScrollDirection.TOP);});
        rowUpButton.onClick.AddListener(delegate{scrollButtonClicked(ScrollDirection.UP_ROW);});
        rowDownButton.onClick.AddListener(delegate{scrollButtonClicked(ScrollDirection.DOWN_ROW);});
        toBottomButton.onClick.AddListener(delegate{scrollButtonClicked(ScrollDirection.BOTTOM);});
    }

    protected float findBottomY(){
        return content.transform.GetComponentsInChildren<Transform>().
            Select(delegate (Transform t) { return t.localPosition.y; })
            .ToList<float>().Min(); //the smallest y value out of content elements
    }

    protected void scrollButtonClicked(ScrollDirection scrollDirection){
        float childHeight = content.transform.GetChild(0).GetComponent<RectTransform>().rect.height;
        float rowHeight = childHeight + 1.667f*contentLayout.padding.top;
        Vector3 scrollDownRow = new Vector3(0,rowHeight,0);
        Vector3 topPosition = Vector3.zero;
        Vector3 bottomPosition = new Vector3(0, -findBottomY() ,0); //

        switch (scrollDirection)
        {
            case ScrollDirection.TOP: 
                content.transform.localPosition = topPosition;
                break;
            case ScrollDirection.UP_ROW: 
                content.transform.localPosition -= scrollDownRow;
                break;
            case ScrollDirection.DOWN_ROW: 
                content.transform.localPosition += scrollDownRow;
                break;
            case ScrollDirection.BOTTOM: 
                content.transform.localPosition = bottomPosition; 
                break;
        }        
    }

    public void scrollToTop(){
        scrollButtonClicked(ScrollDirection.TOP);
    }


    
}
