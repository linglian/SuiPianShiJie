using UnityEngine;
using System.Collections;

public class FightNormalAttack : FightEvent {
	void Start () {
        if (isDefined) {
            this.id = 2;
            this.buttonText = "普通攻击";
            this.conText = "很普通的一击";
        }
	}

    override public void skillEvent() {
        GameNpc myNpc = this.gameObject.GetComponent<GameNpc>();
        GameNpc enemyNpc = myNpc.enemyNpc;
        this.attackNpc(enemyNpc, GameNpc.getPhysicsRealAttack(myNpc, enemyNpc));
    }
}
