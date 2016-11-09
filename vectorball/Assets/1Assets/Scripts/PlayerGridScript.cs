using UnityEngine;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;

public enum Team { Home, Opponent}

public class PlayerGridScript : MonoBehaviour {

    [SerializeField]
    private PlayerScript[] playerScripts;
    [SerializeField]
    private GameObject denseGridHolderL, denseGridHolderR;

    public bool isDense { private set; get; }

	// Use this for initialization
	void Start () {
        UpdatePlayerGrid(true);
        playerScripts = GetComponentsInChildren<PlayerScript>();
	}

    public void UpdatePlayerGrid(bool isDense)
    {
        this.isDense = isDense;

        denseGridHolderL.SetActive(isDense);
        denseGridHolderR.SetActive(isDense);
    }
	
	public Vector3 GetAbsolutePosition(int x, int y)
    {
        print("Input x: " + x + " y:" + y);
        if (!isDense)
        {
            if (y > 2)
                y += 2;
            else if (y > 1)
                y++;
        }

        print("Processed x: " + x + " y:" + y);
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
                print(ps.name + " => x: " + xy[0] + " y:" + xy[1]);

                return selected.transform.position;
            }
        }
        print(xy + "  x: " + xy[0] + " y:" + xy[1]);
        print("Not Found. ERROR");
        return Vector3.zero;
    }
}
