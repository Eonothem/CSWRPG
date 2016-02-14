using UnityEngine;
using System.Collections;

public class BattleLoader: MonoBehaviour{
    static void loadBattle()
    {
        TurnBasedBattleManager.addOpponent("TestSphere");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        loadBattle();
        //Debug.Log("EnteredBattle");
    }
}
