using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VectorRepresentationScript : MonoBehaviour {


	public GameObject dialogueCanvasObj;

    public static VectorRepresentationScript instance;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () {

		GameObject dialogueCanvasObj = GameObject.FindGameObjectWithTag ("DialogueCanvas");

	}
	
	// Update is called once per frame
	void Update () {
		/*if (Input.GetKey (KeyCode.H)) {
			dialogueCanvasObj.SetActive (false);
		}*/
	}

	public void convertResultToVector (string message,int[] result)
	{
		// Format of result : {2,3}

		dialogueCanvasObj.SetActive (true);
		GameObject dialogueObj = GameObject.FindGameObjectWithTag ("Dialogue");


		GameObject resultObj = GameObject.FindGameObjectWithTag ("Result");

		dialogueObj.GetComponent<Text> ().text = message;
		resultObj.GetComponent<Text> ().text = string.Format ("{0} \n{1}", result [0], result [1]);
	}

	public void disableCanvas(){
		dialogueCanvasObj.SetActive (false);
	}

}
