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
			List<string> currentSpace = Board.spaces [space].getCamelOnSpaces ();
			if (currentSpace.Count > 0) {
				float height = 0.5f;
				Vector3 pos = Board.spaces [space].getPosition ();

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
    public void moveBlock(int newPos)
    {
        Vector3 nextPosition = Board.spaces[this.currentPosition].getPosition();
        List<string> startingSpace = Board.spaces[this.currentPosition].getCamelOnSpaces();
        int camelStackPos = startingSpace.IndexOf(this.id + "_camel");


        List<string> blockToMove = startingSpace.GetRange(camelStackPos, startingSpace.Count - camelStackPos);
        Board.spaces[this.currentPosition].removeRange(camelStackPos, startingSpace.Count - camelStackPos);

        foreach (string s in blockToMove)
        {
            Board.addCamel(s, newPos);
        }

        Camel.stackCamels();
    }
	public void moveSpaces(int roll) {
		int newPos = currentPosition + roll;
        if (newPos > 15)
        {
            moveBlock(newPos % 16);
            GameController.endGame(newPos);
            return;
        }
        BoardSpace landingSpace = Board.spaces [newPos];
        if (landingSpace.getBonus() != 0)
        {
            int playerOwns = Board.bonusSquaresOwned[newPos];
            GameController.players[playerOwns].alterFunds(2);
            newPos += landingSpace.getBonus();
        }
       
		
		if (newPos > 15) {
			GameController.getResults (newPos);
            GameController.gameEndFlag = true;
            return;
		}
        moveBlock(newPos);
	}
	// Update is called once per frame
	void Update () {
		
	}
}
