using UnityEngine;
using System.Collections;

public class FightNormalAttack : FightEvent {

	void Start () {
        if (isDefined) {
            this.buttonText = "普通攻击";
            this.conText = "很普通的一击";
        }
	}

    override public void runEvent() {
        this.normalAttack();
    }
}
