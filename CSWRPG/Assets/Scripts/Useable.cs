using UnityEngine;
using System.Collections;

interface IUseable{
	void execute(BattleComponent origin, BattleComponent target);
	string getName();
}
