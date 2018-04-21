using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TriviaNation;
using UnityEngine.UI;
using System.Linq;


public class AddTime : MonoBehaviour {

	public GameObject hour;
	public GameObject minu;
	public GameObject sec;


	private string Hour;
	private string Minu;
	private string Sec;


	public bool checkField()
	{
		if ((Hour != "") && (Minu != "") && (Sec != "")) {
			return true;
		}

		return false;
	}
	public void SubmitButton()
	{
		if (checkField ()) {
		
			Debug.Log ("Set Time");
		}
	}
}
