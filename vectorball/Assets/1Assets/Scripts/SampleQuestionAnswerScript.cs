using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SampleQuestionAnswerScript : MonoBehaviour {
	public static string CorrectOptionForQuestion(int level){
		//return correct answer(level) from TextAsset
		return "A";
	}
	public static string GetQuestion(int level){
		//Function will read the questions - Done by Hussain
		return "Question 2";
	}
	public static string[] GetOptions(int level){
		//Return the options read from TextAsset
		return new string[]{"Option2A","Option2B","Option2C","Option2D"};
	}
}
