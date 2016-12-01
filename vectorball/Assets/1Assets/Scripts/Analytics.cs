using UnityEngine;
using System.Collections;
using System.IO;

public class Analytics : MonoBehaviour {

	static string path;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void SelectedAnswer(int x,int y, int level, int flow, int pass, bool isCorrect)
    {
		StreamWriter sw = File.AppendText("analytics.csv");
		// Add some text to the file.
		if (new FileInfo ("analytics.csv").Length == 0) {
			sw.Write ("Path,Chosen X,Chosen Y,Level,Flow,Pass,Is Correct");
			sw.WriteLine ();
		}
		sw.WriteLine (path + "," + x + "," + y + "," + level + "," + flow + "," + pass + "," + isCorrect);
		// Close the writer and underlying file.
		sw.Flush();
		sw.Close();

    }

	public static void SelectedPath(string path){
		Analytics.path = path.Trim();
	}
}
