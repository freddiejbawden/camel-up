using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	public static List<Player> players;
	public static int currentTurn;
	public UIController ui;
	public static List<string> getResults(int finalPos) {
		int count = 0;
		List<string> results = new List<string> ();
		while (count < 5) {
			List<string> finalStack = Spaces.spaces [finalPos % 16].getCamelOnSpaces ();
			finalStack.Reverse ();
			results.AddRange (finalStack);
			count += finalStack.Count;
			finalPos--;
		}
		Debug.Log ("Winner: "+ results [0]);
		return results;
	}

	public static List<string> getPositions() {
		int count = 0;
		int finalPos = 15;
		List<string> results = new List<string> ();
		while (count < 5) {
			List<string> finalStack = Spaces.spaces [finalPos % 16].getCamelOnSpaces ();
			finalStack.Reverse ();
			results.AddRange (finalStack);
			count += finalStack.Count;
			finalPos--;
		}
		Debug.Log ("Round: "+ results [0]);
		return results;
	}

	public void setUpPlayers(int numOfPlayers) {
		players = new List<Player>();
		for (int i = 0; i < numOfPlayers; i++) {
			players.Add( new Player ("player" + i.ToString()));
		} 
	}
	public static void nextTurn() {
		Debug.Log ("nextTurn called");
		GameController.currentTurn += (GameController.currentTurn + 1) % 4;
		Debug.Log ("CT: " + GameController.currentTurn);
		GameObject gameC = GameObject.Find ("GameController");
		UIController ui = (UIController) gameC.GetComponent (typeof(UIController));
		ui.updateText ();
	}
	public void setUpGame() {
		currentTurn = 0;
		setUpPlayers (4);
		Spaces.setUpBoard ();

	}

	// Use this for initialization
	void Start () {
		setUpGame ();
		ui.updateText ();

	}

}
