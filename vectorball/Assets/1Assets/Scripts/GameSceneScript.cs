using UnityEngine;
using System.Collections;

public class GameSceneScript : MonoBehaviour {
	static public int level = 1;
	private int playerScore = 0;
	private int opponentScore = 0;

	// Use this for initialization
	void Start () {
		//these should be updated throughout the game
		playerScore = 1;
		opponentScore = 2;
		Debug.Log("GameLevel="+level);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LevelComplete()
	{
		if (playerScore > opponentScore) {
			//direct to next level
			level++; //advance to next level
			Application.LoadLevel (0); //TODO: replace 0 with next level's scene name
		} else {
			Application.LoadLevel ("GameOverScene");
		}
	}
}
