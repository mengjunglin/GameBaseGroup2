using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VectorRepresentationScript : MonoBehaviour {


	public GameObject dialogueCanvasObj;

	// Use this for initialization
	void Start () {

		GameObject dialogueCanvasObj = GameObject.FindGameObjectWithTag ("DialogueCanvas");
		//dialogueCanvasObj.SetActive(false);
		//convertResultToVector (true,0,"2,3");

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void convertResultToVector (bool status, int level, int[] result)
	{
		// Format of result : {2,3}
		string dialogueMessage = "";

		dialogueCanvasObj.SetActive(true);
		GameObject dialogueObj = GameObject.FindGameObjectWithTag ("Dialogue");


		GameObject resultObj = GameObject.FindGameObjectWithTag ("Result");

		if (!status) {
			dialogueMessage = "Whoops! You gave the ball to opponent. Try again!";
		} else {
			dialogueMessage = "Hurray! That was an awesome pass. In vector world it means";
		}
		dialogueObj.GetComponent<Text> ().text = dialogueMessage;

				
		resultObj.GetComponent<Text> ().text = string.Format ("{0} \n{1}", result [0], result [1]);

	}
}
