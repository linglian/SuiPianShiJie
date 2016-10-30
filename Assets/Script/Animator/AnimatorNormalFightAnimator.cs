using UnityEngine;
using System.Collections;

public class AnimatorNormalFightAnimator : AnimatorEvent {
    public GameObject fight;
    private Vector3 pos;
    private Vector3 myPos;
    private Vector3 nowPos;
    private int isFight = 0;//0为停，1为逼近，2为离开
    private float xSpeed;
    private float ySpeed;
    private float zSpeed;
    private float time = 0;
    private float maxTime = 0;

    /******************************
     *不对外开放的函数
     *
     *
     ******************************/
    void Start() {
        myPos = this.transform.position;
    }

    void FixedUpdate() {
        if (isFight!=0) {
            time += Time.fixedDeltaTime;
            nowPos = this.transform.position;
            switch (isFight) {
                case 1:
                    go();
                    break;
                case 2:
                    back();
                    break;
            }
            if (isFight==1&&time >= maxTime) {
                myNpc.normalAttack(enemyNpc);
                isFight = 2;
                time = 0;
            } else if (isFight == 2 && time >= maxTime) {
                isFight = 0;
            }
        }
    }

    private void go() {
        pos = fight.transform.position;
        xSpeed = (nowPos.x - pos.x) / (maxTime - time)*Time.fixedDeltaTime;
        ySpeed = (nowPos.y - pos.y) / (maxTime - time) * Time.fixedDeltaTime;
        zSpeed = (nowPos.z - pos.z) / (maxTime - time) * Time.fixedDeltaTime;
        nowPos.x -= xSpeed;
        nowPos.y -= ySpeed;
        nowPos.z -= zSpeed;
        this.transform.position = nowPos;
    }

    private void back() {
        pos = myPos;
        xSpeed = (nowPos.x - pos.x) / (maxTime - time) * Time.fixedDeltaTime;
        ySpeed = (nowPos.y - pos.y) / (maxTime - time) * Time.fixedDeltaTime;
        zSpeed = (nowPos.z - pos.z) / (maxTime - time) * Time.fixedDeltaTime;
        nowPos.x -= xSpeed;
        nowPos.y -= ySpeed;
        nowPos.z -= zSpeed;
        this.transform.position = nowPos;
    }
    /******************************
     *对外开放的接口
     *
     *
     ******************************/

    public override void runEvent() {
        if (myNpc != null) {
            this.isFight = 1;
            this.maxTime = myNpc.norFightSpeed * (1f + myNpc.fightSpeedAmp) / 2f;
            this.time = 0;
        }
    }
}
