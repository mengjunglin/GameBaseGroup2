using UnityEngine;
using System.Collections;
using System.Linq;

public enum Team { Home, Opponent}

public class PlayerGridScript : MonoBehaviour {

    [SerializeField]
    private PlayerScript[] playerTransforms;
    [SerializeField]
    private GameObject denseGridHolderL, denseGridHolderR;

    public bool isDense;

	// Use this for initialization
	void Start () {
        UpdatePlayerGrid();
        playerTransforms = GetComponentsInChildren<PlayerScript>();
	}

    public void UpdatePlayerGrid()
    {
        denseGridHolderL.SetActive(isDense);
        denseGridHolderR.SetActive(isDense);
    }
	
	public Vector3 GetAbsolutePosition(int x, int y)
    {
        //PlayerScript selected = ;
        return Vector3.zero;
    }
}
