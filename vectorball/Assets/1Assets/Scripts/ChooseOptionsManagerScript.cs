using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChooseOptionsManagerScript : MonoBehaviour {


	//Toggle Game Objects
	public Toggle isOptionA;
	public Toggle isOptionB;
	public Toggle isOptionC;
	public Toggle isOptionD;

	//Check which option is active
	public char ActiveOption(){
		//Set the chosen option as A or B or C or D below
		char option = ' ';
		if (isOptionA.isOn)
			option = 'A';
		else if (isOptionB.isOn)
			option = 'B';
		else if (isOptionC.isOn)
			option = 'C';
		else if (isOptionD.isOn)
			option = 'D';
		return option;
	}

	public void OnSubmit(){
		//Check the selected option with correct option
		Debug.Log("User chose "+ ActiveOption());
		//If ActiveOption() == CorrectOptionForQuestion() call IfCorrectOption()
		//Maintain TextAsset with q and a. 
		//Script to make TextAsset values into array. CorrectOptionForQuestion(index) return ans(index)
		//Can use same for generating question and display on canvas
	}

	public void IfCorrectOption(){
		//Move ball and load next question
	}

	public void IfIncorrectOption(){
		//Hint Popup. Maintain hint also in TextAsset
	}
		
}
