using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;
using TriviaNation;


//using TriviaNation.DataBaseOperation;
/**
TriviaNation is a networked trivia game designed for use in 
classrooms. Class members are each in control of a nation on 
a map. The goal of the game is to increase the size of the nation 
by winning trivia challenges and defeating other class members 
in contested territories. The focus is on gamifying learning and 
making it an enjoyable experience.
@author Timothy McWatters
@author Keenal Shah
@author Randy Quimby
@author Wesley Easton
@author Wenwen Xu
@version 1.0
CEN3032    "TriviaNation" SEII- Group 1's class project
*/


public class Login : MonoBehaviour {

	public GameObject email;
	public GameObject password;


	private string Email;
	private string Password;

	public void EnterButton()
	{
		//create User Information Table 
		new DataBaseOperations();
		DataBaseOperations.ConnectToDB ();

		IDataBaseTable UT = new UserTable ();
		IUser user = new User ();


		IUserAuthentication user1 = new UserAuthentication (UT, user);

		bool t = user1.AuthenticateUser (Email, Password);

		///If t is returning ture, connect to main GUI
		if (t) {
			Debug.Log ("Login Successfully");
			Debug.Log ("True");
		} else {
			Debug.Log("Login Failed");
		}

		email.GetComponent<InputField> ().text = "";
		password.GetComponent<InputField> ().text = "";

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Tab)) {
			if (email.GetComponent<InputField> ().isFocused) {
				password.GetComponent<InputField> ().Select ();
			}
		}

		if (Input.GetKeyDown (KeyCode.Return)) {
			if (Email != "" && Password != "") {
				EnterButton ();
			}
		}

		Email = email.GetComponent<InputField> ().text;
		Password = password.GetComponent<InputField> ().text;
		
	}

}
