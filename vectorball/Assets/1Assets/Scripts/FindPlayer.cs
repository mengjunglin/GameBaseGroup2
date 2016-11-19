using UnityEngine;
using System.Collections;

public class FindPlayer : MonoBehaviour {

    int[,] player_pos = new int[,] { { 0, 0 }, { 0, 2 }, { -1, 2 }, { 1, 2 }, { 0, 3 }, { -2, 3 }, { 2, 3 }, { 0, 5 }, { -1, 1 }, { 1, 1 } };
    int row, temp;
    int[] next_player = new int[2];
    void Start () {
	
	}
	
	void Update () {
	
	}

    public int[] find_teammate(int[] pos){
        row = pos[1];
        while (true)
        {
			temp = Random.Range(0,10);
            if (player_pos[temp, 1] == row && player_pos[temp, 0] != pos[0])
            {
                break;
            }
        }
        next_player[0] = player_pos[temp, 0];
        next_player[1] = player_pos[temp, 1];
        return next_player;
    }
}
