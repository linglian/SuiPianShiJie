using UnityEngine;
using System.Collections;

public class AnimatorEvent : MonoBehaviour {

    public Sprite[] sprite;
    public int isFight = 0;//0为停，1为逼近，2为离开,3为停留
    public int nowStep;
    public float posX;//偏差
    public float posY;//偏差
    public float posZ;//偏差
    public float step;
    public float time = 0;

    protected GameNpc myNpc;
    protected GameNpc enemyNpc;
    protected FightEvent fightEvent;
    protected float maxTime;

    public void startFight(GameNpc myNpc, GameNpc enemyNpc, FightEvent fightEvent) {
        this.myNpc = myNpc;
        this.enemyNpc = enemyNpc;
        this.fightEvent = fightEvent;
        runEvent();
    }

    public void startFight(GameNpc myNpc, GameNpc enemyNpc, FightEvent fightEvent,float maxTime) {
        this.myNpc = myNpc;
        this.enemyNpc = enemyNpc;
        this.fightEvent = fightEvent;
        this.maxTime = maxTime;
        runEvent();
    }

    public void startFight(GameNpc myNpc, GameNpc enemyNpc,  float maxTime) {
        this.myNpc = myNpc;
        this.enemyNpc = enemyNpc;
        this.fightEvent = null;
        this.maxTime = maxTime;
        runEvent();
    }
    public void startFight(GameNpc myNpc, GameNpc enemyNpc) {
        this.myNpc = myNpc;
        this.enemyNpc = enemyNpc;
        this.fightEvent = null;
        this.maxTime = 0;
        runEvent();
    }
    public void setFightEvent(FightEvent fightEvent) {
        this.fightEvent = fightEvent;
    }
    public virtual void runEvent() {

    }
}