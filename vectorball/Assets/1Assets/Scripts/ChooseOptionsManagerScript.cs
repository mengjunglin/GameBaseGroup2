using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChooseOptionsManagerScript : MonoBehaviour {


	//Toggle Game Objects
	public Toggle isOptionA;
	public Toggle isOptionB;
	public Toggle isOptionC;
	public Toggle isOptionD;

	//Question textbox
	public Text questionText;

	private static int level = 1;

	//Check which option is active
	public string ActiveOption(){
		//Set the chosen option as A or B or C or D below
		string option = " ";
		if (isOptionA.isOn)
			option = "A";
		else if (isOptionB.isOn)
			option = "B";
		else if (isOptionC.isOn)
			option = "C";
		else if (isOptionD.isOn)
			option = "D";
		return option;
	}

	public void OnSubmit(){
		//Check the selected option with correct option
		Debug.Log("User chose "+ ActiveOption());
		//If ActiveOption() == CorrectOptionForQuestion() call IfCorrectOption()
		if (ActiveOption ().Equals(SampleQuestionAnswerScript.CorrectOptionForQuestion (level)))
			IfCorrectOption ();
		else
			IfIncorrectOption ();
		//Maintain TextAsset with q and a. 
		//Script to make TextAsset values into array. CorrectOptionForQuestion(index) return ans(index)
		//Can use same for generating question and display on canvas
	}

	public void IfCorrectOption(){
		//Increase level
		++level;

		//TODO - Move ball
	
		LoadNextQuestion ();



	}

	public void IfIncorrectOption(){
		//Hint Popup. Maintain hint also in TextAsset
	}

	public void LoadNextQuestion(){
		//Load next question
		string question = SampleQuestionAnswerScript.GetQuestion (level);

		//Set the question in the text box
		questionText.text = question;

		//Laod the options for the question
		string[] options = SampleQuestionAnswerScript.GetOptions (level);

		//Set the options in the toggles
		isOptionA.GetComponentInChildren<Text>().text = options[0];
		isOptionB.GetComponentInChildren<Text>().text = options[1];
		isOptionC.GetComponentInChildren<Text>().text = options[2];
		isOptionD.GetComponentInChildren<Text>().text = options[3];

	}
		
}
