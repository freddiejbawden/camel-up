using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPyramid : MonoBehaviour {
    public static List<GameObject> diceInPyramid = new List<GameObject>();
    public static List<GameObject> constDice;
    

	// Use this for initialization
	
	public static void rollDice() {
		
			int r = Random.Range (0, diceInPyramid.Count);
			GameObject d = diceInPyramid [r];
			diceInPyramid.RemoveAt (r);
            DiceLauncher.createDice (d);
			
	}
     public static void resetPyramid() {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Dice")) {
            Destroy(g);
        }
        diceInPyramid = new List<GameObject>();
        for (int i = 0; i < 5; i++)
        {
            diceInPyramid.Add(constDice[i]);
        }
	}
    void Start()
    {
        constDice = Resources.LoadAll("Prefabs/Dice").OfType<GameObject>().ToList();
        
        diceInPyramid = new List<GameObject>(constDice);
    }
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("space"))
			rollDice ();
		if (Input.GetKeyDown ("y"))
			resetPyramid ();
	}
    
}
