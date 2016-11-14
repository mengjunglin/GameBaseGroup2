using UnityEngine;
using System.Collections;

public class LearningObjective : MonoBehaviour {

    public GameObject objective;
    string ob;
    // Use this for initialization
	void Start () {
        objective.GetComponent<TextMesh>().text = getObjective(); ;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    string getObjective()
    {
        return ob;
    }
}
