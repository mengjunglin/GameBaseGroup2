﻿using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ClickExit()
	{
		Application.Quit ();
	}

	public void OpenScene()
	{
		Application.LoadLevel (1);
	}
}
