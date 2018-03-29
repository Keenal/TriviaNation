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

namespace TriviaNation {
	
public class SignUp : MonoBehaviour {

	public GameObject username;
	public GameObject email;
	public GameObject password;
	public GameObject confirmPassword;

	private string Username;
	private string Email;
	private string Password;
	private string ConfirmPassword;

	public void SignUpButton()
	{
		
	
		//create User Information Table 
		new DataBaseOperations();
		DataBaseOperations.ConnectToDB ();

		IDataBaseTable UT = new UserTable ();



		UT.CreateTable(UT.TableName, UT.TableCreationString);



		Debug.Log("Table exist" + UT.TableExists (UT.TableName));


	

		IUser user = new User ();

		IUserAdministration newUser = new UserAdministration (user,UT);


		newUser.AddUser (Username, Email, Password, Password,"");

		

		username.GetComponent<InputField>().text = "";
		email.GetComponent<InputField>().text = "";
		password.GetComponent<InputField>().text = "";
		confirmPassword.GetComponent<InputField>().text = "";

		Debug.Log("Sign Up Successfully");


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
	}
		
}
