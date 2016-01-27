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
		win
	}

	public enum typeOfAction
	{
		items,
		attack,
		none
	}
	
	public battleState currentState;
	public List<GameObject> players;
	public List<GameObject> opponents;

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

		weaponList.Add(new Weapon ("Axe of Apathy"));
		weaponList.Add(new Weapon ("Lance of Lethargy"));
		weaponList.Add(new Weapon ("Sword of Stoicism"));

		players [0].GetComponent<BattleComponent> ().weapons = weaponList;
		players [1].GetComponent<BattleComponent> ().weapons = weaponList;
		//players [0].GetComponent<BattleComponent> ().attacked = true;

		currentState =  battleState.start;

		canvas = GameObject.Find ("Canvas");
		//GameObject textObject = Instantiate (Resources.Load ("TextObject")) as GameObject;
		
		bottomPanel = GameObject.Find ("BottomPanel").GetComponent<RectTransform>();
		topPanel = GameObject.Find ("TopPanel").GetComponent<RectTransform>();
		
		//textObject.transform.SetParent (canvas.transform, false);
		//dialogText = textObject.GetComponent<Text> ();
	}

	private Button createButton(string text, RectTransform panel){
		GameObject button = Instantiate (Resources.Load ("PrefabButton")) as GameObject;
		button.transform.SetParent (panel.transform, false);
		button.GetComponentInChildren<Text> ().text = text;
		
		Button b = button.GetComponent<Button> ();
		return b;
		//b.onClick.AddListener (() => functionOnClick());
	}

	private void clearRectTransform(RectTransform panel){
		foreach (Transform child in panel.transform) {
			GameObject.Destroy(child.gameObject);
		}
	}

	//========================================
	//=--------------------------------------=
	//========================================

	public void listUserButtons(List<GameObject> members){
		foreach (GameObject member in members) {
			Button selectPlayerButton = createButton(member.GetComponent<BattleComponent>().name, topPanel);

			GameObject tempObject = member;
			selectPlayerButton.onClick.AddListener( () => setSelectedPlayer(tempObject));
			//selectPlayerButton.onClick.AddListener( () => weird(players[count]));
			//selectPlayerButton.onClick.AddListener( () => Debug.Log (tempObject.name));

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

	private void listUsableButtons(List<IUseable> usables){
		foreach (IUseable usable in usables) {
			Button selectUseableButton = createButton(usable.getName(), topPanel);

			selectUseableButton.onClick.AddListener( () => clearRectTransform(topPanel));
			selectUseableButton.onClick.AddListener( () => setSelectedAction(usable));
			selectUseableButton.onClick.AddListener( () => listTargetButtons(opponents));
		}
	}

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

		setBotButtons (true);
	}

	private void selectActionMode(typeOfAction action){
		actionType = action;
	}


	Button attackButton;
	Button itemButton;
	// Update is called once per frame
	void Update () {
		//Debug.Log (checkIfAllDead(opponents));
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
			bool allAttacked = false;
			foreach(GameObject player in players){
				allAttacked = player.GetComponent<BattleComponent>().attacked;
				if(allAttacked == false){
					break;
				}
			}

			if(allAttacked){

				foreach(GameObject player in players){
					player.GetComponent<BattleComponent>().attacked = false;
				}

				currentState = battleState.opponent;
				setBotButtons(false);
			}
			//playersTurn();
			break;
		case (battleState.opponent):
			opponentTurn();
			//Debug.Log ("OPPONONET");
			//opponentsTurn();
			break;
		case (battleState.lose):
			//loseBattle();
			break;
		case (battleState.win):
			//winBattle();
			//Debug.Log("U WON!");
			break;
		}
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

		//attackButton.onClick.AddListener (() => disableButton (attackButton));
		attackButton.onClick.AddListener (() => setBotButtons(false));

		itemButton = createButton("ITM", bottomPanel);

		currentState = battleState.player;
	}

}
