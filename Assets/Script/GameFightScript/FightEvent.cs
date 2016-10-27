using UnityEngine;
using System.Collections;

public class FightEvent : GameEvent {
    protected GameNpc enemyNpc;
    protected GameNpc myNpc;


    /******************************
     *对继承类接口
     *调用其他类的功能请在这里调用
     *继承类不得自己调用
     ******************************/
    protected void normalAttack() {
        if (selectEnemyNpc() && selectMyNpc()) {
            if (myNpc.isCanFight) {
                myNpc.isCanFight = false;
                myNpc.speedTime = 0;
                myNpc.normalAttack(enemyNpc);
            }
        }
    }
    


    /******************************
     *不对外开放的函数
     *
     *
     ******************************/

    private bool selectEnemyNpc() {
        if (this.enemyNpc==null) {
            this.enemyNpc = GameObject.Find("Game").GetComponent<MainGame>().getEnemyNpc();
        }
        return true;
    }

    private bool selectMyNpc() {
        if (this.myNpc == null) {
            this.myNpc = GameObject.Find("Game").GetComponent<MainGame>().getMyNpc();
        }
        return true;
    }
}
