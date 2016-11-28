using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FindPlayer : MonoBehaviour
{

	int[,,] player_pos = new int[4, 12, 2] { { { 0, 0 },{ -1, 1 },{ 1, 1 },{ 0, 2 },{ -1, 2 }, { 1, 2 }, { 0, 3 }, { -2, 3 }, { 2, 3 }, { 0, 5 },{ -1, 4 }, { 1, 4 } },      //level 1 grid
		{ { 0, 0 },{ -1, 1 },{ 1, 1 },{ 0, 2 },{ -1, 2 }, { 1, 2 }, { 0, 3 }, { -2, 3 }, { 2, 3 }, { 0, 5 },{ -1, 4 }, { 1, 4 } },         //level 2 grid - to be changed accordingly
		{ { 0, 0 },{ -1, 1 },{ 1, 1 },{ 0, 2 },{ -1, 2 }, { 1, 2 }, { 0, 3 }, { -2, 3 }, { 2, 3 }, { 0, 5 },{ -1, 4 }, { 1, 4 } },         //level 3 grid - to be changed accordingly
		{ { 0, 0 },{ -1, 1 },{ 1, 1 },{ 0, 2 },{ -1, 2 }, { 1, 2 }, { 0, 3 }, { -2, 3 }, { 2, 3 }, { 0, 5 },{ -1, 4 }, { 1, 4 } } };       //level 4 grid - to be changed accordingly
    int row, temp;
    int[] next_player = new int[2];
    string[] path;
	int selected_line;

	public TextAsset pathfile;

    void Start() {
        //path = System.IO.File.ReadAllLines(@"C:\Users\hussa\OneDrive\Documents\GitHub\GameBaseGroup2\vectorball\Assets\1Assets\Questions\path.txt");
		path = pathfile.text.Split('\n');
    }

    void Update() {

    }

	public int[] find_teammate(int level, int[] pos,int pass) {
		int flag = -1;
        row = pos[1];
		HashSet<int> exclude = new HashSet<int>() ;
		if (1 == pass) {
			selected_line = new System.Random ().Next (((level * 5) - 5), (level * 5));
		}
		if (null == path) {
			path = pathfile.text.Split('\n');
		}
			
        string current_path = path[selected_line];

        //To search where in path you are currently, and where to go to next, converting int to String, in order to use String Search methods.

        string current_row = System.Convert.ToString(row, 10);
        int indexOCR = current_path.IndexOf(current_row);
		if (indexOCR != -1 && indexOCR != current_path.Length-1) {
			//Uh, this might be incorrect - the line below
			int next_row = int.Parse (current_path.ToCharArray () [indexOCR + 1].ToString ());
			while (flag == -1) {
				//flag = Random.Range (0, 10);
				IEnumerable<int> range = Enumerable.Range(0, 12).Where(i => !exclude.Contains(i));
				int index = new System.Random().Next(0, 12 - exclude.Count);
				flag = range.ElementAt(index);
				exclude.Add (flag);

				if (player_pos [level, flag, 1] == next_row) {
					break;
				} else {
					flag = -1;
				}
			}

		} else {
			next_player [0] = player_pos [level, 9, 0];
			next_player [1] = player_pos [level, 9, 1];
		} 
		next_player [0] = player_pos [level, flag, 0];
		next_player [1] = player_pos [level, flag, 1];
		return next_player;
    }
}

