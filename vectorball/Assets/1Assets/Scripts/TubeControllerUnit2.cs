using UnityEngine;
using System.Collections;

public class TubeControllerUnit2 : MonoBehaviour {

	private bool initialized = false;
	public TubeController tb1;
	public TubeController tb2;

	public static TubeControllerUnit2 instance;
	//public Material redTube;
	//public Material blueTube;

	// Use this for initialization
	void Awake () {
		//GetComponent<TubeRenderer>.AddPosition 
		instance = this;

	}
	
	// Update is called once per frame
	void Update () {
		if (TubeController.instance != null & initialized == false) {
			initializeTube ();
			initialized = true;
		}
	}

	void initializeTube(){
		tb1.AddPosition(FieldController.instance.GetAbsolutePosition (-1, 1));
		tb1.AddPosition(FieldController.instance.GetAbsolutePosition (-1, 2));
		tb1.AddPosition(FieldController.instance.GetAbsolutePosition (0, 1));
		tb1.AddPosition(FieldController.instance.GetAbsolutePosition (0, 2));

		tb2.AddPosition(FieldController.instance.GetAbsolutePosition (-1, 1));
		tb2.AddPosition(FieldController.instance.GetAbsolutePosition (0, 3));
		tb2.AddPosition(FieldController.instance.GetAbsolutePosition (0, 1));
		tb2.AddPosition(FieldController.instance.GetAbsolutePosition (1, 3));
		//TubeController.instance.setMaterial (redTube);
	}
}
