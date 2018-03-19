using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class PopUpPanel : MonoBehaviour
{
    public Text question;
    public Image iconImage;
    public Button yesButton;
    public Button noButton;
    public Button cancelButton;
    public GameObject popUpPanelObject;

    private static PopUpPanel popUpPanel;

    public static PopUpPanel Instance ()
    {
        if(!popUpPanel)
        {
            popUpPanel = FindObjectOfType(typeof(PopUpPanel)) as PopUpPanel;

            if (!popUpPanel)
                Debug.LogError("there needs to be one active Popup Panel script on a gameObject in your scene");
        }

        return popUpPanel;
    }

    //Yes - No - Cancel
    //Parameters:  A string, a Yes event, a No event, and Cancel event
    public void Choice(string question, UnityAction yesEvent, UnityAction noEvent, UnityAction cancelEvent)
    {   
        //Activates the panel
        popUpPanelObject.SetActive(true);

        //Removes all listeners from the button so that when you click it later, it does not
        //call an older function.
        yesButton.onClick.RemoveAllListeners();

        //Add your own listener.
        yesButton.onClick.AddListener(yesEvent);

        //Close the Panel
        yesButton.onClick.AddListener(ClosePanel);

        noButton.onClick.RemoveAllListeners();
        noButton.onClick.AddListener(noEvent);
        noButton.onClick.AddListener(ClosePanel);

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(cancelEvent);
        cancelButton.onClick.AddListener(ClosePanel);

        this.question.text = question;

        this.iconImage.gameObject.SetActive(false);
        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(true);
    }

    //closes the panel
    public void ClosePanel()
    {
        popUpPanelObject.SetActive(false);
    }
}
