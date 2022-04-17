using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Attach an InputField to a keyboard by assigning currentInputField field.
/// </summary>
public class VRKeyboard: MonoBehaviour
{
    public Button shiftButton;
    public Button enterButton;
    public Button backButton;
    private List<Button> alphaKeys;
    private List<GameObject> allKeys = new List<GameObject>();
    private bool shiftOn = false;
    private bool isOpen = false;
    public InputField currentInputField;

    // Start is called before the first frame update
    void Start()
    {
        // populate alpha and singular keys
        List<GameObject> alphaKeysList = GameObject.FindGameObjectsWithTag("Keyboard Key").ToList();
        alphaKeys = alphaKeysList.Select(i => i.GetComponent<Button>()).ToList();
        shiftButton.onClick.AddListener(shiftButtonClicked);
        enterButton.onClick.AddListener(Close);
        backButton.onClick.AddListener(backButtonClicked);
        // populate all keys
        allKeys.AddRange(alphaKeysList);
        allKeys.Add(shiftButton.gameObject);
        allKeys.Add(enterButton.gameObject);
        allKeys.Add(backButton.gameObject);
        // add listeners
        foreach (Button key in alphaKeys)
        {
            key.onClick.AddListener(delegate { keyClicked(key); } );
        }
        Hide();
    }

    void Update() {
        GameObject selectedObject = EventSystem.current.currentSelectedGameObject; 
        if(currentInputField != null){
            if(currentInputField.isFocused){
                Open();
            }else if(selectedObject == null || (selectedObject != null && !allKeys.Contains(selectedObject)) ){
                // closing condition: user clicks off the keyboard or another ui element which is not a key
                Close();
            }
        }

    }


    void Open()
    {
        // spawn in front of user
        if (!isOpen)
        {
            isOpen = true;
            gameObject.SetActive(true);
            transform.position = (currentInputField.transform.position + currentInputField.transform.forward * -1f) - new Vector3(0, 1f, 0);
        }
    }

    void Close()
    {
        if (isOpen)
        {
            isOpen = false;
            currentInputField = null;
            Hide();
        }
    }

    void Hide(){
        gameObject.transform.position = new Vector3(0,200,0);
    }

    void keyClicked(Button key)
    {
        if(currentInputField != null)
        {
            currentInputField.text += key.GetComponentInChildren<Text>().text;
            if (shiftOn)
            {
                shiftButtonClicked();   
            }
        }
    }

    void shiftButtonClicked()
    {
        shiftOn = !shiftOn;
        foreach (Button key in alphaKeys)
        {
            Text textComponent = key.GetComponentInChildren<Text>();
            textComponent.text = shiftOn ? textComponent.text.ToUpper() : textComponent.text.ToLower();
        }

    }

    void backButtonClicked()
    {
        if (currentInputField != null)
        {
            currentInputField.text = currentInputField.text.Remove(currentInputField.text.Length - 1, 1);
        }
    }
}
