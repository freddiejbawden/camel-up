using System.Collections;
using System.Collections.Generic;

namespace UnityEngine {
	public class Player {
		private string name; 
		private int funds;
		private bool placedTile;
        private int tilePosition;
		private PlayerHand playerHand;
        private List<BettingTile> bettingTiles;
		private List<string> hands;

        public Player(string name) {
			this.name = name;
			this.funds = 3;
			this.placedTile = false;
            this.playerHand = new PlayerHand(name);
			this.hands = new List<string> ();
            this.bettingTiles = new List<BettingTile>();
		}
		public void alterFunds(int cash) {
			this.funds += cash;
		}
		public bool checkPlacedTile() {
			return this.placedTile;
		}
        public void setPlacedTile(int s)
        {
           if (!placedTile)
            {
                placedTile = true;
            }
            this.tilePosition = s;
           
        }
        public int getTilePosition()
        {
            return this.tilePosition;
        }
        public void placeTile() {
			this.placedTile = true;
		}
		public void removeBettingCard(string color)
        {
           
            this.playerHand.getHand().Remove(color);
        }
		public string getName() {
			return this.name;
		}
		public int getFunds() {
			return this.funds;
		}
        public void addTile(BettingTile bt)
        {
            this.bettingTiles.Add(bt);
        }
        public Dictionary<string, BettingCard> getPlayerHand()
        {
            return this.playerHand.getHand();
        }
        public List<BettingTile> getBettingTiles()
        {
            return this.bettingTiles;
        }
        public void clearTiles()
        {
            this.bettingTiles = new List<BettingTile>();
        }


	}
}