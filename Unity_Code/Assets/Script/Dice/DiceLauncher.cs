using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceLauncher : MonoBehaviour {
	public static Transform launcher;

	// Use this for initialization
	void Start () {
		launcher = transform;
	}
	
	// Update is called once per frame

	public static void createDice(GameObject d) {
		Instantiate (d.transform,launcher.position, Quaternion.identity);
	}
}
