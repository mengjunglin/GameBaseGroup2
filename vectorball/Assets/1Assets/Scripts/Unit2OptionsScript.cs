using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Unit2OptionsScript : MonoBehaviour {

	//Toggle Game Objects
	public Toggle[] isOptioni;
	public ToggleGroup optionsToggle;
	private bool isCorrect;
	public Image timerBar;

	private int[] startPositions;
	private int[] targetPositions;
	//Question textbox
	public TextAsset unit2;
	string[] path;

	public TubeController tb1;
	public TubeController tb2;

	int quesNumber;

	void Start(){
		startPositions = new int[] {0,0,-1,0,0,1,-1,1};
		quesNumber = 0;
		path = unit2.text.Split('\n');
		targetPositions = ParseIntAnswers(LoadTarget().Split(','));
		tb1.clearLine ();
		tb1.CreateTube (startPositions);

		tb2.clearLine ();
		tb2.CreateTube (targetPositions);

	}
	
	// Update is called once per frame
	void Update () {
		// Update tube based on player movement
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
			FieldController.instance.GetPlayerAt (startPositions [j], startPositions [j + 1]).idlePosition.x = targetPlayers [j/2].idlePosition.x;
			FieldController.instance.GetPlayerAt (startPositions [j], startPositions [j + 1]).idlePosition.y = targetPlayers [j/2].idlePosition.y;
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

	public int[] ParseIntAnswers(string [] answerCordinates){
		int[] answers = new int[answerCordinates.Length];
		int i = 0;
		foreach (string ans in answerCordinates) {
			answers[i++] = int.Parse(ans);
		}
		return answers;
	}

	public string LoadTarget(){
		if (null == path) {
			path = unit2.text.Split('\n');
		}
		return path[quesNumber].ToString();	
	}
}
