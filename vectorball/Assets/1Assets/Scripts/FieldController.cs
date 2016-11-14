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
    private GameObject denseGridHolderL, denseGridHolderR, ballPrefab;

    public static bool isDense { private set; get; }

    public static FieldController instance;

    void Awake()
    {
        instance = this;
    }

    void Start () {
		int count = 0;
		foreach (PlayerScript player in playerScripts) {
			if (true == player.isOpponent)
				opponentPlayerScripts [count++] = player;
		}
        UpdatePlayerGrid(true);
	}

    public void UpdatePlayerGrid(bool isDense)
    {
		FieldController.isDense = isDense;

        denseGridHolderL.SetActive(isDense);
        denseGridHolderR.SetActive(isDense);

        playerScripts = GetComponentsInChildren<PlayerScript>();

        foreach(PlayerScript ps in playerScripts)
        {
            ps.ball = ballPrefab.transform;
        }
    }
	
	public static Vector3 GetAbsolutePosition(int x, int y)
    {
		PlayerScript selectedPlayer = FieldController.instance.GetPlayerAt(x, y);

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

}
