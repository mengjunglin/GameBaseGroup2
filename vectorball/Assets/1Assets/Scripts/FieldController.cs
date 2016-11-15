using UnityEngine;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;

public class FieldController : MonoBehaviour {

    [SerializeField]
    private static PlayerScript[] playerScripts;
	[SerializeField]
	private static PlayerScript[] opponentPlayerScripts;
    [SerializeField]
    private GameObject denseGridHolderL, denseGridHolderR, ballPrefab, positionLabel;

    public bool isDense { private set; get; }

    public static FieldController instance;

    void Awake()
    {
        instance = this;
    }

    void Start () {
        UpdatePlayerGrid(true,0);
	}

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.H))
    //        AnimatePlayerMood(true);
    //    if (Input.GetKeyDown(KeyCode.U))
    //        AnimatePlayerMood(false);
    //}

	public void UpdatePlayerGrid(bool isDense, int level)
    {
		this.isDense = isDense;

        denseGridHolderL.SetActive(isDense);
        denseGridHolderR.SetActive(isDense);

        playerScripts = GetComponentsInChildren<PlayerScript>();

		foreach(PlayerScript ps in playerScripts)
		{
			ps.ball = ballPrefab.transform;
            Transform label = ((GameObject) GameObject.Instantiate(positionLabel, ps.transform)).transform;
            label.localPosition = new Vector2(0, 30);
            label.rotation = Quaternion.Euler(0,90,0);
            ps.label = label.GetComponent<TextMesh>();

            if(level == 0)
            {
                Match positionIndices = Regex.Match(ps.name, "(?<=\\[).+?(?=\\])");
                string[] xy = positionIndices.Value.Split(',');
                ps.label.text = "(" + xy[0] + "," + xy[1] + ")";
            }
			else if (level == 1) {
				// Set new positions for level 2 

				//Find orignal positions.
				Match positionIndices = Regex.Match (ps.name, "(?<=\\[).+?(?=\\])");
				string[] xy = positionIndices.Value.Split (',');
				ps.setMultiplier (Random.Range (1, 4));
				int[] positions = { ps.getMultiplier () * int.Parse (xy [0]), ps.getMultiplier () * int.Parse (xy [1]) };
				string newPositions = positions [0].ToString () + "," + positions [1].ToString ();
				ps.name = "PositionA [" + newPositions + "]";

                ps.label.text = "(" + positions[0] + "," + positions[1] + ")";
            }
		}

		int count = 0;
		ArrayList players = new ArrayList ();
		foreach (PlayerScript player in playerScripts) {
			if (true == player.isOpponent)
				players.Add(player);
		}
		opponentPlayerScripts = new PlayerScript[players.Count];
		foreach (PlayerScript ps in players)
			opponentPlayerScripts [count++] = ps;
	
    }
	
	public Vector3 GetAbsolutePosition(int x, int y)
    {
		PlayerScript selectedPlayer = GetPlayerAt(x, y);

        if (selectedPlayer)
            return selectedPlayer.transform.position;
        else
            return Vector3.zero;
    }

    public PlayerScript GetPlayerAt(int x, int y)
    {
        //Debug.Log("Raw x: " + x + " y:" + y);
        if (!isDense)
        {
            if (y > 2)
                y += 2;
            else if (y > 1)
                y++;
        }

        //Debug.Log("Processed x: " + x + " y:" + y);
        PlayerScript selected = null;
        Match positionIndices;
        string[] xy = null;

        foreach (PlayerScript ps in playerScripts)
        {
            positionIndices = Regex.Match(ps.name, "(?<=\\[).+?(?=\\])");
            //if match not found throw error
            xy = positionIndices.Value.Split(',');
            if (xy[0] == x.ToString() && xy[1] == y.ToString())
            {
                selected = ps;
                //Debug.Log(ps.name + " => x: " + xy[0] + " y:" + xy[1]);

                return selected;
            }
        }

        //Debug.LogError("X,Y not foundin player Grid.");
        return null;
    }

	public PlayerScript GetRandomOpponent(){
		return opponentPlayerScripts[Random.Range(0,opponentPlayerScripts.Length)];
	}

    public void AnimatePlayerMood(bool homeTeamWon)
    {
        foreach(PlayerScript ps in playerScripts)
        {
            ps.PlayExpression((ps.isOpponent ^ homeTeamWon)); 
        }
    }
}
