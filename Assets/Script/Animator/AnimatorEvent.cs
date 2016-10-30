using UnityEngine;
using System.Collections;

public class AnimatorEvent : MonoBehaviour {

    protected GameNpc myNpc;
    protected GameNpc enemyNpc;

    public void startFight(GameNpc myNpc, GameNpc enemyNpc) {
        this.myNpc = myNpc;
        this.enemyNpc = enemyNpc;
    }

    public virtual void runEvent() {
        
    }
}
