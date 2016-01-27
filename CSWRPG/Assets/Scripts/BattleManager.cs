using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

using UnityEngine.UI;

public class BattleManager : MonoBehaviour {

	public enum battleState 
	{
		start,
		player,
		opponent,
		lose,
		win
	}

	public battleState currentState;
	public List<GameObject> players;
	public List<GameObject> opponents;

	private GameObject canvas;
	private RectTransform bottomPanel;
	private RectTransform topPanel;
	private Text dialogText;

	private GameObject currentPlayer;
	private GameObject currentOpponent;

	private List<Weapon> weaponList = new List<Weapon>();
		


	void Start () {
		currentState =  battleState.start;

		weaponList.Add(new Weapon ("Axe of Apathy"));
		weaponList.Add(new Weapon ("Lance of Lethargy"));
		weaponList.Add(new Weapon ("Sword of Stoicism"));


		canvas = GameObject.Find ("Canvas");
		GameObject textObject = Instantiate (Resources.Load ("TextObject")) as GameObject;

		bottomPanel = GameObject.Find ("BottomPanel").GetComponent<RectTransform>();
		topPanel = GameObject.Find ("TopPanel").GetComponent<RectTransform>();

		textObject.transform.SetParent (canvas.transform, false);
		dialogText = textObject.GetComponent<Text> ();


	}

	private void createButton(string text, Action functionOnClick, RectTransform panel){
		GameObject button = Instantiate (Resources.Load ("PrefabButton")) as GameObject;
		button.transform.SetParent (panel.transform, false);
		button.GetComponentInChildren<Text> ().text = text;

		Button b = button.GetComponent<Button> ();
		b.onClick.AddListener (() => functionOnClick());
	}

	private void setDialogBoxText(string text){
		dialogText.text = text;
	}


	public void attackTarget(){

	}

	private void clearRectTransform(RectTransform panel){
		foreach (Transform child in panel.transform) {
			GameObject.Destroy(child.gameObject);
		}
	}

	private void createTargetSelectMenu(){
		foreach (GameObject gameObject in opponents) {
			createButton(gameObject.GetComponent<BattleComponent>().name, attackTarget,topPanel);
		}
	}

	private void useUsableOn(IUseable useable, GameObject target){
		//useable.execute (target.GetComponent<BattleComponent> ());
	}

	private void createUseableButtonList(List<IUseable> usableList){
		//foreach (IUseable use in usableList) {
			//createButton(use.getName(),attackTarget,topPanel);
		//}
	}

	public void attackTarget(GameObject target, Weapon weaponToAttack){
		//BattleComponent playerBattle = currentPlayer.GetComponent<BattleComponent> ();
		//BattleComponent opponentBattle = currentOpponent.GetComponent<BattleComponent> ();
		//playerBattle.getWeapon().attackOpponent(opponentBattle);
		//currentPlayer.GetComponent<BattleComponent> ().getWeapon ().attackOpponent (currentOpponent.GetComponent<BattleComponent>);
		//Debug.Log (player.name+" attacked "+enemy.name+" for "+player.getWeapon().getDamage()+" damage.");
		//currentOpponent.GetComponent<Animator>().SetTrigger("Damage");
		
		//clearRectTransform(bottomPanel);
	}

	public void useItemOnTarget(GameObject target){

	}



	// Update is called once per frame
	void Update () {
		switch (currentState){
			case (battleState.start):
				battleStart();
				break;
			case (battleState.player):
				playersTurn();
				break;
			case (battleState.opponent):
				opponentsTurn();
				break;
			case (battleState.lose):
				loseBattle();
				break;
			case (battleState.win):
				winBattle();
				break;
		}

		checkHP();

	}

	private void battleStart(){
		List<IUseable> list = weaponList.Cast<IUseable> ().ToList ();
		setDialogBoxText ("You encounter the enemy!");
		createButton("Attack!", attackTarget, bottomPanel);
		createButton ("Defend!", attackTarget, bottomPanel);
		createButton ("Items!", attackTarget, bottomPanel);
		createButton ("Run Away!", attackTarget, bottomPanel);

		currentPlayer = players [0];
		currentOpponent = opponents [0];

		Weapon playerWeapon = new Weapon ();
		playerWeapon.setDamage (50);
		currentPlayer.GetComponent<BattleComponent>().setWeapon(playerWeapon);
		currentState = battleState.player;
	}
	

	private void playersTurn(){

	}

	private void opponentsTurn(){
		currentState = battleState.player;

	}

	private void checkHP(){
	}

	public void endTurn(){
		if (currentState == battleState.player) {
			currentState = battleState.opponent;
		} else {
			currentState = battleState.player;
		}
	}
	

	public void loseBattle(){
		endBattle();
	}

	private void winBattle(){
		endBattle();
	}

	public void endBattle(){
		
	}
}
