using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	public static List<Player> players;
	public static int currentTurn;
	public UIController ui;
    public List<Camel> camels;
    public static Dictionary<string, BettingTilePile> bettingTilePile = new Dictionary<string, BettingTilePile>();
    public static bool gameEndFlag = false;
    public static bool diceSettled = false;
    public static List<string> previousPositions;
    public static Queue<BettingCard> winnerPile = new Queue<BettingCard>();
    public static Queue<BettingCard> loserPile = new Queue<BettingCard>();

    public static List<Player> getPlayerStandings()
    {
        //Using Selection sort as there is only going to be a constant array length
        List<Player> unsorted = new List<Player>(players);
        List<Player> sorted = new List<Player>();
        for (int i = 0; i < 4; i++)
        {
            int max = 0;
            Player maxPlayer = null;
            foreach (Player p in unsorted)
            {
                if (p.getFunds() >= max)
                {
                    max = p.getFunds();
                    maxPlayer = p;
                }
            }
            unsorted.Remove(maxPlayer);
            sorted.Add(maxPlayer);
        }
        return sorted;
    }
    public static Player getCurrentPlayer()
    {
        return players[currentTurn];
    } 
    public static void endGame(int newPos)
    {
        GameController.getResults(newPos);
        GameController.gameEndFlag = true;
        GameController.evalRoundBets();
        
    }
    public static string dealWithBets(Queue<BettingCard> pile)
    {
        Queue<int> cash = new Queue<int>(new[] { 8, 5, 3, 2, 1 });
        string firstPlace = getPositions()[0];
        string resultsText = "";
        foreach (BettingCard bc in pile)
        {
            int owner = Int32.Parse(bc.getOwner());
            if (firstPlace == bc.getCamel())
            {
                if (cash.Count == 1)
                {
                    resultsText += (players[owner].getName() + " wins 1 for " + firstPlace + "winning!\n");
                    players[owner].alterFunds(1);
                }
                else
                {
                    int winnings = cash.Peek();
                    players[owner].alterFunds(cash.Dequeue());
                    resultsText += (players[owner].getName() + " wins " + winnings + " for " + firstPlace + "winning!\n");
                }
            }
            else
            {
                resultsText += players[owner].getName() + " loses 1 for " + bc.getCamel() + " losing!\n";
                players[owner].alterFunds(-1);
            }
        }
        return resultsText;
       
    }
    public static void finalBets()
    {
                string resultsText = "";
        resultsText += dealWithBets(winnerPile);
        resultsText += dealWithBets(loserPile);

        GameObject gc = GameObject.FindGameObjectWithTag("GameController");
        UIController ui = gc.GetComponent<UIController>();
        if (resultsText == "")
        {
            resultsText = "No one bet this game!";
        }
        ui.UpdateWinnerPanel(resultsText);

    }
	public static List<string> getResults(int finalPos) {
		int count = 0;
		List<string> results = new List<string> ();
		while (count < 5) {
			List<string> finalStack = new List<string>(Board.spaces[finalPos % 16].getCamelOnSpaces());
            finalStack.Reverse ();
			results.AddRange (finalStack);
			count += finalStack.Count;
			finalPos--;
		}
		
		return results;
	}

	public static List<string> getPositions() {
		int count = 0;
		int finalPos = 15;
		List<string> results = new List<string> ();
		while (count < 5) {
            List<string> finalStack = new List<string>(Board.spaces[finalPos % 16].getCamelOnSpaces());     
            finalStack.Reverse();
            results.AddRange (finalStack);
			count += finalStack.Count;
			finalPos--;
		}
        
		return results;
	}

	public void setUpPlayers(int numOfPlayers) {
		players = new List<Player>();
		for (int i = 0; i < numOfPlayers; i++) {
			players.Add( new Player ("player" + i.ToString()));
		} 
	}
    public static string ToStringPositions()
    {
        string standings;
        standings = "";
        int count = 1;
        foreach (string p in getPositions())
        {
            standings += count + ": " + p + ", ";
            count++;
        }
        return standings;
    }
    public static  void evalRoundBets()
    {
        string resultsText = "";
        List<string> pos = getPositions();
        foreach (Player p in players)
        {
            foreach (BettingTile bt in p.getBettingTiles())
            {
               
                if (bt.getColor() == pos[0])
                {
                    resultsText += p.getName() + " gets " + bt.getValue() + " for " + pos[0] + " coming first\n";
                    p.alterFunds(3);
                }
                else if (bt.getColor() == pos[1])
                {
                    resultsText += p.getName() + " gets 1 for " + pos[1] + " coming second\n";
                    p.alterFunds(1);
                }
                else
                {
                    resultsText += p.getName() + " gets -1 for " + bt.getColor() + " not placing\n";
                    p.alterFunds(-1);
                }
            }
        }
        foreach (Player p in players)
        {
            p.clearTiles();
        }
        GameObject gc = GameObject.FindGameObjectWithTag("GameController");
        UIController ui = gc.GetComponent<UIController>();
        if (resultsText == "")
        {
            resultsText = "No one bet this round!";
        }
        
        ui.UpdateRoundPanel(resultsText);
    }
    public void nextTurn() {
        GameObject gameC = GameObject.FindGameObjectWithTag("GameController");
        UIController ui = (UIController)gameC.GetComponent(typeof(UIController));
     
        if (TrackPyramid.diceInPyramid.Count == 0)
        {
            evalRoundBets();
            TrackPyramid.resetPyramid();
        }
        GameController.currentTurn = (GameController.currentTurn + 1) % 4;
        ui.destroyAllChildren();
        ui.updateUI();
	}
    public void setUpTiles()
    {
        bettingTilePile.Add("green_camel", new BettingTilePile("green"));
        bettingTilePile.Add("yellow_camel", new BettingTilePile("yellow"));
        bettingTilePile.Add("white_camel", new BettingTilePile("white"));
        bettingTilePile.Add("orange_camel", new BettingTilePile("orange"));
        bettingTilePile.Add("blue_camel", new BettingTilePile("blue"));
    }
    public void setUpGame() {
		currentTurn = 0;
		setUpPlayers (4);
		Board.setUpBoard ();
        setUpTiles();

	}
    public bool checkPlacement(int space)
    {
        foreach (Camel c in camels)
        {
            if (c.getPos() == space)
            {
                return false;
            }
        }
        if (!Board.checkPlacement(space))
        {
            return false;
        }
        return true;
    }
    
    

    // Use this for initialization
    void Start () {
       
        setUpGame ();
		ui.updateUI ();

        


    }

}
