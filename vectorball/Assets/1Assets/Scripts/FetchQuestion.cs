using UnityEngine;
using System.Collections;

public class FetchQuestion : MonoBehaviour {

    // Use this for initialization
    public GameObject question;
    string[][] questions;
    public string ques; 
    void Start () {
        question.GetComponent<TextMesh>().text = getQuestion(getLevel());
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    string getQuestion(int level)
    {
        //Function will read the questions - Done by Hussain
        return "Question 2";
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
