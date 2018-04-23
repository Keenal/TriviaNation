using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TriviaNation;
using UnityEngine.UI;
using System.Linq;

public class AddStudentToGame : MonoBehaviour {


	List<string> nameList;
	private bool [] listOfToggles = new bool[100];
	private IEnumerable<IUser> userList;

	/*
	 * Connect to Database and read user name also store them in a array
	 */
	/*
	*Diaplay all the name on the GUI
	*/
	void OnGUI() {

		new DataBaseOperations ();
		DataBaseOperations.ConnectToDB ();

		IDataBaseTable UT = new UserTable ();

		IUser user = new User ();

		IUserAdministration userName = new UserAdministration (user,UT);

		userList = userName.ListUsers ();

		int i = 0;
		int x = 150;
		int y = 230;
		int length = 100;
		int width = 40;
		int z = 0;

		foreach( var element in userList )
		{	

			listOfToggles [z] = GUI.Toggle(new Rect (x, y, length, width), listOfToggles[z], element.UserName.ToString());
			y = y + 50;
			z++;

		}

	}
		
	/*
	* when teacher click addButton, will send all students that the 
	* teacher selected to DATABASE.
	*/
	public void addButton(){
		int i = 0;
		List<IUser> users = userList;
		foreach (var element in listOfToggles) {

			if (element) {

				users[i];
			
			}

		}

		Debug.Log ("Added");


	}


}
