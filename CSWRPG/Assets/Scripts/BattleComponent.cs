using UnityEngine;
using System.Collections;
using System.Collections;
using System.Collections.Generic;

[System.Serializable] 
public class BattleComponent : MonoBehaviour {

	public byte DEF;
	public byte ATK;
	public byte S_DEF;
	public byte S_ATK;
	public int maxHP;
	public int currentHP;
	public List<Weapon> weapons = new List<Weapon>();
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

    public void setStats(byte ATK, byte DEF, byte S_DEF, byte S_ATK, int maxHP)
    {
        this.DEF = DEF;
        this.ATK = ATK;
        this.S_DEF = S_DEF;
        this.S_ATK = S_ATK;
        this.maxHP = maxHP;
    }

    public void paste(BattleComponent c)
    {
        setStats(c.ATK,c.DEF,c.S_DEF,c.S_ATK,c.maxHP);
        this.currentHP = c.currentHP;
        this.weapons = c.weapons;
        this.name = c.name;
        this.attacked = c.attacked;
        this.dead = c.dead;
    }

    public void setName(string name)
    {
        this.name = name;
    }

	public void setWeapon(Weapon w){
		//weapon = w;
	}

	//public Weapon getWeapon(){
		//return weapon;
	//}


}
