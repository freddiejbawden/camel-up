using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
	public Button moveCamelButton;

	public Text playerDetails;
	public GameController gameCont;
	public void moveCamel() {
		bool wasAbleToRoll = TrackPyramid.rollDice ();
		if (wasAbleToRoll) {
			GameController.players [GameController.currentTurn].alterFunds (1);
		}
		updateText ();
	}
	public void setupListeners() {
		Button moveCamelBTN = moveCamelButton.GetComponent<Button> ();
		moveCamelBTN.onClick.AddListener (moveCamel);
	}

	public void updateText() {
		int playerNum = GameController.currentTurn;
		Debug.Log (GameController.players [playerNum].getName());
		Player currentPlayer = GameController.players [playerNum];
		string name = currentPlayer.getName ();
		int funds = currentPlayer.getFunds ();
		playerDetails.text = "Player Name: " + name + "\nFunds: " + funds;
	}
	void Start() {
		setupListeners ();
	}
}
