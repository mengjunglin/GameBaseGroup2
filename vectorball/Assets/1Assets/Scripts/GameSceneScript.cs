using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameSceneScript : MonoBehaviour {
	private int playerScore;
	private int opponentScore;


	//Score text box
	public Text scoreText;
	public Text opponentScoreText;

	// Use this for initialization
	void Start () {
		//these should be updated throughout the game
		playerScore = 0;
		opponentScore = 0;
	}
	
	// Update is called once per frame
	public void Update(){
		scoreText.text = playerScore.ToString();
		opponentScoreText.text = opponentScore.ToString();
	}

	public void SetPlayerScore(int score){
		playerScore = score;
	}

	public void SetOpponentPlayerScore(int score){
		opponentScore = score;
	}

	public int GetPlayerScore(){
		return playerScore;
	}

	public int GetOpponentPlayerScore(){
		return opponentScore;
	}

	public void LevelComplete()
	{
		//if player score>opponentand , update grid => next level
		if (playerScore > opponentScore) {
			//direct to next level
			//Application.LoadLevel (0); //TODO: replace 0 with next level's scene name
			FieldController.instance.UpdatePlayerGrid (true, 1);
			GetComponent<ChooseOptionsManagerScript>().LoadNextQuestion ();
		} else {
			Application.LoadLevel ("GameOverScene");
		}
	}
}
