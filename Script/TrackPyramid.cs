using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPyramid : MonoBehaviour {
	public static ArrayList diceInPyramid;
	public GameObject greenDice; 
	public GameObject blueDice;
	public GameObject yellowDice;
	public GameObject orangeDice;
	public GameObject whiteDice;

	// Use this for initialization
	void Start () {
		diceInPyramid = new ArrayList ();
		diceInPyramid.Add (greenDice);
		diceInPyramid.Add (blueDice);
		diceInPyramid.Add (yellowDice);
		diceInPyramid.Add (orangeDice);
		diceInPyramid.Add (whiteDice);
	}
	public static bool rollDice() {
		if (diceInPyramid.Count == 0) {
			return false;
		} else {
			int r = Random.Range (0, diceInPyramid.Count);
			GameObject d = (GameObject) diceInPyramid [r];
			diceInPyramid.RemoveAt (r);
			DiceLauncher.createDice (d);
			return true;
		}
	}
	void resetPyramid() {
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("Dice")) {
			Destroy (g);
		}
		diceInPyramid = new ArrayList ();
		diceInPyramid.Add (greenDice);
		diceInPyramid.Add (blueDice);
		diceInPyramid.Add (yellowDice);
		diceInPyramid.Add (orangeDice);
		diceInPyramid.Add (whiteDice);
		GameController.getPositions ();
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("space"))
			rollDice ();
		if (Input.GetKeyDown ("y"))
			resetPyramid ();
	}
}
