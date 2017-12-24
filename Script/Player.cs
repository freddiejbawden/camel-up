using System.Collections;
using System.Collections.Generic;

namespace UnityEngine {
	public class Player {
		private string name; 
		private int funds;
		private bool placedTile;
		private List<string> bettingCards;
		private List<string> hands;
		public Player(string name) {
			this.name = name;
			this.funds = 3;
			this.placedTile = false;
			this.bettingCards =  new List<string>{"green_camel", "yellow_camel", "orange_camel", "white_camel", "blue_camel"};
			this.hands = new List<string> ();
		}
		public void alterFunds(int cash) {
			this.funds += cash;
		}
		public void placeTile() {
			this.placedTile = false;
		}
		public void playCard(string s) {
			this.bettingCards.Remove (s);
		}
		public string getName() {
			return this.name;
		}
		public int getFunds() {
			return this.funds;
		}

	}
}