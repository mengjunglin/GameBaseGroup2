using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TubeControllerUnit2 : MonoBehaviour {


	//Toggle Game Objects
	public Toggle[] isOptioni;
	public ToggleGroup optionsToggle;
	private bool isCorrect;
	public Image timerBar;

	private bool initialized = false;
	public TubeController tb1;
	public TubeController tb2;

	private int[] startPositions;
	private int[] targetPositions;
	private int[] updatePositions;
	//Question textbox
	public TextAsset unit2;
	public TextAsset unit2wrong;
	public TextAsset unit2answers;

	string[] unit2path;
	string[] unit2wrongpath;
	string[] unit2answerspath;
	string loadedAnswer;
	string[] answers;
	string[] wrong;

	PlayerScript [] players;


	public PlayerScript[] playerRefs;

	int playerReachCount = 5;


	int quesNumber;

	public static TubeControllerUnit2 instance;
	//public Material redTube;
	//public Material blueTube;

	void OnTimerUpdate(float percent)
	{
		timerBar.fillAmount = 1 - percent;
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


	// Use this for initialization
	void Awake () {
		//GetComponent<TubeRenderer>.AddPosition 
		instance = this;
		LoadTarget ();
		//players = new PlayerScript[4];
		startPositions = new int[] {0,0,-1,0,0,1,-1,1};
	}
	
	// Update is called once per frame
	void Update () {
		if (TubeController.instance != null & initialized == false) {
			//initializeTube ();
			tb1.clearLine ();
			tb2.clearLine ();
			CreateTube (startPositions, tb1);
			CreateTube (targetPositions, tb2);
			initialized = true;
		}
		/*
		if(playerReachCount < 5){
			for (int l = 0; l < 4; l++) {
					//tb1.clearLine ();
					//tb1.AddPosition (players [l].transform.position);
					if (players [l].transform.position.Equals (players [l].idlePosition)) {
						playerReachCount++;
					}			
				}
			//tb1.clearLine ();
		}
		if (playerReachCount == 4) {
			//tb1.clearLine ();
			playerReachCount = 5;
		}
		*/
	}

	public void CreateTube(int[] x, TubeController tb){
		for (int i = 0; i < 8; i = i + 2) {
			tb.AddPosition (FieldController.instance.GetPermanentPosition (x [i], x [i+1]));
			if (tb.Equals (tb1)) {
				tb.SetTubeThickness (1);
			}
		}
	}

	public void LoadTarget(){

		if (quesNumber == 2)
			Application.LoadLevel (7);

		if (null == unit2path) {
			unit2path = unit2.text.Split('\n');
		}
		loadedAnswer = unit2path[quesNumber].ToString();
		targetPositions = ParseIntAnswers(loadedAnswer.Split(','));
		TimerScript.instance.StartTimer (50);

		if (null == unit2answerspath) {
			unit2answerspath = unit2answers.text.Split('\n');
		}
		answers = unit2answerspath[quesNumber].ToString().Split('/');

		if (null == unit2wrongpath) {
			unit2wrongpath = unit2wrong.text.Split('\n');
		}
		wrong = unit2wrongpath[quesNumber].ToString().Split(',');

		//Make toggles clickable
		for (int i = 0; i < 16; i+=4) {

			isOptioni[i/4].transform.FindChild("Answer").GetComponent<Text>().text = answers[i/4];

			Text optionText = isOptioni [i/4].transform.FindChild ("MatrixValue1").transform.FindChild ("Value").GetComponent<Text>();
			optionText.text = string.Format ("{0} {1}\n{2} {3}", wrong [i], wrong [i+1],wrong [i+2],wrong [i+3]);

			isOptioni [i/4].interactable = true;
		}

		//Clear all toggles
		optionsToggle.SetAllTogglesOff ();
	}

	public int[] ParseIntAnswers(string [] answerCordinates){
		int[] answers = new int[answerCordinates.Length];
		int i = 0;
		foreach (string ans in answerCordinates) {
			answers[i++] = int.Parse(ans);
		}
		return answers;
	}


	public void MovePlayer(){


		TimerScript.instance.StopTimer();

		for (int i = 0; i < 4; ++i) {
			isOptioni [i].interactable = false;
		}

		//Check the selected option with correct option
		string option = ActiveOption ();

		int[] answers = ParseIntAnswers(option.Split (','));
		PlayerScript [] targetPlayers = new PlayerScript[4];
		for (int k = 0; k < 8; k+=2) {
			targetPlayers [k/2] = FieldController.instance.GetPlayerAt (answers [k], answers [k + 1]); 
		}

		for (int j = 0; j < 8; j+=2) {
			/*PlayerScript startPlayer = FieldController.instance.GetPlayerAt (startPositions [j], startPositions [j + 1]);
			FieldController.instance.GetPlayerAt (startPositions [j], startPositions [j + 1]).lastPosition = FieldController.instance.GetPlayerAt (startPositions [j], startPositions [j + 1]).idlePosition;
			FieldController.instance.GetPlayerAt (startPositions [j], startPositions [j + 1]).idlePosition = targetPlayers[j/2].permPosition; */

			playerRefs[j/2].lastPosition = playerRefs[j/2].idlePosition;
			playerRefs[j/2].idlePosition = targetPlayers [j / 2].permPosition;
			//players[j/2] = playerRefs[j/2];
			playerReachCount = 0;


			//playerRefs [j / 2] = FieldController.instance.GetPlayerAt (startPositions [j], startPositions [j + 1]);
		}

		if (option.Equals (loadedAnswer)) {
			quesNumber++;

			for (int p=0; p < 8; p++) {
				startPositions [p] = targetPositions [p];
			}
			LoadTarget ();

			initialized = false;
			// Load Next Question
		} else {
			// Try Again
			LoadTarget ();

			initialized = false;
		}
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

	void OnTimeOut()
	{
		for (int i = 0; i < 4; ++i) {
			isOptioni [i].interactable = false;
		}

	}
}
