using UnityEngine;
using System.Collections;

public class FetchQuestion : MonoBehaviour {

    // Use this for initialization
    public GameObject question;
    string[] questions;
    int ques_line;
    void Start () {
        question.GetComponent<TextMesh>().text = getQuestion(getLevel());
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    string getQuestion(int level)
    {
        ques_line = new Random().Next(((level*5)-5), (level*5));
        questions = System.IO.File.ReadAllLines(@"C:\Users\hussa\OneDrive\Documents\GitHub\GameBaseGroup2\vectorball\Assets\1Assets\Questions\Questions.txt");
        int first = questions[ques_line].IndexOf("<");
        int last = questions[ques_line].IndexOf(">");
        return questions[ques_line].Substring(first, last);
    }
    string[] getOptions(int level)
    {
        //Return the options read from TextAsset
        return new string[] { "Option2A", "Option2B", "Option2C", "Option2D" };
    }
    int getLevel()
    {
        return 1;
        //write code for getting Level from ?
    }
}
