using UnityEngine;
using System.Collections;

public class FindPlayer : MonoBehaviour {

    int[,,] player_pos = new int[4,10,2] { { { 0, 0 }, { 0, 2 }, { -1, 2 }, { 1, 2 }, { 0, 3 }, { -2, 3 }, { 2, 3 }, { 0, 5 }, { -1, 1 }, { 1, 1 } },      //level 1 grid
                                        { { 0, 0 }, { 0, 2 }, { -1, 2 }, { 1, 2 }, { 0, 3 }, { -2, 3 }, { 2, 3 }, { 0, 5 }, { -1, 1 }, { 1, 1 } },         //level 2 grid - to be changed accordingly
                                        { { 0, 0 }, { 0, 2 }, { -1, 2 }, { 1, 2 }, { 0, 3 }, { -2, 3 }, { 2, 3 }, { 0, 5 }, { -1, 1 }, { 1, 1 } },         //level 3 grid - to be changed accordingly
                                        { { 0, 0 }, { 0, 2 }, { -1, 2 }, { 1, 2 }, { 0, 3 }, { -2, 3 }, { 2, 3 }, { 0, 5 }, { -1, 1 }, { 1, 1 } } };       //level 4 grid - to be changed accordingly
    int row, temp;
    int[] next_player = new int[2];
    string[] path;
    void Start () {
        path = System.IO.File.ReadAllLines(@"C:\Users\hussa\OneDrive\Documents\GitHub\GameBaseGroup2\vectorball\Assets\1Assets\Questions\path.txt");
    }
	
	void Update () {
	
	}

    public int[] find_teammate(int level, int[] pos){
        row = pos[1];
  
        int selected_line = new Random().Next(((level * 5) - 5), (level * 5));
        string current_path = path[selected_line];
        
        //To search where in path you are currently, and where to go to next, converting int to String, in order to use String Search methods.
        string current_row = Convert.ToString(row, 10);
        int indexOCR = current_path.IndexOf(current_row);
        if (indexOCR < (current_path.Length - 1))
        {
            //Uh, this might be incorrect - the line below
            int next_row = Convert.ToInt32(current_path[pos + 1], 10);
            while (true)
            {
                temp = new Random().Next(10);

                // player_pos[next_row,y,level]
                //bad code below. Inefficient, bad design, but works

                int flag = 0;
                for (int x = 0; x < 10; x++){
                    if(temp == player_pos[level, x, 0] && player_pos[level, x, 1] == row)
                    {
                        flag = x;
                    }
                }
                if (flag != 0){ //that means we've found our random player in the next row
                    break;
                }
                
            }
        }
        next_player[0] = player_pos[level, flag, 0];
        next_player[1] = player_pos[level, flag, 1];
        return next_player;
    }
}
