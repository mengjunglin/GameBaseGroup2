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
			//FieldController.instance.UpdatePlayerGrid (true, 1);
			//GetComponent<ChooseOptionsManagerScript>().LoadNextQuestion ();
			ChooseOptionsManagerScript.level++; //advance to next level - commeneted next line as we are using same scene for level 2 as well
            PlayerPrefs.SetInt("UnlockedTillLevel", ChooseOptionsManagerScript.level);
			Application.LoadLevel ("MainMenu");
            //Application.LoadLevel (0); //TODO: replace 0 with next level's scene name
        } else {
			Application.LoadLevel ("GameOverScene");
		}
	}
}
