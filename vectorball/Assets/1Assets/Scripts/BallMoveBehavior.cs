using UnityEngine;
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
			//transform.position = _bullseye.position;
		}
	}

	public void setTarget(Transform target){
        Transform ballOwner = this.target;
        this.target = target;
		this._bullseye = target;

        //If u get an error here. Be sure to give a player0:0 reference to target(this script's target field) by default
        ballOwner.GetComponent<PlayerScript>().PlayKickAnimation();

        CancelInvoke("Launch");
		Invoke("Launch", kickDelay);
	}

	private void Launch()
	{
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
}