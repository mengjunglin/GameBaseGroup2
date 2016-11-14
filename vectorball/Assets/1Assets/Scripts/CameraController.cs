using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	private float zoomSpeed = 50.0f;

	void Update () {

		float scroll = Input.GetAxis("Mouse ScrollWheel");
			transform.Translate(0, scroll * zoomSpeed, scroll * zoomSpeed, Space.World);

		if (Input.GetKey(KeyCode.RightArrow)){
			transform.position += Vector3.right * zoomSpeed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.LeftArrow)){
			transform.position += Vector3.left * zoomSpeed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.UpArrow)){
			transform.position += Vector3.forward * zoomSpeed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.DownArrow)){
			transform.position += Vector3.back * zoomSpeed * Time.deltaTime;
		}


	}
}
