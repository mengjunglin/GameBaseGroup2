﻿using UnityEngine;
using System.Collections;

public class GameOverScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GoToMainMenu()
	{
		Application.LoadLevel ("MainMenu");
	}

	public void BackToLevel()
	{
		//int level = GameSceneScript.level;
		Application.LoadLevel ("GameScene");
	}
}
