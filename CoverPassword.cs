using System.Collections;
using UnityEngine;
using System;

public class CoverPassword : MonoBehaviour {

	string passwordToEdit = string.Empty;

	void OnGUI() {
		passwordToEdit = GUI.PasswordField(new Rect(10, 10, 200, 20), passwordToEdit, "*"[0], 25);
	}




	void Update()
	{
		if (passwordToEdit == "password" && Input.GetKey ("enter")) {
			Debug.Log ("log-in");
		}
	}

}
