﻿using UnityEngine;
using System.Collections;
using System;

public class BallMoveBehavior : MonoBehaviour {
	// handles
	[SerializeField] private Transform _bullseye;    // target transform
	public Transform target;
	[Range(20f, 70f)] public float _angle;      // shooting angle

    [SerializeField]
    float kickDelay;

	public float speed;

    public PlayerScript ballOwner;

    ///*
	public string lastPass;

	public int[,] tubeOptions; 

	public TubeController tb;

	public string message;

	public int[] result;

	GameObject optionsManager;
	public VectorRepresentationScript script;
	public bool printed = false;
    //*/


	void Start(){
		tubeOptions = new int[10,2]; 
		tb = TubeController.instance;

		optionsManager = GameObject.FindGameObjectWithTag ("OptionsManager");
		script = optionsManager.GetComponent<VectorRepresentationScript>();
	}

	void Update() {
		float step = speed * Time.deltaTime;
		/*if (target != null && !transform.position.Equals(target.position)) {
			transform.position = Vector3.MoveTowards (transform.position, target.position, step);
		}
		*/
		Rigidbody rb = GetComponent<Rigidbody>();
		SphereCollider sc = GetComponent<SphereCollider> ();

		if((_bullseye != null) && (((float)_bullseye.position.x - (float)transform.position.x) < 6.5) && (((float)_bullseye.position.x - (float)transform.position.x) > 0)){
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			if((sc != null) && sc.enabled.Equals(false)){
				sc.enabled = true;
			}
			if (lastPass.Equals ("Goal")) {
				//	transform.position = new Vector3 (-144.7f, 11.22563f, -0.6000003f);

				//	//score goal animation
				//	AudioSource cheerAudio =GameObject.Find("Cheer").GetComponent<AudioSource>();
				//	cheerAudio.Play ();
				//	GameObject.FindGameObjectWithTag ("GoalText").GetComponent<ParticleSystem> ().Play ();
				tb.AddPosition(FieldController.instance.GetAbsolutePosition (0,0));
				for (int i = 0; i < 10; ++i) {
					if (tubeOptions [i, 0] == 0 && tubeOptions [i, 1] == 0)
						break;
					tb.AddPosition (FieldController.instance.GetAbsolutePosition (tubeOptions [i, 0], tubeOptions [i, 1]));
				}
				tubeOptions = new int[10, 2];


				lastPass = "Done";
			}
			//} else if (lastPass.Equals ("OpponentGoal")) {
			//	transform.position = new Vector3 (-144.7f, 11.22563f, -0.6000003f);

			//	//score goal animation
			//	AudioSource booAudio = GameObject.Find("Boo").GetComponent<AudioSource>();
			//	booAudio.Play();
			//	//GameObject.FindGameObjectWithTag ("GoalText").GetComponent<ParticleSystem> ().Play();

			//	lastPass = "Done";
			//} 

			//	//function to display vector notation
				if (!printed) {
					script.convertResultToVector (message, result);
					printed = true;
				} 

			//transform.position = _bullseye.position;
		}

	}

	public void setTarget(Transform target){
        this.target = target;
		this._bullseye = target;

        if (ballOwner)
            ballOwner.PlayKickAnimation();

        //If u get an error here. Be sure to give a player0:0 reference to target(this script's target field) by default
        ballOwner = target.GetComponent<PlayerScript>();
        

        CancelInvoke("Launch");
		Invoke("Launch", kickDelay);
	}

	private void Launch()
	{
        print("Launch");

		// source and target positions
		Vector3 pos = transform.position;
		Vector3 target = _bullseye.position;

		// distance between target and source
		float dist = Vector3.Distance(pos, target);

		// rotate the object to face the target
		transform.LookAt(target);

		// calculate initival velocity required to land the ball on target using the formula (9)
		float Vi = Mathf.Sqrt(dist * -Physics.gravity.y / (Mathf.Sin(Mathf.Deg2Rad * _angle * 2)));
		float Vy, Vz;   // y,z components of the initial velocity

		Vy = Vi * Mathf.Sin(Mathf.Deg2Rad * _angle);
		Vz = Vi * Mathf.Cos(Mathf.Deg2Rad * _angle);

		// create the velocity vector in local space
		Vector3 localVelocity = new Vector3(0f, Vy, Vz);

		// transform it to global vector
		Vector3 globalVelocity = transform.TransformVector(localVelocity);

		// Before launching the ball, disable it's collision
		GetComponent<SphereCollider>().enabled = false;

		// launch the ball by setting its initial velocity
		GetComponent<Rigidbody>().velocity = globalVelocity;

	}

	public void closeCanvas(){
		script.disableCanvas ();
	}

}