using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

[System.Serializable]
public class PlayerData : MonoBehaviour {
    static BattleComponent player1 = new BattleComponent();
    private static List<BattleComponent> party = new List<BattleComponent>();

    public static void init(){

    }

    public static BattleComponent getPlayer()
    {
        player1.setStats(1, 2, 3, 4, 5);
        player1.setName("Jimjams");
        player1.weapons.Add(new Weapon("Test Wepon"));
        return player1;
    }

    public static List<BattleComponent> getParty(){
        return party;
    }

    public static void addPlayer(){

    }

}
