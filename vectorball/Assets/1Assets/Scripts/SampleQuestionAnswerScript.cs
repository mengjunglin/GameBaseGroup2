using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SampleQuestionAnswerScript : MonoBehaviour {
	
	public static string GetQuestion(int level){
		//Function will read the questions - Done by Hussain
		return "Question " + level.ToString();
	}
	public static string[] GetOptions(int level){
		//Return the options read from TextAsset
		return new string[]{"1,2","4,2","3,3","2,1"};
	}
	public static string GetAnswer(int level){
		//Function will read the questions - Done by Hussain
		return "(1,3)";
	}
}
