using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
   public class BettingTile
    {
        private string color;
        private int value;
        public BettingTile(string color, int value)
        {
            this.color = color +"_camel";
            this.value = value;
        }
        public string getColor()
        {
            return this.color;
        }
        public int getValue()
        {
            return this.value;
        }

    }
    public class BettingTilePile
    {   
        private Stack<BettingTile> pile;
        public BettingTilePile(string color)
        {
            this.pile = new Stack<BettingTile>();
            this.pile.Push(new BettingTile(color, 1));
            this.pile.Push(new BettingTile(color, 3));
            this.pile.Push(new BettingTile(color, 5));
        }
        public BettingTile PopCard()
        {
            
             return this.pile.Pop(); 
        }
        public bool isEmpty()
        {
            if (this.pile.Count == 0)
            {
                return true;
            } else
            {
                return false;
            }
        }
        public string ToString()
        {
            string toReturn;
            toReturn = this.pile.Peek().getColor() +": ";
            if (this.pile.Count == 0)
            {
                return toReturn + "None";
            }
            foreach (BettingTile b in this.pile)
             {
                toReturn = toReturn + b.getValue() + ", ";
             }
            return toReturn;
        }
    }
}