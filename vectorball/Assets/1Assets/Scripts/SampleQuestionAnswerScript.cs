using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Question{
	public int level;
	public int subLevel;
	public string question;
	public string answer;
};

public class SampleQuestionAnswerScript : MonoBehaviour {

	public TextAsset questionsFile;
	int prevLevel = 0;
	int subLevelCounter = 0;


	Question[] questions = new Question[2];

	public void Start(){
		//questions = new Question[2];
		int counter = 0;
		if (null != questionsFile) {
			string[] questionLines = (questionsFile.text.Split('\n'));
			foreach (string questionLine in questionLines) {
				string[] questionElements = questionLine.Split ('@');
				questions [counter] = new Question ();
				Debug.Log ("start" + questionElements [0] + "end");
				questions [counter].level = int.Parse(questionElements [0].Trim());
				if (prevLevel < questions [counter].level) {
					prevLevel = questions [counter].level;
					subLevelCounter = 1;
				} else {
					++subLevelCounter;
				}
				questions [counter].subLevel = subLevelCounter;
				questions [counter].question = questionElements [1];
				questions [counter].answer = questionElements [2];	
				Debug.Log (questions [counter].question = questionElements [1]);
				++counter;
			}
		}
	}
		
	public Question GetQuestion(int level,int sublevel){
		foreach (Question question in questions) {
			Debug.Log (question);
			if (question.level == level && question.subLevel == sublevel) {
				return question;
			}
				
		}
		return null;
	}

}
