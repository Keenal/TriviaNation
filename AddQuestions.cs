using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;
using TriviaNation;

public class AddQuestions : MonoBehaviour {

	public GameObject question_type;
	public GameObject question_value;

	private string Question_type;
	private int Question_value;


	//True or False Questions
	public GameObject T_F;
	public GameObject choice_true;
	public GameObject choice_false;
	public GameObject answer;

	private string True_False;
	private string Choice_True;
	private string Choice_False;
	private string T_F_Answer;



	//Multip choice Question
	public GameObject multiple_coice;
	public GameObject choice_a;
	public GameObject choice_b;
	public GameObject choice_c;
	public GameObject choice_d;
	public GameObject multiple_answer;

	private string Multiple_coice;
	private string choice_A;
	private string choice_B;
	private string choice_C;
	private string choice_D;
	private string multiple_Answer;


	private bool isT_F_Button_Selected = false;
	private bool isMultiple_Button_Selected = false;

	public void T_F_Button()
	{
		Question_type = "True or False";
		isT_F_Button_Selected = true;
	}

	public void multiple_Button()
	{
		Question_type = "Multiple Choice";
		isMultiple_Button_Selected = true;

	}

	public bool checkT_F_Field()
	{
		if (isT_F_Button_Selected) {

			if ((True_False != "") && (Choice_True != "") && (Choice_False != "") && (T_F_Answer != "")) {

				return true;
			}

		} else {
			Debug.Log ("ERROR! Field empty");
		}

		return false;
	}

	public bool checkMultiple_Field()
	{
		if (isMultiple_Button_Selected) {
		
			if(( Multiple_coice != "") &&(choice_A != "")&&(choice_B != "")&&(choice_C != "")&&(choice_D != "") && (multiple_Answer != ""))
			{
				return true;
			}

		} else {
			Debug.Log ("ERROR! Field empty");
		}

		return false;
	}

	//add question to database and also create question pack name
	public void addButton()
	{




		new DataBaseOperations ();
		DataBaseOperations.ConnectToDB ();

		/*
			 * Connect to database
			*/
		new DataBaseOperations ();
		DataBaseOperations.ConnectToDB ();


		/*
			 * Create Question Pack Table
			 * 
			*/
		IDataBaseTable QP_Table = new QuestionPackTable ();
		QP_Table.CreateTable (QP_Table.TableName, QP_Table.TableCreationString);



		/*
			 * Add new Question pack to Question pack table
			*/
		ITriviaAdministration newQuestionPack = new TriviaAdministration ();
		newQuestionPack.AddQuestionPack(Question_type,1);

		/*
			 * Create Question table
			*/

		IDataBaseTable QT = new QuestionTable (Question_type);
		QT.CreateTable (QT.TableName, QT.TableCreationString);


		if (checkT_F_Field()) {

			String QText = Choice_True +"~"+ Choice_False+ "~"+T_F_Answer;
			IQuestion newQuestion = new Questions (True_False, QText, Question_type, Question_value,Question_type);

		
		} else if (checkMultiple_Field()) {

			String MText = Multiple_coice + "~" + choice_A + "~" + "~" + choice_B + "~" + choice_C + "~" + choice_D + "~" + multiple_Answer;
			IQuestion newQ1 = new Questions(Multiple_coice, MText, Question_type, Question_value, Question_type );
		}


		question_type.GetComponent<InputField>().text = "";
		question_value.GetComponent<InputField>().text = "";
		T_F.GetComponent<InputField>().text = "";
		choice_true.GetComponent<InputField>().text = "";
		choice_false.GetComponent<InputField>().text = "";
		answer.GetComponent<InputField>().text = "";

		Multiple_coice = multiple_coice.GetComponent<InputField> ().text = "";
		choice_A = choice_a.GetComponent<InputField> ().text = "";
		choice_B = choice_b.GetComponent<InputField> ().text = "";
		choice_C = choice_c.GetComponent<InputField> ().text = "";
		choice_D = choice_d.GetComponent<InputField> ().text = "";
		multiple_Answer = multiple_answer.GetComponent<InputField> ().text = "";



	}
		
	// Update is called once per frame
	void Update () {

		/*
		 * True or False Panel
		*/
		if (Input.GetKeyDown (KeyCode.Tab)) {

			if (T_F.GetComponent<InputField> ().isFocused) {
				choice_true.GetComponent<InputField> ().Select();
			}

			if (choice_true.GetComponent<InputField> ().isFocused) {
				choice_false.GetComponent < InputField> ().Select ();
			}

			if (choice_false.GetComponent<InputField> ().isFocused) {
				answer.GetComponent<InputField> ().Select ();
			}

		}

		Question_type = question_type.GetComponent<InputField> ().text;
		True_False = T_F.GetComponent<InputField> ().text;
		Choice_True = choice_true.GetComponent<InputField> ().text;
		Choice_False = choice_false.GetComponent<InputField> ().text;
		T_F_Answer = answer.GetComponent<InputField> ().text;


		/*
		* Multiple choice panel
		*/
		if (Input.GetKeyDown (KeyCode.Tab)) {

			if (multiple_coice.GetComponent<InputField> ().isFocused) {
				choice_a.GetComponent<InputField> ().Select();
			}

			if (choice_a.GetComponent<InputField> ().isFocused) {
				choice_b.GetComponent < InputField> ().Select ();
			}

			if (choice_b.GetComponent<InputField> ().isFocused) {
				choice_c.GetComponent < InputField> ().Select ();
			}

			if (choice_c.GetComponent<InputField> ().isFocused) {
				choice_d.GetComponent < InputField> ().Select ();
			}

			if (choice_d.GetComponent<InputField> ().isFocused) {
				multiple_answer.GetComponent<InputField> ().Select ();
			}

		}


		Multiple_coice = multiple_coice.GetComponent<InputField> ().text;
		choice_A = choice_a.GetComponent<InputField> ().text;
		choice_B = choice_b.GetComponent<InputField> ().text;
		choice_C = choice_c.GetComponent<InputField> ().text;
		choice_D = choice_d.GetComponent<InputField> ().text;
		multiple_Answer = multiple_answer.GetComponent<InputField> ().text;
	}
}
