using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System.Text.RegularExpressions;

public class ChooseOptionsManagerScript : MonoBehaviour {


	//Toggle Game Objects
	public Toggle[] isOptioni;
	public ToggleGroup optionsToggle;

	public GameObject ball;

    public Image timerBar;

    //Question textbox
    public Text questionText;

	public static int level = 2;

	GameSceneScript scoreScript;

	PlayerScript targetPlayer;

	private int[] startPositions;
	private int[] targetPositions;

	public int MaxQuestionsInLevel = 3;

	private int levelCounter = 1;

	public void Start(){
		scoreScript = GetComponent<GameSceneScript> ();
		startPositions = new int[] {0,0};
		LoadNextQuestion ();
	}

	public void Update(){
		//Check if all questions for level complete 
		if (levelCounter == MaxQuestionsInLevel) {
			levelCounter = 1;
			scoreScript.LevelComplete ();
		}
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
		scoreScript.SetOpponentPlayerScore (scoreScript.GetOpponentPlayerScore () + 1);

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

		++levelCounter;

		// remove highlight
		targetPlayer.ResetPlayer();

		//Check the selected option with correct option
		string option = ActiveOption ();

		string positionIndices = Regex.Match (option, "(?<=\\().+?(?=\\))").Value;

		string[] coordinates = positionIndices.Split(',');

		int x = int.Parse(coordinates[0]);
		int y = int.Parse(coordinates[1]);

		MoveBall (x,y);

		if (x == targetPositions[0] && y == targetPositions[1])
			IfCorrectOption (targetPositions);
		else
			IfIncorrectOption (new int[]{x,y});

        TimerScript.instance.StopTimer();


		// store the current player location before loading next question
		startPositions = targetPositions;

		LoadNextQuestion ();
	}

	public void IfCorrectOption(int[] option){
		//increase score
		scoreScript.SetPlayerScore (scoreScript.GetPlayerScore() + 1);

		//function to display vector notation
		VectorRepresentationScript script = GetComponent<VectorRepresentationScript>();
		script.convertResultToVector(true,level,option);
	}

	public void IfIncorrectOption(int[] option){
		//increase opponent's score
		scoreScript.SetOpponentPlayerScore (scoreScript.GetOpponentPlayerScore () + 1);

		VectorRepresentationScript script = GetComponent<VectorRepresentationScript>();
		script.convertResultToVector(false,level,option);
	}

	public void MoveBall(int x, int y){
		//ball.transform.position = Vector3.MoveTowards(ball.transform.position, FieldController.instance.GetAbsolutePosition(x,y), step);
		BallMoveBehavior e = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallMoveBehavior>();
		e.setTarget (FieldController.instance.GetPlayerAt(x,y).transform);

	}



	public void LoadNextQuestion(){

		//Make toggles clickable
		for (int i = 0; i < 4; ++i) {
			isOptioni [i].interactable = true;
		}

		//Clear all toggles
		optionsToggle.SetAllTogglesOff ();

		FindPlayer findPlayerScript = GetComponent<FindPlayer>();

		//Find next player to pass ball to
		//targetPositions = findPlayerScript.find_teammate(level, startPositions);
		targetPositions = new int[]{2,3};
		targetPlayer = FieldController.instance.GetPlayerAt (targetPositions [0], targetPositions [1]);

		//highlight player
		targetPlayer.HighlightPlayer ();

		//Set the question in the text box
		questionText.text = "Pass the ball to the hightlighted player";

		string[] options = new string[4];
		int[,] optionValues = new int[4,2];

		//Choose a random position to set correct option in
		int correctIndex = Random.Range (0, 3);
		optionValues [correctIndex,0] = targetPositions[0] - startPositions[0];
		optionValues [correctIndex,1] = targetPositions[1] - startPositions[1];
		options [correctIndex] = "("+ targetPositions[0].ToString() + "," + targetPositions [1].ToString() + ")";

		//Load random opponents
		for(int i=0;i<4;++i)
		{
			if (correctIndex != i) {
				PlayerScript ps = FieldController.instance.GetRandomOpponent ();
				string positionIndices = Regex.Match (ps.name, "(?<=\\[).+?(?=\\])").Value;
				while(options.Contains(positionIndices)){
					ps = FieldController.instance.GetRandomOpponent ();
					positionIndices = Regex.Match (ps.name, "(?<=\\[).+?(?=\\])").Value;
				}
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
		}

		//start timer
		TimerScript.instance.StartTimer (10);

	}
		
}
