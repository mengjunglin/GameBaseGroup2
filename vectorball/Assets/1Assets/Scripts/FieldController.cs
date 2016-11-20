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
    private GameObject denseGridHolderL, denseGridHolderR, positionLabel, arrowSprite;
  
	[SerializeField]
	private BallMoveBehavior ballMono;

    public bool isDense { private set; get; }

    public static FieldController instance;

    void Awake()
    {
        instance = this;
    }

    void Start () {
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
            PlayerScript attacker = GetNearestPlayer(ps);
            if (attacker)
                attacker.ChargeTowardsBall(percent, ballMono.target.position);
            else
                print("Err" + ps);
        }
    }

    PlayerScript GetNearestPlayer(PlayerScript target)
    {
        Vector2 coordinates = GetXYOfPlayer(target);
        PlayerScript playerBehind = GetPlayerAt(Mathf.RoundToInt(coordinates.x), Mathf.RoundToInt(coordinates.y - 1));
        if (playerBehind && playerBehind.isOpponent != target.isOpponent)
        {
            return playerBehind;
        }
        else
        {
            return GetPlayerAt(Mathf.RoundToInt(coordinates.x), Mathf.RoundToInt(coordinates.y + 1));
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
    }

    public void UpdatePlayerGrid(bool isDense)
    {
		this.isDense = isDense;

        denseGridHolderL.SetActive(isDense);
        denseGridHolderR.SetActive(isDense);

        playerScripts = GetComponentsInChildren<PlayerScript>();

        PlayerScript.ball = ballMono.transform;

        foreach (PlayerScript ps in playerScripts)
		{
            ps.idlePosition = ps.transform.position;

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
