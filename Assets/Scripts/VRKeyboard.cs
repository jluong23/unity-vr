using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRKeyboard: MonoBehaviour
{
    public Button shiftButton;
    public Button enterButton;
    public Button backButton;
    private List<Button> keys;
    private bool shiftOn;
    public bool isOpen;
    private InputField currentInputField;

    // Start is called before the first frame update
    void Start()
    {
        keys = GameObject.FindGameObjectsWithTag("Keyboard Key").ToList().Select(i => i.GetComponent<Button>()).ToList();
        shiftButton.onClick.AddListener(shiftButtonClicked);
        enterButton.onClick.AddListener(Close);
        backButton.onClick.AddListener(backButtonClicked);
        isOpen = false;
        shiftOn = false;
        foreach (Button key in keys)
        {
            key.onClick.AddListener(delegate { keyClicked(key); } );
        }
        gameObject.SetActive(false);
    }


    public void Open(InputField inputField)
    {
        // spawn in front of user
        if (!isOpen)
        {
            isOpen = true;
            gameObject.SetActive(true);
            currentInputField = inputField;
            transform.position = (currentInputField.transform.position + currentInputField.transform.forward * -1f) - new Vector3(0, 1f, 0);
        }
    }

    public void Close()
    {
        if (isOpen)
        {
            isOpen = false;
            currentInputField = null;
            gameObject.SetActive(false);
        }
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
        foreach (Button key in keys)
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
