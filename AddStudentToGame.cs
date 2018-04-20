using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TriviaNation;

public class AddStudentToGame : MonoBehaviour {

	public GameObject names;

	private string Name;
	/*
	*read from Database and display all the student name on the screen
	*/
	void OnGUI() {

		Name = "SAMMMMM";

		GUI.Label(new Rect(200, 200, 300, 200), Name);

		Name = names.tag;

	}


	public void addButton(){
		
	}


	// Update is called once per frame
	//void Update () {}
}
