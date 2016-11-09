using UnityEngine;
using System.Collections;

public class AnimatorPhy : AnimatorEvent {
    private Vector3 pos;
    private Vector3 pos2;
    private Vector3 myPos;
    private Vector3 nowPos;
    private Animator ani;
    private float xSpeed;
    private float ySpeed;
    private float zSpeed;

    /******************************
     *不对外开放的函数
     *
     *
     ******************************/
    void Start() {

    }

    void FixedUpdate() {
        if (isFight != 0) {
            if (enemyNpc == null || myNpc.mode!=MainGame.MODE_FIGHT||enemyNpc.mode != MainGame.MODE_FIGHT) {
                Destroy(this.gameObject);
            }
            time += Time.fixedDeltaTime;
            nowPos = myNpc.transform.localPosition;
            switch (isFight) {
                case 1:
                    go();
                    break;
                case 2:
                    back();
                    break;
                case 3:
                    pos2 = Vector3.zero;
                    pos2.x += posX;
                    pos2.y += posY;
                    pos2.z += posZ;
                    this.transform.localPosition = pos2;
                    if (time - maxTime / 2 >= step * (nowStep % (sprite.Length / 2)+ 1)) {
                        nowStep++;
                        if (nowStep < sprite.Length) {
                            this.GetComponent<SpriteRenderer>().sprite = sprite[nowStep];
                        }
                    }
                    break;
            }
            if (isFight == 1 && time >= maxTime / 2) {
                if (fightEvent != null)
                    fightEvent.skillEvent();
                if (enemyNpc.hp <= 0) {
                    isFight = 0;
                    Destroy(this.gameObject);
                }
                isFight = 3;
                if (myNpc.transform.localPosition.x > enemyNpc.transform.localPosition.x) {
                    this.posX = -this.posX;
                    nowStep = sprite.Length/2;
                    if (ani != null)
                        ani.CrossFade("Left", 0f);
                } else {
                    nowStep = 0;
                    if (ani != null)
                        ani.CrossFade("Right", 0f);
                }
                step = maxTime / 8f / (sprite.Length / 2f);
            } else if (isFight == 3 && time >= maxTime / 1.6f) {
                this.GetComponent<SpriteRenderer>().sprite = null;
                isFight = 2;
            } else if (isFight == 2 && time >= maxTime) {
                if (ani != null)
                    ani.CrossFade("Wait", 0f);
                Destroy(this.gameObject);
                isFight = 0;
            }
        }
    }

    private void go() {
        pos = enemyNpc.transform.localPosition;
        if (pos.x > myNpc.transform.localPosition.x) {
            pos.x -= 2f;
            if (ani != null)
                ani.CrossFade("Right", 0f);
        } else {
            if (ani != null)
                ani.CrossFade("Left", 0f);
            pos.x += 2f;
        }
        xSpeed = (nowPos.x - pos.x) / (maxTime / 2f - time) * Time.fixedDeltaTime;
        ySpeed = (nowPos.y - pos.y) / (maxTime / 2f - time) * Time.fixedDeltaTime;
        zSpeed = (nowPos.z - pos.z) / (maxTime / 2f - time) * Time.fixedDeltaTime;
        nowPos.x -= xSpeed;
        nowPos.y -= ySpeed;
        nowPos.z -= zSpeed;
        myNpc.transform.localPosition = nowPos;
    }

    private void back() {
        pos = myPos;
        xSpeed = (nowPos.x - pos.x) / (maxTime - time) * Time.fixedDeltaTime;
        ySpeed = (nowPos.y - pos.y) / (maxTime - time) * Time.fixedDeltaTime;
        zSpeed = (nowPos.z - pos.z) / (maxTime - time) * Time.fixedDeltaTime;
        nowPos.x -= xSpeed;
        nowPos.y -= ySpeed;
        nowPos.z -= zSpeed;
        if (xSpeed < 0) {
            if (ani != null)
                ani.CrossFade("Right", 0f);
        } else if (xSpeed > 0) {
            if (ani != null)
                ani.CrossFade("Left", 0f);
        }
        myNpc.transform.localPosition = nowPos;
    }
    /******************************
     *对外开放的接口
     *
     *
     ******************************/

    public override void runEvent() {
        if (myNpc != null) {
            myPos = myNpc.transform.localPosition;
            this.isFight = 1;
            if(this.maxTime==0)
            this.maxTime = myNpc.norFightSpeed * (1f + myNpc.fightSpeedAmp);
            this.time = 0;
            this.GetComponent<SpriteRenderer>().sprite = null;
            ani = myNpc.transform.GetComponent<Animator>();
        }
    }
}