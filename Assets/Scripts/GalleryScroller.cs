using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryScroller : MonoBehaviour
{
    enum ScrollDirection {TOP, UP_ROW, DOWN_ROW, BOTTOM};
    public GridLayoutGroup contentLayout;
    public Button toTopButton;
    public Button rowUpButton;
    public Button rowDownButton;
    public Button toBottomButton;
    private GameObject content;
    private int numCols;
    // Start is called before the first frame update
    void Start()
    {
        content = contentLayout.gameObject;
        toTopButton.onClick.AddListener(delegate{scrollButtonClicked(ScrollDirection.TOP);});
        rowUpButton.onClick.AddListener(delegate{scrollButtonClicked(ScrollDirection.UP_ROW);});
        rowDownButton.onClick.AddListener(delegate{scrollButtonClicked(ScrollDirection.DOWN_ROW);});
        toBottomButton.onClick.AddListener(delegate{scrollButtonClicked(ScrollDirection.BOTTOM);});
        numCols = contentLayout.constraintCount;
    }

    public int getGridRows(){
        return (int) content.transform.childCount / numCols;        
    }

    void scrollButtonClicked(ScrollDirection scrollDirection){
        float rowHeight = contentLayout.cellSize.y + 1.667f*contentLayout.padding.top;
        Vector3 scrollDownRow = new Vector3(0,rowHeight,0);
        Vector3 topPosition = Vector3.zero;
        Vector3 bottomPosition = topPosition + (scrollDownRow*(getGridRows()-2));

        switch (scrollDirection)
        {
            case ScrollDirection.TOP: 
                content.transform.localPosition = topPosition;
                break;
            case ScrollDirection.UP_ROW: 
                if(content.transform.localPosition != topPosition){
                    content.transform.localPosition -= scrollDownRow;
                }
                break;
            case ScrollDirection.DOWN_ROW: 
                if(content.transform.localPosition != bottomPosition){
                    content.transform.localPosition += scrollDownRow;
                }
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
