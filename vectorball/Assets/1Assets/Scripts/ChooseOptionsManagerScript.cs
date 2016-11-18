﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System.Text.RegularExpressions;

public class ChooseOptionsManagerScript : MonoBehaviour {


	//Toggle Game Objects
	public Toggle isOptionA;
	public Toggle isOptionB;
	public Toggle isOptionC;
	public Toggle isOptionD;

	public GameObject ball;

	//Question textbox
	public Text questionText;

	private Question currentQuestion;

	public static int level = 1;

	GameSceneScript scoreScript;

	public void Start(){
		scoreScript = GetComponent<GameSceneScript> ();
		//LoadNextQuestion ();
	}

	public void Update(){
		//Check if all questions for level complete  
		//scoreScript.LevelComplete();
	}

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

		// remove highlight

		//Check the selected option with correct option
		string option = ActiveOption ();
		char multiplier = option.ToCharArray () [0];
		string positionIndices = Regex.Match (option, "(?<=\\().+?(?=\\))").Value;
		Debug.Log("User chose "+ positionIndices);
		string[] coordinates = positionIndices.Split(',');
		int x = int.Parse(coordinates[0]);
		int y = int.Parse(coordinates[1]);
		if ('(' != multiplier) {
			int m = int.Parse (multiplier.ToString());
			x = x * m;
			y = y * m;
		}

		MoveBall (x,y);

		//If ActiveOption() == CorrectOptionForQuestion() call IfCorrectOption()
		if (option.Equals(currentQuestion.answer))
			IfCorrectOption (option);
		else
			IfIncorrectOption (option);
		//Maintain TextAsset with q and a. 
		//Script to make TextAsset values into array. CorrectOptionForQuestion(index) return ans(index)
		//Can use same for generating question and display on canvas
	}

	public void IfCorrectOption(string option){
		//increase score
		//GameSceneScript scoreScript = GameObject.FindGameObjectWithTag("OptionsManager").GetComponent<GameSceneScript>();
		scoreScript.SetPlayerScore (scoreScript.GetPlayerScore() + 1);

		//function to display vector notation
		Debug.Log(option);
		VectorRepresentationScript script = GetComponent<VectorRepresentationScript>();
		script.convertResultToVector(true,level,option);

		//Increase level
		++level;

		//FieldController.instance.UpdatePlayerGrid (true, 1);
		//LoadNextQuestion ();
	}

	public void IfIncorrectOption(string option){
		//increase opponent's score
		//GameSceneScript scoreScript = GameObject.FindGameObjectWithTag("OptionsManager").GetComponent<GameSceneScript>();
		GameSceneScript scoreScript = GetComponent<GameSceneScript>();
		scoreScript.SetOpponentPlayerScore (scoreScript.GetOpponentPlayerScore () + 1);

		VectorRepresentationScript script = GetComponent<VectorRepresentationScript>();
		script.convertResultToVector(false,level,option);
	}

	public void MoveBall(int x, int y){
		//ball.transform.position = Vector3.MoveTowards(ball.transform.position, FieldController.instance.GetAbsolutePosition(x,y), step);
		BallMoveBehavior e = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallMoveBehavior>();
		e.setTarget (FieldController.instance.GetPlayerAt(x,y).transform);

	}



	public void LoadNextQuestion(){
		SampleQuestionAnswerScript script = GetComponent<SampleQuestionAnswerScript>();


		// store the current player location before loading next question


		//Load next question
		currentQuestion = script.GetQuestion(level,1);
		Debug.Log (currentQuestion.answer);
		Debug.Log (currentQuestion.question);
		Debug.Log (currentQuestion.level);

		//Set the question in the text box
		questionText.text = currentQuestion.question;

		//Laod the current answer for the question
		string[] options = new string[4];
		int correctIndex = Random.Range (0, 3);
		options [correctIndex] = currentQuestion.answer;


		//highlight player

		//Load random opponents
		for(int i=0;i<4;++i)
		{
			if (correctIndex != i) {
				FieldController.instance.UpdatePlayerGrid (true,0);
				PlayerScript ps = FieldController.instance.GetRandomOpponent ();
				string positionIndices = Regex.Match (ps.name, "(?<=\\[).+?(?=\\])").Value;
				while(options.Contains(positionIndices)){
					ps = FieldController.instance.GetRandomOpponent ();
					positionIndices = Regex.Match (ps.name, "(?<=\\[).+?(?=\\])").Value;
				}
				if (0 != ps.getMultiplier()) {
					string[] xy = positionIndices.Split (',');
					int[] positions = { int.Parse (xy [0])/ps.getMultiplier (), int.Parse (xy [1])/ps.getMultiplier () };
					string newPositions = ps.getMultiplier () + "(" + positions [0].ToString () + "," + positions [1].ToString () + ")";
					options [i] = newPositions;
				} else {
					options [i] = "(" + positionIndices + ")";
				}
			}
		}

		//Set the options in the toggles
		isOptionA.GetComponentInChildren<Text>().text = options[0];
		isOptionB.GetComponentInChildren<Text>().text = options[1];
		isOptionC.GetComponentInChildren<Text>().text = options[2];
		isOptionD.GetComponentInChildren<Text>().text = options[3];

	}

	// Function to get current question number for a particular level
	public int GetCurrentQuestionNumber(){
		if (currentQuestion != null) {
			return currentQuestion.subLevel;
		}
		return -1;
	}
		
}
