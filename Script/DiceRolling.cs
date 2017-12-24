using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRolling : MonoBehaviour {
	public Rigidbody rb;
	public string color; 
	public static Vector3 prev = new Vector3(0,0,0);
	private bool stopped = false;
	// Use this for initialization
	void Start () {
		Fling ();
	}

	// Update is called once per frame
	void Update () {
		if (stopped == false) 
		if (stoppedMoving ()) {
			stopped = true;
			int val = getSideUp ();
			Debug.Log (color + "_camel");
			GameObject g = GameObject.Find (color + "_camel");
			Camel c = (Camel)g.GetComponent (typeof(Camel));
			c.moveSpaces (val);
			GameController.nextTurn ();
		}
	}
	bool stoppedMoving () {
		float dist = Vector3.Distance (prev, transform.position);
		if (dist < 0.001) {
			return true;
		}
		prev = transform.position;
		return false;
	}
	int getSideUp() {
		ArrayList faceAngles = new ArrayList ();
		faceAngles.Add(Vector3.Angle (transform.up, Vector3.up));
		faceAngles.Add(Vector3.Angle (-transform.up, Vector3.up));
		faceAngles.Add(Vector3.Angle (transform.right, Vector3.up));
		faceAngles.Add(Vector3.Angle (-transform.right, Vector3.up));
		faceAngles.Add(Vector3.Angle (transform.forward, Vector3.up));
		faceAngles.Add(Vector3.Angle (-transform.forward, Vector3.up));
		float min = (float) faceAngles [0];
		int index = 0;
		for (int i = 1; i < 6; i++) {
			float testMag = (float) faceAngles[i];
			if (testMag < min) {
				min = testMag;
				index = i;
			}
		}

		if (index == 1 || index == 3) {
			return 1;
		} else if (index == 0 || index == 4) {
			return 2;
		} else {
			return 3;
		}	
	}
	void Fling () {
		rb.AddForce (0, 200, 400);
		rb.AddTorque (transform.right * 1 * Random.Range(-10,10));
		rb.AddTorque (transform.up * 1 * Random.Range(-10,10));
	}
}
