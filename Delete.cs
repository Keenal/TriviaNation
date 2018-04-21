using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;
using TriviaNation;


public class Delete : MonoBehaviour {

	public GameObject name;

	private string Name;

	public bool checkEmptyField()
	{
		if (Name != "") {
		
			return true;
		}

		return false;
	}

	public void DeleteButton()
	{
		new DataBaseOperations ();
		DataBaseOperations.ConnectToDB ();

		if (checkEmptyField ()) {

			ITriviaAdministration T = new TriviaAdministration ();
			T.DeleteQuestionPack (Name);
		}

		Name = name.GetComponent<InputField>().text = "";
	}
}
