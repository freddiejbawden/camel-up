using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnityEngine
{
    public class BettingCard
    {
        private string owner;
        private string camel;
        public BettingCard(string camel, string owner)
        {
            this.camel = camel;
            this.owner = owner;
        }
        public string getOwner()
        {
            return this.owner;
        }
        public string getCamel()
        {
            return this.camel;
        }
    }
    public class PlayerHand {
         private string owner;
         private Dictionary<string,BettingCard> hand;
         public PlayerHand(string owner)
        {
            this.owner = owner;
            this.hand = new Dictionary<string,BettingCard>();
            this.hand.Add("yellow_camel", new BettingCard("yellow_camel", owner));
            this.hand.Add("green_camel", new BettingCard("green_camel", owner));
            this.hand.Add("white_camel", new BettingCard("white_camel", owner));
            this.hand.Add("orange_camel", new BettingCard("orange_camel", owner));
            this.hand.Add("blue_camel", new BettingCard("blue_camel", owner));
        }
        public Dictionary<string, BettingCard> getHand()
        {
            return this.hand;
        }
}
    
}

