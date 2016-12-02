using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class FieldController : MonoBehaviour {

    [SerializeField]
    private static PlayerScript[] playerScripts;
	[SerializeField]
	private static PlayerScript[] opponentPlayerScripts;
    [SerializeField]
    private GameObject denseGridHolderL, denseGridHolderR, positionLabel, arrowSprite;

	[SerializeField]
	private BallMoveBehavior ballMono;

	public static Transform startTransform;

    [SerializeField]
    public Transform[] goalsTransform;

    public bool isDense { private set; get; }

    public static FieldController instance;

	HashSet<int> exclude = new HashSet<int>() ;

    void Awake()
    {
        instance = this;
		startTransform = ballMono.transform;
		UpdatePlayerGrid(true);
    }

    void Start () {
		startTransform = ballMono.transform;
		UpdatePlayerGrid(true);
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
        Debug.Log("Timed out");
    }

    void OnTimerUpdate(float percent)
    {
        if(percent > PlayerScript.chargePercent)
        {
            PlayerScript ps = ballMono.target.GetComponent<PlayerScript>();
            PlayerScript attacker = GetNearestOpponent(ps);
            if (attacker)
                attacker.ChargeTowardsBall(percent, ballMono.target.position);
            else
                print("Err" + ps);
        }
    }

    PlayerScript GetNearestOpponent(PlayerScript target)
    {
        Vector2 coordinates = GetXYOfPlayer(target);
        PlayerScript opponent = GetPlayerAt(Mathf.RoundToInt(coordinates.x), Mathf.RoundToInt(coordinates.y - 1));
        if (opponent && opponent.isOpponent != target.isOpponent)
        {
            return opponent;
        }
        opponent = GetPlayerAt(Mathf.RoundToInt(coordinates.x), Mathf.RoundToInt(coordinates.y + 1));
        if (opponent && opponent.isOpponent != target.isOpponent)
        {
            return opponent;
        }
        opponent = GetPlayerAt(Mathf.RoundToInt(coordinates.x) + 1, Mathf.RoundToInt(coordinates.y));
        if (opponent && opponent.isOpponent != target.isOpponent)
        {
            return opponent;
        }
        opponent = GetPlayerAt(Mathf.RoundToInt(coordinates.x) -1, Mathf.RoundToInt(coordinates.y));
        if (opponent && opponent.isOpponent != target.isOpponent)
        {
            return opponent;
        }
        else
        {
            Debug.LogError("No opponent found");
            return null;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            AnimatePlayerMood(true);
        if (Input.GetKeyDown(KeyCode.U))
            AnimatePlayerMood(false);
        //if (Input.GetKeyDown(KeyCode.P))
        //    foreach (PlayerScript ps in playerScripts)
        //    {
        //        ps.HighlightPlayer();
        //    }
        //if (Input.GetKeyDown(KeyCode.T))
        //    TimerScript.instance.StartTimer(4);

        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    ballMono.target.GetComponent<PlayerScript>().PassToLastPlayerAndScore();
        //}
    }

    public void UpdatePlayerGrid(bool isDense)
    {
		this.isDense = isDense;

        denseGridHolderL.SetActive(isDense);
        denseGridHolderR.SetActive(isDense);

        playerScripts = GetComponentsInChildren<PlayerScript>();

		ballMono.transform.position = startTransform.position;

        PlayerScript.ball = ballMono.transform;

        foreach (PlayerScript ps in playerScripts)
		{
			
            ps.idlePosition = ps.transform.position;
			ps.permPosition = new Vector3(ps.idlePosition.x,ps.idlePosition.y,ps.idlePosition.z);
            //In case we need labels in future
            //Transform label = ((GameObject) GameObject.Instantiate(positionLabel, ps.transform)).transform;
            //label.localPosition = new Vector2(0, 30);
            //label.rotation = Quaternion.Euler(0,90,0);
            //ps.label = label.GetComponent<TextMesh>();

            Transform positionMarker = ((GameObject)GameObject.Instantiate(arrowSprite, ps.transform)).transform;
            positionMarker.localPosition = new Vector2(0, 35);
            positionMarker.gameObject.SetActive(false);
            ps.arrowMarker = positionMarker.GetComponent<SpriteRenderer>();
            Match positionIndices = Regex.Match(ps.name, "(?<=\\[).+?(?=\\])");
           	string[] xy = positionIndices.Value.Split(',');
           	if(ps.label)
                    ps.label.text = "(" + xy[0] + "," + xy[1] + ")";
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
            return selectedPlayer.idlePosition;
        else
            return Vector3.zero;
    }

	public Vector3 GetPermanentPosition(int x, int y)
	{
		PlayerScript selectedPlayer = GetPlayerAt(x, y);

		if (selectedPlayer)
			return selectedPlayer.permPosition;
		else
			return Vector3.zero;
	}

    public PlayerScript GetPlayerAt(int x, int y)
    {
        if (!isDense)
        {
            if (y > 2)
                y += 2;
            else if (y > 1)
                y++;
        }

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
                return selected;
            }
        }

        return null;
    }

    public Vector2 GetXYOfPlayer(PlayerScript ps)
    {
        Match positionIndices = Regex.Match(ps.name, "(?<=\\[).+?(?=\\])");
        //if match not found throw error
        string[] xy = positionIndices.Value.Split(',');
        return new Vector2(int.Parse(xy[0]), int.Parse(xy[1]));
    }

	public PlayerScript GetRandomOpponent(){
		IEnumerable<int> range = Enumerable.Range(0, opponentPlayerScripts.Length).Where(i => !exclude.Contains(i));
		int index = new System.Random().Next(0, opponentPlayerScripts.Length - exclude.Count);
		int flag = range.ElementAt(index);
		exclude.Add (flag);
		if (3 == exclude.Count)
			exclude.Clear ();
		return opponentPlayerScripts[flag];
	}

    public void AnimatePlayerMood(bool homeTeamWon)
    {
        foreach(PlayerScript ps in playerScripts)
        {
            ps.PlayExpression((ps.isOpponent ^ homeTeamWon)); 
        }
    }
}
