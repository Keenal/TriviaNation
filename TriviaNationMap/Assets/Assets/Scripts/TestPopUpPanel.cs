using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TriviaNation;

public class TestPopUpPanel : MonoBehaviour
{
    private PopUpPanel popUpPanel;
    private DisplayManager displayManager;

    //Creates the actions
    private UnityAction yesAction;
    private UnityAction noAction;
    private UnityAction cancelAction;

    private QuestionTable questionTable = new QuestionTable();

    private void Awake()
    {
        new DataBaseOperations();
        DataBaseOperations.ConnectToDB();
        popUpPanel = PopUpPanel.Instance();
        displayManager = DisplayManager.Instance();

        //Assigns a method to the function
        yesAction = new UnityAction(TestYes);
        noAction = new UnityAction(TestNo);
        cancelAction = new UnityAction(TestCancel);
    }

    public void TestActions()
    {
        popUpPanel.Choice("Pick a Button\n", yesAction, noAction, cancelAction);
    }

    //Send to the Popup Panel to set up the Buttons and Functions to call
    //These are wrapped into UnityActions
    public void TestYes()
    {
        displayManager.DisplayMessage(questionTable.RetrieveTableRow(1).ToString());
    }

    public void TestNo()
    {
        displayManager.DisplayMessage(questionTable.RetrieveTableRow(2).ToString());
    }

    public void TestCancel()
    {
        displayManager.DisplayMessage("Canceling...");
    }

}
