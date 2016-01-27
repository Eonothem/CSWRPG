using UnityEngine;
using System.Collections;
using System.Collections;
using System.Collections.Generic;

public class BattleComponent : MonoBehaviour {

	public byte DEF;
	public byte ATK;
	public byte S_DEF;
	public byte S_ATK;
	public int maxHP;
	public int currentHP;
	public List<Weapon> weapons;
	public string name;
	public bool attacked;
	public bool dead = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void takeDamage(int damage){
		currentHP -= damage;

		if (currentHP <= 0) {
			dead = true;
		}
	}

	public void heal(){

	}

	public void applyStatusEffect(){

	}

	public void removeStatusEffect(){
	
	}

	public void setWeapon(Weapon w){
		//weapon = w;
	}

	//public Weapon getWeapon(){
		//return weapon;
	//}


}
