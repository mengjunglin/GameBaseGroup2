using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChooseOptionsManagerScript : MonoBehaviour {


	//Toggle Game Objects
	public Toggle isOptionA;
	public Toggle isOptionB;
	public Toggle isOptionC;
	public Toggle isOptionD;

	public GameObject ball;

	//Question textbox
	public Text questionText;

	private static int level = 0;


	// num 

	//Check which option is active
	public string ActiveOption(){
		//Set the chosen option as A or B or C or D below
		string option = " ";
		if (isOptionA.isOn)
			option = isOptionA.GetComponentInChildren<Text>().text;
		else if (isOptionB.isOn)
			option = isOptionB.GetComponentInChildren<Text>().text;
		else if (isOptionC.isOn)
			option = isOptionC.GetComponentInChildren<Text>().text;
		else if (isOptionD.isOn)
			option = isOptionD.GetComponentInChildren<Text>().text;
		return option;
	}

	public void OnSubmit(){
		//Check the selected option with correct option
		string option = ActiveOption ();
		Debug.Log("User chose "+ option);
		//TODO - Move ball
		string[] coordinates = option.Split(',');
		int x = int.Parse(coordinates[0]);
		int y = int.Parse(coordinates[1]);
		MoveBall (x,y);
		//If ActiveOption() == CorrectOptionForQuestion() call IfCorrectOption()
		if (option.Equals(SampleQuestionAnswerScript.CorrectOptionForQuestion (level)))
			IfCorrectOption (option);
		else
			IfIncorrectOption (option);
		//Maintain TextAsset with q and a. 
		//Script to make TextAsset values into array. CorrectOptionForQuestion(index) return ans(index)
		//Can use same for generating question and display on canvas
	}

	public void IfCorrectOption(string option){
		//function to display vector notation
		Debug.Log(option);
		VectorRepresentationScript script = GameObject.FindGameObjectWithTag("OptionsManager").GetComponent<VectorRepresentationScript>();
		script.convertResultToVector(true,level,option);
		//Increase level
		++level;
		LoadNextQuestion ();
	}

	public void IfIncorrectOption(string option){
		//Hint Popup. Maintain hint also in TextAsset
		VectorRepresentationScript script = GameObject.FindGameObjectWithTag("OptionsManager").GetComponent<VectorRepresentationScript>();
		script.convertResultToVector(false,level,option);
	}

	public void MoveBall(int x, int y){
		ball.transform.position = FieldController.GetAbsolutePosition(x,y);
	}

	public void LoadNextQuestion(){
		//Load next question
		string question = SampleQuestionAnswerScript.GetQuestion(level);

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
