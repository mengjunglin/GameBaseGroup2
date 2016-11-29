using UnityEngine;
using System.Collections;

public class Unit2OptionsScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void MovePlayer(){
		
		PlayerScript targetPlayer1 = FieldController.instance.GetPlayerAt (0,3);
		PlayerScript targetPlayer2 = FieldController.instance.GetPlayerAt (1,3);
		targetPlayer1.gameObject.GetComponentInChildren<Transform>().FindChild("Char_2 (1)").gameObject.SetActive(true);
		targetPlayer2.gameObject.GetComponentInChildren<Transform>().FindChild("Char_2 (1)").gameObject.SetActive(true);

		PlayerScript sourcePlayer1 = FieldController.instance.GetPlayerAt (-1,2);
		PlayerScript sourcePlayer2 = FieldController.instance.GetPlayerAt (0,2);
		sourcePlayer1.gameObject.GetComponentInChildren<Transform>().FindChild("Char_2 (1)").gameObject.SetActive(false);
		sourcePlayer2.gameObject.GetComponentInChildren<Transform>().FindChild("Char_2 (1)").gameObject.SetActive(false);

		TubeControllerUnit2.instance.tb1.clearLine ();
	}
}
