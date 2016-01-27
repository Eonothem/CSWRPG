using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour, IUseable {
	private int damage = 0;
	private string name = "test wepon";
	// Use this for initialization

	public Weapon(string name){
		this.name = name;
	}

	public Weapon(){
		this.name = "Unnamed Weapon";
	}
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void attackOpponent(BattleComponent opponent){
		opponent.takeDamage(damage);
	}

	void addStatusEffect(){

	}

	public void setDamage(int d){
		damage = d;
	}

	public int getDamage(){
		return damage;
	}

	public void execute(BattleComponent origin, BattleComponent target){
		Debug.Log (origin.name +" attacked "+target.name+" with "+this.name);
		target.takeDamage (40);
		//Debug.Log ("Attacked" target.+ with "+name);
	}

	public string getName(){
		return this.name;
	}
}
