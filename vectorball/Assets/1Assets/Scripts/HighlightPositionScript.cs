using UnityEngine;
using System.Collections;

public class HighlightPositionScript : MonoBehaviour {

	public Object highlightPrefab;
	// Use this for initialization
	void Start () {
		var center = transform.position;
		var pos = RandomCircle(center, 10);
		// make the object face the center
		var rot = Quaternion.FromToRotation(Vector3.forward, center);
		Instantiate(highlightPrefab, pos, Quaternion.Euler(new Vector3(90,90,0)));

		//PlayerScript ps = transform.GetComponent<PlayerScript>();
		//ps.idlePosition.z = 100;

		/*for (i = 0; i < numObjects; i++){
			var pos = RandomCircle(center, 10);
			// make the object face the center
			var rot = Quaternion.FromToRotation(Vector3.forward, center-pos);
			Instantiate(prefab, pos, rot);
		}*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Vector3 RandomCircle(Vector3 center, float radius){ 
		// create random angle between 0 to 360 degrees
		float ang = 0;
		Vector3 pos;
		pos.x = center.x;
		pos.y = center.y+2; 
		pos.z = center.z; 
		return pos;
	}
}
