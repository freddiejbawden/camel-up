using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camel : MonoBehaviour {
	public int currentPosition;
	public string id;

	// Use this for initialization

	public string getId() {
		return this.id;
	}
	public static int getPosition(string s) {
		GameObject g = GameObject.Find (s);
		Camel c = (Camel) g.GetComponent (typeof(Camel));
		return c.getPos ();
	}

	public static void stackCamels() {
		int count = 0;
		int space = 0;
		while (count < 5) {
			List<string> currentSpace = Spaces.spaces [space].getCamelOnSpaces ();
			if (currentSpace.Count > 0) {
				float height = 0.5f;
				Vector3 pos = Spaces.spaces [space].getPosition ();

				foreach (string o in currentSpace) {
					
					GameObject g = GameObject.Find (o.ToString ());
					g.transform.position = new Vector3 (pos.x, height, pos.z);
					height += 1.0f;
					count++;
				}
			}
			space++;
		}
	}
	public void updatePos(int newPos) {
		this.currentPosition = newPos;
	
	}
	public int getPos() {
		return currentPosition;
	}
	public void moveSpaces(int roll) {

		int newPos = currentPosition + roll;
		BoardSpace landingSpace = Spaces.spaces [newPos];
		newPos += landingSpace.getBonus ();
		Vector3 nextPosition = Spaces.spaces [this.currentPosition].getPosition ();
		if (newPos > 15) {
			GameController.getResults (newPos);
			return;
		}
		List<string> startingSpace = Spaces.spaces [this.currentPosition].getCamelOnSpaces ();

		//get the position of the camel in the stack
		int camelStackPos = startingSpace.IndexOf (this.id +"_camel");
		//Debug.Log ("Camel stack pos: " + camelStackPos);
		//get the block of camels to move 	
		List<string> blockToMove = startingSpace.GetRange (camelStackPos, startingSpace.Count - camelStackPos);

		Spaces.spaces[this.currentPosition].removeRange(camelStackPos, startingSpace.Count - camelStackPos);

		foreach (string s in blockToMove) {
			Spaces.addCamel (s, newPos);
		}

		Camel.stackCamels ();
	}
	// Update is called once per frame
	void Update () {
		
	}
}
