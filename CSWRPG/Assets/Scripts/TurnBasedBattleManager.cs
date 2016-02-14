using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

using UnityEngine.UI;

public class TurnBasedBattleManager : MonoBehaviour {

	public enum battleState 
	{
		start,
		player,
		opponent,
		lose,
		win,
        flee
	}

	public enum typeOfAction
	{
		items,
		attack,
		none
	}
	
	public battleState currentState;
	public static List<GameObject> players = new List<GameObject>();
	public static List<GameObject> opponents = new List<GameObject>();
    public static List<string> opponentsToLoad = new List<string>();

	private GameObject canvas;
	private RectTransform bottomPanel;
	private RectTransform topPanel;
	private Text dialogText;

	private GameObject selectedPlayer;
	private GameObject selectedTarget;
	private IUseable selectedAction;
	private typeOfAction actionType;

	private List<Weapon> weaponList = new List<Weapon>();
    


	// Use this for initialization
	void Start () {

        /*
         * What we gon do
         * 
         * On start, get the player data from the PlayerData class
         * Create objects and give the battle components to those objects
         * play some gosh darn vidya games
         * */
        //PlayerData.getPlayer();
        initPlayers();
        initEnemies();

		currentState =  battleState.start;

		canvas = GameObject.Find ("Canvas");
		
		bottomPanel = GameObject.Find ("BottomPanel").GetComponent<RectTransform>();
		topPanel = GameObject.Find ("TopPanel").GetComponent<RectTransform>();

	}

    private void initEnemies()
    {
        foreach(string s in opponentsToLoad){
            GameObject g = Instantiate(Resources.Load("Enemies/"+s)) as GameObject;
            opponents.Add(g);
        }
    }

    private void initPlayers(){
        GameObject lel = new GameObject();

        //GameObject top = Instantiate(Resources.Load("Enemies/TestSphere")) as GameObject;
        //GameObject afdaf = Resources.Load("Enemies/TestSphere") as GameObject;
        
        BattleComponent kek = lel.AddComponent<BattleComponent>();
        kek.paste(PlayerData.getPlayer());
        //opponents.Add(top);
        players.Add(lel); 
    }

	private Button createButton(string text, RectTransform panel){
		GameObject button = Instantiate (Resources.Load ("PrefabButton")) as GameObject;
		button.transform.SetParent (panel.transform, false);
		button.GetComponentInChildren<Text> ().text = text;
		
		Button b = button.GetComponent<Button> ();
		return b;
	}

	private void clearRectTransform(RectTransform panel){
		foreach (Transform child in panel.transform) {
			GameObject.Destroy(child.gameObject);
		}
	}

	//========================================
	//=--------------------------------------=
	//========================================

	/*
	 * listUserButtons()
	 * ----------
	 * When a player selects attack or flee, this shows the dialog of characters that the user can attack with.
	 * Also this determies if the user is using an item or an attack, and sets up a new list accordingly.
	 * 
	 * 
	 * */
	public void listUserButtons(List<GameObject> members){
		foreach (GameObject member in members) {
			Button selectPlayerButton = createButton(member.GetComponent<BattleComponent>().name, topPanel);

			GameObject tempObject = member;
			selectPlayerButton.onClick.AddListener( () => setSelectedPlayer(tempObject));

			if(member.GetComponent<BattleComponent>().attacked){
				selectPlayerButton.interactable = false;
			}

			List<IUseable> usableList = new List<IUseable>();
			switch(actionType){
				case(typeOfAction.attack):
					usableList = member.GetComponent<BattleComponent>().weapons.Cast<IUseable>().ToList ();
					break;
				case(typeOfAction.items):
					break;
			}
			
			selectPlayerButton.onClick.AddListener( () => clearRectTransform(topPanel));

			selectPlayerButton.onClick.AddListener( () => listUsableButtons(usableList));

		}
	}

	/*
	 * listUseableButtons()
	 * ----------
	 * Displays a button list containing useables
	 * */
	private void listUsableButtons(List<IUseable> usables){
		foreach (IUseable usable in usables) {
			Button selectUseableButton = createButton(usable.getName(), topPanel);

			selectUseableButton.onClick.AddListener( () => clearRectTransform(topPanel));
			selectUseableButton.onClick.AddListener( () => setSelectedAction(usable));
			selectUseableButton.onClick.AddListener( () => listTargetButtons(opponents));
		}
	}

	/*
	 * listTargetButtons()
	 * --------
	 * Creates/Displays a button list of the opponents
	 * */
	private void listTargetButtons(List<GameObject> targets){
		foreach (GameObject target in targets) {
			Button selectTargetButton = createButton(target.GetComponent<BattleComponent>().name, topPanel);

			if(target.GetComponent<BattleComponent>().dead){
				selectTargetButton.interactable = false;
			}

			GameObject tempTarget = target;

			selectTargetButton.onClick.AddListener( () => clearRectTransform(topPanel));
			selectTargetButton.onClick.AddListener( () => setSelectedTarget(tempTarget));
			selectTargetButton.onClick.AddListener( () => executeActionOnTarget());
		}
	}

	

	private void setSelectedPlayer(GameObject origin){
		Debug.Log ("Attacker: " + origin.GetComponent<BattleComponent> ().name);
		selectedPlayer = origin;
	}

	private void setSelectedTarget(GameObject target){
		Debug.Log ("Target: " + target.GetComponent<BattleComponent> ().name);
		selectedTarget = target;
	}

	private void setSelectedAction(IUseable usable){
		Debug.Log ("Action: " + usable.getName());
		selectedAction = usable;
	}

	private void executeActionOnTarget(){
		selectedPlayer.GetComponent<BattleComponent> ().attacked = true;
		selectedAction.execute(selectedPlayer.GetComponent<BattleComponent>(), selectedTarget.GetComponent<BattleComponent> ());

        if (actionType == typeOfAction.attack)
        {
            if (currentState == battleState.player)
            {
               selectedTarget.GetComponent<Animator>().SetTrigger("Damage");
            }
        }

		setBotButtons(true);
	}

	private void selectActionMode(typeOfAction action){
		actionType = action;
	}


	Button attackButton;
	Button itemButton;
	// Update is called once per frame

	/*
	 * Control Flow
	 * ------------
	 * 1) List action options
	 * 2) List all of the players who can use that option
	 * 3) List the individual options within that action type
	 * 4) List the opponents on which you can use the action on
	 * 5) Commence actoin on target
	 * 6) Revert to step one until win/lose condition
	 * 
	 * */
	void Update () {

		if (checkIfAllDead (opponents)) {
			currentState = battleState.win;
		} else if (checkIfAllDead (players)) {
			currentState = battleState.lose;
		}

		switch (currentState) {

		case (battleState.start):
			battleStart();
			break;
		case (battleState.player):

            //Check if all the players have attacked
			bool allAttacked = false;
			foreach(GameObject player in players){
				allAttacked = player.GetComponent<BattleComponent>().attacked;
				if(allAttacked == false){
					break;
				}
			}

            //if they all attacked, switch over to opponent's turn
            //if they didn't, just keep going until all of the players have attacked
			if(allAttacked){

				foreach(GameObject player in players){
					player.GetComponent<BattleComponent>().attacked = false;
				}

				currentState = battleState.opponent;
				setBotButtons(false);
			}
			break;
		case (battleState.opponent):
			opponentTurn();
			break;
		case (battleState.lose):
            endBattle();
			break;
		case (battleState.win):
            endBattle();
			break;
        case (battleState.flee):
            endBattle();
            break;
		}
	}

    private void endBattle()
    {
        switch (currentState)
        {
            case (battleState.lose):
                break;
            case (battleState.win):
                Debug.Log("U WON!");
                break;
            case (battleState.flee):
                endBattle();
                break;
        }
        foreach(GameObject g in opponents){
            Destroy(g);
        }
        opponentsToLoad.Clear();
        opponents.Clear();
    }

	private void opponentTurn(){
		Debug.Log ("Enemy took turn");
		currentState = battleState.player;
		setBotButtons (true);
	}

	private bool checkIfAllDead(List<GameObject> list){
		bool allDead = false;
		foreach (GameObject member in list) {
			allDead = member.GetComponent<BattleComponent>().dead;
			if(allDead == false){
				break;
			}
		}
		return allDead;
	}

	private void disableButton(Button b){
		b.interactable = false;
	}

	private void enableButton(Button b){
		b.interactable = true;
	}

	private void setBotButtons(bool val){
		attackButton.interactable = val;
		itemButton.interactable = val;
	}

	private void battleStart(){
		attackButton = createButton ("Attck", bottomPanel);
		attackButton.onClick.AddListener (() => selectActionMode(typeOfAction.attack));
		attackButton.onClick.AddListener (() => listUserButtons(players));

		attackButton.onClick.AddListener (() => setBotButtons(false));

		itemButton = createButton("ITM", bottomPanel);

		currentState = battleState.player;
	}

    public static void addOpponent(List<string> o)
    {
       opponentsToLoad.AddRange(o);
    }

    public static void addOpponent(string o)
    {
       opponentsToLoad.Add(o);
    }

}
