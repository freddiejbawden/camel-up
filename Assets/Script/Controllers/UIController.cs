using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour {
    public List<Button> tileAddButtons;
	public Button moveCamelButton;
	public Button placeTileButton;
    public Button takeBettingTileButton;
    public Button placeBettingCard;
	public Text playerDetails;
	public GameController gameCont;
    public int selectedSpace = 0;
    public List<Transform> positions;
    public GameObject bettingCardButtons;
    public GameObject winLose;
    public bool cardSelected;
    public GameObject winnerPanel;
    public GameObject roundPanel;
    public GameObject tilePanel;
    public GameObject playerTiles;
    public GameObject tile;
    public GameObject placeTileErrorPanel;
    public GameObject bettingPileEmptyPanel;
    private bool closed;
    public Dictionary<string, Color> colors = new Dictionary<string, Color>();
    
    public void createDictionaryColors()
    {
        colors.Add("white_camel", Color.white);
        colors.Add("green_camel", Color.green);
        colors.Add("blue_camel", Color.blue);
        colors.Add("yellow_camel", Color.yellow);
        colors.Add("orange_camel", new Color(1f, 0.7176471f,0f,1f));
    }
    public void createTile(int value, Color c)
    {
        GameObject tileCopy = Instantiate(tile);
        Transform tcTrans = tileCopy.GetComponent<Transform>();
        tcTrans.SetParent(playerTiles.transform);
        tcTrans.GetComponent<Image>().color = c;
        tcTrans.GetChild(0).GetComponent<Text>().text = value.ToString();
    }
    public void destroyAllChildren()
    {
        foreach (Transform child in playerTiles.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    public void displayPlayerTiles()
    {
        Player p = GameController.getCurrentPlayer();
        foreach (BettingTile bt in p.getBettingTiles())
        {
            Color c = colors[bt.getColor()];
            createTile(bt.getValue(), c);
        }
    }
    public void updateTile(int y, GameObject go)
    { 
        Player curPlayer = GameController.getCurrentPlayer();
        if (curPlayer.checkPlacedTile()) 
        {
            Board.bonusSquaresOwned.Remove(curPlayer.getTilePosition());
            GameObject previousTile = GameObject.Find("space" + curPlayer.getTilePosition());
            previousTile.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        }
        curPlayer.setPlacedTile(y);
        Board.spaces[y].setSpaceBonus(selectedSpace);
        Board.bonusSquaresOwned.Add(y, GameController.currentTurn);

        Renderer rend = go.GetComponent<Renderer>();
        if (selectedSpace == 1)
        {
            
            rend.material.SetColor("_Color", Color.green);
        }
        else
        {
          
            rend.material.SetColor("_Color", Color.red);
        }
        selectedSpace = 0;
        toggleTileButtons();
        gameCont.nextTurn();
    }
    public IEnumerator waitForTileSelect(int y, GameObject go)
    {
        while(selectedSpace == 0)
        {
            yield return null;
        }
        updateTile(y, go);
        yield return null;
        
    }
   
    public void closeWarning()
    {
        placeTileErrorPanel.active = false;
    }
    public void handlePlaceTile(GameObject go)
    {
        togglePlayerTiles();
        string name = go.name;
        string x;
        if (name.Length == 7)
        {
            x = name.Substring(5, 2);
        }
        else
        {
            x = name.Substring(5, 1);
        }
        int y = Int32.Parse(x);
        if (!gameCont.checkPlacement(y))
        {
            placeTileErrorPanel.active = true;
            return;
        }
        toggleTileButtons();
        StartCoroutine(waitForTileSelect(y,go));
        
    }
    public IEnumerator waitForTile()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    if (hit.transform.gameObject.tag == "Space")
                    {
                       handlePlaceTile(hit.transform.gameObject);
                       yield break;
                        
                    }
                }


            }
            yield return null;
        }

    }
    public void toggleBettingCardButtons()
    {
        bettingCardButtons.active = !bettingCardButtons.active;
        if (bettingCardButtons.active)
        {
            foreach (Transform t in bettingCardButtons.transform)
            {
                Player p = GameController.getCurrentPlayer();
                string colorCamel = t.name.Substring(0, t.name.Length - 7);
                bool containsColor = p.getPlayerHand().ContainsKey(colorCamel);
                if (!containsColor)
                {
                    t.GetComponent<Image>().color = Color.black;
                    t.GetComponent<Button>().interactable = false;
                }
            }
        }
       
    }
    public void toggleWinLoseButtons()
    {
        winLose.active = !winLose.active;
    }
    public void toggleTileButtons()
    {
        bool cVal = tileAddButtons[0].gameObject.active;
        tileAddButtons[0].gameObject.active = !cVal;
        tileAddButtons[1].gameObject.active = !cVal;
    }
    public void selectedAdd()
    {
        selectedSpace = 1;
    }
    public void selectedSub()
    {
        selectedSpace = -1;
    }
    public IEnumerator WaitUntilDiceSettled()
    {
        while (!GameController.diceSettled)
        {
            yield return null;
        }
        GameController.diceSettled = false;
        updateUIAfterMove();
    }
    public void updateUIAfterMove()
    {
        updateUI();

        if (!GameController.gameEndFlag)
        {
            updatePositions();
        }
       

        GameController.previousPositions = GameController.getPositions();

        gameCont.nextTurn();
    }
    public void resetPlayerUI()
    {

        bettingCardButtons.active = false;
        winLose.active = false;
        tileAddButtons[0].gameObject.active = false;
        tileAddButtons[1].gameObject.active = false;
        playerTiles.active = true;

    }
    public void moveCamel() {
        resetPlayerUI();
        TrackPyramid.rollDice ();
        GameController.players [GameController.currentTurn].alterFunds (1);
        StartCoroutine(WaitUntilDiceSettled());
    }
    public void setPositions()
    {
        List<string> standings = GameController.getPositions();
        foreach (Transform t in positions)
        {
            string camelForm = t.name.Substring(0, t.name.Length - 6) + "_camel";
            int ind = standings.IndexOf(camelForm);
            
                RectTransform rt = t.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector3(-425, 360 - 25 * ind, 0);
                t.GetChild(0).GetComponent<Text>().text = (ind + 1).ToString(); 
        }
    }
    public void updatePositions()
    {
        List<string> standings = GameController.getPositions();
        
      
        foreach (Transform t in positions)
        {
            string camelForm = t.name.Substring(0, t.name.Length - 6) + "_camel";
            int ind = standings.IndexOf(camelForm);
            if (standings[ind] != GameController.previousPositions[ind])
            {
                RectTransform rt = t.GetComponent<RectTransform>(); 
                StartCoroutine(LerpObj(rt, rt.anchoredPosition, new Vector3(-425, 360 - 25 * ind, 0)));
                StartCoroutine(FadeOut(t.GetChild(0).GetComponent<Text>(), (ind + 1).ToString()));
            }
        }
    }
    public IEnumerator FadeIn(Text i, string set)
    {
        float t = .15f;
        i.text = set;
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }
    public IEnumerator FadeOut(Text i, string set)
    {
        float t = .15f;
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
        StartCoroutine(FadeIn(i, set));
    }
    public IEnumerator LerpObj(RectTransform rectTransform, Vector3 startPosition, Vector3 endPosition)
    {
        float currentTime = 0f;
        float timeOfTravel = .5f;
        float normVal;
        while (currentTime <= timeOfTravel)
        {
            currentTime += Time.deltaTime;
            normVal = currentTime / timeOfTravel;
            normVal = normVal * normVal * (3f - 2f * normVal);
            rectTransform.anchoredPosition = Vector3.Lerp(startPosition, endPosition, normVal);
            yield return null;
        }

    }
    public void placeTile() {
        resetPlayerUI();
        StartCoroutine(waitForTile());
	}

    public void takeBettingTile()
    {
        resetPlayerUI();
        StartCoroutine(waitForTileChoose());
    }
    public IEnumerator waitForTileChoose()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {  
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
                    if (hit.transform.gameObject.tag == "BettingTilePile")
                    {
                        handleTakeTile(hit.transform.gameObject);
                        yield break;
                    }
                }
            }
            yield return null;
        }
    }
    public void closeBettingTileEmptyPanel()
    {
        bettingPileEmptyPanel.active = false;
    }
    public void handleTakeTile(GameObject go)
    {
        string keyName = go.name.Remove(go.name.Length - 5);
        if (GameController.bettingTilePile[keyName].isEmpty())
        {
            bettingPileEmptyPanel.active = true;
            return;
        }
        BettingTile b = GameController.bettingTilePile[keyName].PopCard();
        GameController.getCurrentPlayer().addTile(b);
        ToggleTileTakePanel();
        Text tileText = tilePanel.transform.GetChild(0).GetComponent<Text>();
        tileText.text = "Took " + b.getColor() + " tile worth " + b.getValue();
        StartCoroutine(waitForBettingTileClose());
    }
    public IEnumerator waitForBettingTileClose()
    {
        while(!closed)
        {
            yield return null;
        }
        ToggleTileTakePanel();
        closed = false;
        togglePlayerTiles();
        gameCont.nextTurn();
    }
    public void ToggleTileTakePanel()
    {
        tilePanel.active = !tilePanel.active;
    }
    public void handlePlaceCard()
    {
        toggleBettingCardButtons();
        togglePlayerTiles();
        StartCoroutine(selectCard());
    }
    public IEnumerator selectCard()
    {
        while(!cardSelected)
        {
            yield return null;
        }
       
    }
   



    public void chooseCardColor()
    {
        cardSelected = true;
        string name = EventSystem.current.currentSelectedGameObject.name;
        string currentPlayerNum = GameController.currentTurn.ToString();
        string shortName = name.Substring(0, name.Length - 7);
        
        GameController.getCurrentPlayer().removeBettingCard(shortName);
        toggleBettingCardButtons();
        cardSelected = false;
        toggleWinLoseButtons();
        StartCoroutine(selectCard());
    }

    public void addToPileWinnerLoser()
    {
        cardSelected = true;
        string name = EventSystem.current.currentSelectedGameObject.name;
        string currentPlayer = GameController.currentTurn.ToString();
        if (name == "winner")
        {
            GameController.winnerPile.Enqueue(new BettingCard(name, currentPlayer));
        }
        else
        {
            GameController.loserPile.Enqueue(new BettingCard(name, currentPlayer));
        }
        cardSelected = false;
        toggleWinLoseButtons();
        gameCont.nextTurn();
    }

    public void setupListeners() {
       

        Button moveCamelBTN = moveCamelButton.GetComponent<Button> ();
		moveCamelBTN.onClick.AddListener (moveCamel);
		Button placeTileBTN = placeTileButton.GetComponent<Button> ();
		placeTileBTN.onClick.AddListener (placeTile);
        tileAddButtons[0].GetComponent<Button>().onClick.AddListener(selectedAdd);
        tileAddButtons[1].GetComponent<Button>().onClick.AddListener(selectedSub);
        takeBettingTileButton.GetComponent<Button>().onClick.AddListener(takeBettingTile);
        placeBettingCard.GetComponent<Button>().onClick.AddListener(handlePlaceCard);
        Transform betCont = GameObject.FindGameObjectWithTag("BettingCardButtons").transform;
        roundPanel.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(closePopUp);
        tilePanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(closePopUp);
        placeTileErrorPanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(closeWarning);
        bettingPileEmptyPanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(closeBettingTileEmptyPanel);

        foreach (Transform t in betCont)
        {
            t.GetComponent<Button>().onClick.AddListener(chooseCardColor);
        }
        Transform wlCont = GameObject.FindGameObjectWithTag("WinnerLoser").transform;
        foreach (Transform t in wlCont)
        {
            t.GetComponent<Button>().onClick.AddListener(addToPileWinnerLoser);
        }

    }
   
	public void updateUI() {
		int playerNum = GameController.currentTurn;
		Player currentPlayer = GameController.players [playerNum];
		string name = currentPlayer.getName ();
		int funds = currentPlayer.getFunds ();
		playerDetails.text = "Player Name: " + name + "\nFunds: " + funds;
        displayPlayerTiles();
        playerTiles.active = true;
        

    }
    public void ToggleWinnerPanel()
    {
        winnerPanel.active = !winnerPanel.active;
    }
    public void ToggleRoundPanel()
    {
        roundPanel.active = !roundPanel.active;
    }
    public void UpdateWinnerPanel(string results)
    {
        ToggleWinnerPanel();
        Text winningInfo = winnerPanel.transform.GetChild(1).GetComponent<Text>();
        Text bettingInfo = winnerPanel.transform.GetChild(2).GetComponent<Text>();
        string winners = (GameController.getPositions())[0] + " came 1st!\n";
        winningInfo.text = winners;
        bettingInfo.text = results;
        string playerStandingsText = "";
        Text playerStandings = winnerPanel.transform.GetChild(3).GetComponent<Text>();
        List<Player> standings = GameController.getPlayerStandings();
        int count = 1;
        int previousWins = standings[0].getFunds();
        foreach (Player p in standings)
        {
            if (previousWins != p.getFunds())
            {
                count++;
            }
            playerStandingsText += count + ". " + p.getName() + ": " + p.getFunds() + "EP\n";
            previousWins = p.getFunds();
            
        }
        playerStandings.text = playerStandingsText;

    }
    public void UpdateRoundPanel(string results)
    {
        ToggleRoundPanel();
        Text winningInfo = roundPanel.transform.GetChild(1).GetComponent<Text>();
        Text bettingInfo = roundPanel.transform.GetChild(2).GetComponent<Text>();
        string winners = (GameController.getPositions())[0] + " came 1st!\n";
        winners += (GameController.getPositions())[1] + " came 2nd!\n";
        winningInfo.text = winners;
        bettingInfo.text = results;
        StartCoroutine(waitForRoundClose());
    }
    public IEnumerator waitForRoundClose()
    {
        while (!closed)
        {
            yield return null;
        }
        closed = false;
        ToggleRoundPanel();
        if (GameController.gameEndFlag == true)
        {
            GameController.finalBets();
        } else
        {
            gameCont.nextTurn();
        }
        
    }
    
    public void closePopUp()
    {
        closed = true;
    }
    public void togglePlayerTiles()
    {
        playerTiles.active = !playerTiles.active;
    }
    void Start() {
        setupListeners ();
        toggleBettingCardButtons();
        toggleTileButtons();
        toggleWinLoseButtons();
        ToggleRoundPanel();
        ToggleWinnerPanel();
        ToggleTileTakePanel();
        createDictionaryColors();
        closeBettingTileEmptyPanel();
        placeTileErrorPanel.active = false;

        GameObject container = GameObject.FindGameObjectWithTag("Positions");
        positions = new List<Transform>();
        foreach (Transform t in container.transform)
        {
            positions.Add(t);
        }
        GameController.previousPositions = GameController.getPositions();
        setPositions();
        resetPlayerUI();
    }
}
