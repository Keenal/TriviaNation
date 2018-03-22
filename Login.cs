using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine;
using System.IO;



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

	public GameObject username;
	public GameObject password;


	private string Username;
	private string Password;
	private string[] lines;
	private string DecryptedPassword;


	public void EnterButton()
	{
		//check is the username is valid
		bool UN = false;

		//check if the password is valid 
		bool PW = false;

		//check if the username exist in the Database
		if (Username != "") {
			if (Username.Length < 20) {
				UN = true;
			} else {
				Debug.LogWarning ("Username length need be be least than 20");
				}

		} else {
			Debug.LogWarning ("Username File Empty");
			}


		//check if the password match the username
		if (Password != "") {
			
			if (System.IO.File.Exists (Username + ".txt")) {
				
				int i = 1;

				foreach (char c in lines[2]) {
					i++;
					char Decrypted = (char)(c / i);
					DecryptedPassword += Decrypted.ToString ();
				}
			
				if (Password == DecryptedPassword) {
					PW = true;
				} 

			} else {
				Debug.LogWarning ("Password does not match the Username ");

				} 

			if (UN == true && PW == true) {
				print ("Login successful");
				username.GetComponent<InputField> ().text = "";
				password.GetComponent<InputField> ().text = "";
			}
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Tab)) {
			if (username.GetComponent<InputField> ().isFocused) {
				password.GetComponent<InputField> ().Select ();
			}
		}

		if (Input.GetKeyDown (KeyCode.Return)) {
			if (Username != "" && Password != "") {
				EnterButton ();
			}
		}

		Username = username.GetComponent<InputField> ().text;
		Password = password.GetComponent<InputField> ().text;
		
	}

}
