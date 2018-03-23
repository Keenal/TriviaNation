using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using TriviaNation;
using I18N;
using I18N.West;
using System.Data;


public class SignUp : MonoBehaviour {

	public GameObject username;
	public GameObject email;
	public GameObject password;
	public GameObject confirmPassword;

	private string Username;
	private string Email;
	private string Password;
	private string ConfirmPassword;

	private string form;

	private bool isEmailValid = false;


	private string[] characters = {"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
								   "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z",
								   "1","2","3","4","5","6","7","8","9","0","-","_"};


	
	public void SignUpButton()
	{
		//check if the username is valid
		bool UN = false;

		//check if rhe email is valid
		bool EM = false;

		//check if the password is valid 
		bool PW = false;

		//check if confirm password is equal to password
		bool CP = false;


		//create User Information Table 
		new DataBaseOperations();
		DataBaseOperations.ConnectToDB ();

		IDataBaseTable UT = new UserTable ();



		UT.CreateTable(UT.TableName, UT.TableCreationString);



		Debug.Log("Table exist" + UT.TableExists (UT.TableName));


		//check if two users can not use the same name
		if(Username != ""){
			//
			//connect to database 
			//
			UN = true;
			
		}else{
			Debug.LogWarning("Username file Empty");
			}

		//check if the email is valid 
		if (Email != "") {
				EmailValidation ();
				if (isEmailValid) {
					if (Email.Contains ("@")) {
						if (Email.Contains (".")) {
							EM = true;
						} else {
							Debug.LogWarning("Email is Incorrect");
						}
					} else {
						Debug.LogWarning("Email is Incorrect");
					}
				} else {
					Debug.LogWarning("Email is Incorrect");
				}
			}else {
				Debug.LogWarning("Email Filed is Empty");
		}
			


		//check if the password is longer than 6 digits
		if(Password != "") {
			if (Password.Length > 5) {
				PW = true;
			} else {
				Debug.LogWarning("Passwork need to be mat least 6 characters long");
			}
		}else{
			Debug.LogWarning("Password File is Empty");
		}

		//check if confirm password and password are the same
		if (ConfirmPassword != "") {
			if (ConfirmPassword == Password) {
				CP = true;
			} else {
				Debug.LogWarning("Password do not match");
			}
		} else {
			Debug.LogWarning("Confirm Password File is Empty");
		}


		if (UN == true && EM == true && PW == true && CP == true) {

			bool Clear = true;
			int i = 1;

			foreach (char c in Password) {
				i++;

				if (Clear) {
					
					Password = "";
					Clear = false;

					char Ecrypted = (char)(c * i);
					Password += Ecrypted.ToString();
				
				}


			}

			/*
			//create User Information Table 
			new DataBaseOperations();
			DataBaseOperations.ConnectToDB ();
			IDataBaseTable UT = new UserTable ();
			UT.CreateTable(UT.TableName, UT.TableCreationString);

			IUser user = new User ();

			IDataEntry info1 = new User(Username,Email, Password,"0");
			//IDataEntry info2 = new User (Email);
			//IDataEntry info3 = new User (Password);


			UT.InsertRowIntoTable ("UserTable", info1);
			//UT.InsertRowIntoTable ("UserTable", info2);
			//UT.InsertRowIntoTable ("UserTable", info3);

			*/

			/*
			//create User Information Table 
			new DataBaseOperations();
			DataBaseOperations.ConnectToDB ();

			IDataBaseTable UT = new UserTable ();



			UT.CreateTable(UT.TableName, UT.TableCreationString);

			*/

			Debug.Log("Table exist" + UT.TableExists (UT.TableName));

			IUser user = new User ();

			IUserAdministration newUser = new UserAdministration (user,UT);


			/*
			user.UserName = Username;
			IDataEntry user_username = (IDataEntry)user;

			user.Email = Email;
			IDataEntry user_email = (IDataEntry)user;

			user.Password = Password;
			IDataEntry user_password = (IDataEntry)user;

			user.Score = null;
			IDataEntry user_score = null;
	*/
			newUser.AddUser (Username, Email, Password, Password,null);

			/*
			UT.InsertRowIntoTable ("UserTable", user_username);
			UT.InsertRowIntoTable ("UserTable", user_email);
			UT.InsertRowIntoTable ("UserTable", user_password);
			UT.InsertRowIntoTable ("UserTable", user_score);
	*/




			username.GetComponent<InputField>().text = "";
			email.GetComponent<InputField>().text = "";
			password.GetComponent<InputField>().text = "";
			confirmPassword.GetComponent<InputField>().text = "";

			print ("Sign Up Successfully");
			Console.WriteLine("Sign Up Successfully");
	
		}
	}


	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Tab)) {
			
			if (username.GetComponent<InputField> ().isFocused) {
				email.GetComponent<InputField> ().Select();
			}

			if (email.GetComponent<InputField> ().isFocused) {
				password.GetComponent<InputField> ().Select();
			}

			if (password.GetComponent<InputField> ().isFocused) {
				confirmPassword.GetComponent<InputField> ().Select();
			}
				

		
		}

		if (Input.GetKeyDown (KeyCode.Return)) {

			if (Password != "" && Email != "" && Password != "" && ConfirmPassword != "") {
				SignUpButton ();
			}
		
		}

		Username = username.GetComponent<InputField>().text;
		Email = email.GetComponent<InputField>().text;
		Password = password.GetComponent<InputField>().text;
		ConfirmPassword = confirmPassword.GetComponent<InputField>().text;


	}

	//loop through character[] to check if the email is valid or not 
	void EmailValidation()
	{
		//start character
		bool SW = false;

		//end character
		bool EW = false;

		//check if the start character
		for (int i = 0; i < characters.Length; i++) {
			if (Email.StartsWith (characters [i])) {
				SW = true;
			}
		}

		//check end character
		for (int i = 0; i < characters.Length; i++) {
			if (Email.StartsWith (characters [i])) {
				EW = true;
			}
		}

		if (SW == true && EW == true) {
			
			isEmailValid = true;
		} else {
			isEmailValid = false;
		}
	}
}

