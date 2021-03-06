﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System.Text.RegularExpressions;

public class ChooseOptionsManagerScript : MonoBehaviour {


	//Toggle Game Objects
	public Toggle[] isOptioni;
	public ToggleGroup optionsToggle;
	private bool isCorrect;

	public GameObject ball;

    public Image timerBar;

    //Question textbox
    public Text questionText;

	GameSceneScript scoreScript;

	FindPlayer findPlayerScript;

    PlayerScript targetPlayer;
    [SerializeField]
    Transform defaultPlayer;

	private int index=0;

	private int[] startPositions;
	private int[] targetPositions;
	private int[] chosenPositions;

	public int MaxFlowsInLevel;

	public int MaxPassesInFlow;

	public static int flow = 0;

	public static int level = 1;

	public static int pass = 0;

	public int tries = 0;

	public GameObject dialogueCanvasObj;

	public void Start(){
		scoreScript = GetComponent<GameSceneScript> ();
		findPlayerScript = GetComponent<FindPlayer>();
		startPositions = new int[] {0,0};
		chosenPositions = new int[] {0,0};

		//Find next player to pass ball to
		isCorrect = true;
		LoadNextQuestion ();
	}

    void OnEnable()
    {
        TimerScript.instance.TimeoutEvent += OnTimeOut;
        TimerScript.instance.TimerUpdateEvent += OnTimerUpdate;
    }

    void OnDisable()
    {
        TimerScript.instance.TimeoutEvent -= OnTimeOut;
        TimerScript.instance.TimerUpdateEvent -= OnTimerUpdate;
    }

    void OnTimeOut()
    {
		for (int i = 0; i < 4; ++i) {
			isOptioni [i].interactable = false;
		}
        
		BallMoveBehavior e = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallMoveBehavior>();

		tries += 1;
		Analytics.SelectedAnswer(0, 0, targetPositions[0],targetPositions[1], level, flow+1, pass, isCorrect,TimerScript.instance.secsPassed);
		Vector2 ballOwnerCoords = FieldController.instance.GetXYOfPlayer (e.ballOwner);
		chosenPositions [0] = (int)ballOwnerCoords.x;
		chosenPositions [1] = (int)ballOwnerCoords.y;
		isCorrect = false;
		targetPlayer.ResetPlayer();
		LoadNextQuestion ();
		//ResetField ();
    }

    void OnTimerUpdate(float percent)
    {
       timerBar.fillAmount = 1 - percent;
    }

    //Check which option is active
    public string ActiveOption(){
		//Set the chosen option as A or B or C or D below
		string option = " ";
		for (int i = 0; i < 4; ++i) {
			if(isOptioni[i].isOn)
				option = isOptioni[i].transform.FindChild("Answer").GetComponent<Text>().text;
		}
		return option;
	}

	public void OnSubmit(){

		float timeTaken = TimerScript.instance.secsPassed;
		TimerScript.instance.StopTimer();

		for (int i = 0; i < 4; ++i) {
			isOptioni [i].interactable = false;
		}

		// remove highlight
		targetPlayer.ResetPlayer();

		//Check the selected option with correct option
		string option = ActiveOption ();

		string positionIndices = Regex.Match (option, "(?<=\\().+?(?=\\))").Value;

		string[] coordinates = positionIndices.Split(',');

		int x = int.Parse(coordinates[0]);
		int y = int.Parse(coordinates[1]);

		chosenPositions [0] = x;
		chosenPositions [1] = y;

		BallMoveBehavior e = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallMoveBehavior>();
		e.tubeOptions [index,0] = chosenPositions [0];
		e.tubeOptions [index,1] = chosenPositions [1];
		++index;
			
        if (x == targetPositions [0] && y == targetPositions [1]) {
			startPositions [0] = targetPositions [0];
			startPositions [1] = targetPositions [1];
			IfCorrectOption (chosenPositions);
		}
		else {
			IfIncorrectOption (chosenPositions);
		}
        
		Analytics.SelectedAnswer(x, y, targetPositions[0],targetPositions[1],level, flow+1, pass, isCorrect,timeTaken);
		BeforeLoad ();
		MoveBall (x,y);
	}

	public void IfCorrectOption(int[] option){

		isCorrect = true;
		++pass; 
		tries = 0;

		//function to display vector notation
		GameObject ball = GameObject.FindGameObjectWithTag ("Ball");
		BallMoveBehavior script = ball.GetComponent<BallMoveBehavior>();
		script.message = "Hurray! That was an awesome pass. You passed to:";
		script.result = option;
       // VectorRepresentationScript.instance.convertResultToVector("Hurray! That was an awesome pass. You passed to:", option);
		script.printed = false;
	}

	public void IfIncorrectOption(int[] option){

		isCorrect = false;
		++tries;
		GameObject ball = GameObject.FindGameObjectWithTag ("Ball");
		if (tries > 1) {
			//increase opponent's score
			scoreScript.SetOpponentPlayerScore (scoreScript.GetOpponentPlayerScore () + 1);

					//reset counters
					tries = 0;
					pass = 0;

            //Call this after some animation - delay - Opponent Goal animation
            ball.GetComponent<BallMoveBehavior> ().lastPass = "OpponentGoal";

            ball.GetComponent<BallMoveBehavior>().ballOwner.PassToLastPlayerAndScore(true);
		}
        else
        {
            BallMoveBehavior ballMono = ball.GetComponent<BallMoveBehavior>();
            ballMono.setTarget(FieldController.instance.GetPlayerAt(option[0], option[1]).transform);
        }

		BallMoveBehavior script = ball.GetComponent<BallMoveBehavior>();
		script.message = "Whoops! You gave the ball to opponent. You passed to";
		script.result = option;
		script.printed = false;

        //VectorRepresentationScript.instance.convertResultToVector("Whoops! You gave the ball to opponent. You passed to:", option);
    }

	public void MoveBall(int x, int y){
		//ball.transform.position = Vector3.MoveTowards(ball.transform.position, FieldController.instance.GetAbsolutePosition(x,y), step);
		BallMoveBehavior e = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallMoveBehavior>();
		e.setTarget (FieldController.instance.GetPlayerAt(x,y).transform);

	}


	public void BeforeLoad(){
		if (pass == MaxPassesInFlow) {
			GameObject ball = GameObject.FindGameObjectWithTag ("Ball");
			//increase score
			//scoreScript.SetPlayerScore (scoreScript.GetPlayerScore () + 1);


			//Set counter to next question
			flow++; 
			pass = 0;
			index = 0;

            //Call after some delay - goal animation
            ball.GetComponent<BallMoveBehavior> ().lastPass = "Goal";

            ball.GetComponent<BallMoveBehavior>().ballOwner.PassToLastPlayerAndScore(false);

            //ResetField ();
		}
	}
    
	public void ResetField(){

		//Reset field
		FieldController.instance.UpdatePlayerGrid (true);
		//MoveBall(0,0);
		startPositions = new int[] {0,0};
		chosenPositions = new int[]{0,0};
        print("Reset field");
        bool inOpponentsGoal = ball.GetComponent<BallMoveBehavior>().target.name.Contains("Opponent");
        ball.GetComponent<BallMoveBehavior>().setTarget(defaultPlayer);
        tries = 0;
        index = 0;
        flow++;
        pass = 0;

        if(flow > MaxFlowsInLevel)
        {
            flow = 0;
            scoreScript.LevelComplete();
        }

        CancelInvoke("LoadNextQuestion");
        Invoke("LoadNextQuestion", inOpponentsGoal ? 5 : 2);
    }


	public void LoadNextQuestion(){
        print("Load next question " + flow + " " + pass + " " + level);
        VectorRepresentationScript.instance.disableCanvas();

        if (ball.GetComponent<BallMoveBehavior>().target.name.Contains("Goal") || IsInvoking("LoadNextQuestion") 
            || tries > 1 || pass >= MaxPassesInFlow)
        {
            print("Ignore load next");
            return;
        }

		if (isCorrect) {
			targetPositions = findPlayerScript.find_teammate (level, startPositions, pass , flow);
		}

		BallMoveBehavior e = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallMoveBehavior>();
		if (null != e.tb) {
			e.tb.clearLine ();

		}
		//Make toggles clickable
		for (int i = 0; i < 4; ++i) {
			isOptioni [i].interactable = true;
		}

		//Clear all toggles
		optionsToggle.SetAllTogglesOff ();

		targetPlayer = FieldController.instance.GetPlayerAt (targetPositions [0], targetPositions [1]);

		//highlight player
		targetPlayer.HighlightPlayer ();

		//Set the question in the text box
		questionText.text = "Pass the ball to the hightlighted player";

		string[] options = new string[4];
		int[,] optionValues = new int[4,2];

		//Choose a random position to set correct option in
		int correctIndex = Random.Range (0, 3);
		optionValues [correctIndex,0] = targetPositions[0] - chosenPositions[0];
		optionValues [correctIndex,1] = targetPositions[1] - chosenPositions[1];
		options [correctIndex] = "("+ targetPositions[0].ToString() + "," + targetPositions [1].ToString() + ")";

		//Load random opponents
		for(int i=0;i<4;++i)
		{
			if (correctIndex != i) {
				PlayerScript ps = FieldController.instance.GetRandomOpponent ();
				string positionIndices = Regex.Match (ps.name, "(?<=\\[).+?(?=\\])").Value;
				optionValues [i,0] = int.Parse(positionIndices.Split (',')[0]) - startPositions[0];
				optionValues [i,1] = int.Parse(positionIndices.Split (',')[1]) - startPositions[1];

				options [i] = "("+ positionIndices + ")";
			}

			isOptioni[i].transform.FindChild("Answer").GetComponent<Text>().text = options[i];

		}
		if (level == 1) {
			for (int i = 0; i < 4; ++i) {

				Text optionText = isOptioni [i].transform.FindChild ("VectorValue1").transform.FindChild ("Value").GetComponent<Text>();
				optionText.text = string.Format ("{0} \n{1}", optionValues [i, 0], optionValues [i, 1]);

			}
					
		} else if (level == 2) {
			for (int i = 0; i < 4; ++i) {
				int multiplier = Random.Range (2, 5);
				optionValues [i,0] *= multiplier;
				optionValues [i,1] *= multiplier;
		
				Text optionText = isOptioni [i].transform.FindChild ("VectorValue1").transform.FindChild ("Value").GetComponent<Text>();
				optionText.text = string.Format ("{0} \n{1}", optionValues [i, 0], optionValues [i, 1]);

				Text multiplierText = isOptioni [i].transform.FindChild ("VectorValue1").transform.FindChild ("Multiplier1").GetComponent<Text>();
				multiplierText.text = string.Format ("{0}\n---\n{1}", 1, multiplier);

				GameObject multiplierObj = isOptioni [i].transform.FindChild ("VectorValue1").transform.FindChild ("Multiplier1").gameObject;
				multiplierObj.SetActive (true);

			}
		} else if (level == 3) {
			for (int i = 0; i < 4; ++i) {
				int x = Random.Range (-2, optionValues [i,0]-1);
				int y = Random.Range (-2, optionValues [i,1]-1);

				Text optionText1 = isOptioni [i].transform.FindChild ("VectorValue1").transform.FindChild ("Value").GetComponent<Text>();
				optionText1.text = string.Format ("{0} \n{1}", x, y);

				Text optionText2 = isOptioni [i].transform.FindChild ("VectorValue2").transform.FindChild ("Value").GetComponent<Text>();
				optionText2.text = string.Format ("{0} \n{1}", optionValues [i, 0] - x, optionValues [i, 1] - y);

				GameObject optionObj2 = isOptioni [i].transform.FindChild ("VectorValue2").transform.gameObject;
				optionObj2.SetActive (true);

				isOptioni [i].transform.FindChild ("Addition").gameObject.SetActive (true);

			}
		}

		//start timer
		TimerScript.instance.StartTimer (30);
	}



		
}
