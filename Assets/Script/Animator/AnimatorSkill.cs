using UnityEngine;
using System.Collections;

public class AnimatorSkill : AnimatorEvent {

    /******************************
     *不对外开放的函数
     *
     *
     ******************************/
    void Start() {
        nowStep = 0;
    }

    void FixedUpdate() {
        if (isFight != 0) {
            if (enemyNpc == null || enemyNpc.mode != MainGame.MODE_FIGHT) {
                Destroy(this.gameObject);
            }
            Vector3 pos = enemyNpc.transform.position;
            pos.x += posX;
            pos.y += posY;
            pos.z += posZ;
            this.transform.position = pos;
            time += Time.fixedDeltaTime;
            if (time >= step * (nowStep + 1)) {
                nowStep++;
                if (nowStep < sprite.Length) {
                    this.GetComponent<SpriteRenderer>().sprite = sprite[nowStep];
                }
            }
            if (isFight ==1&&time >= maxTime / 2) {
                if (fightEvent != null){
                    fightEvent.skillEvent();
                }
                isFight = 2;
            }
            if (time >= maxTime) {
                isFight = 0;
                Destroy(this.gameObject);
            }
        }
    }

    /******************************
     *对外开放的接口
     *
     *
     ******************************/

    public override void runEvent() {
        if (myNpc != null) {
            this.transform.position = enemyNpc.transform.position;
            this.isFight = 1;
            if (this.maxTime == 0)
                this.maxTime = myNpc.norFightSpeed * (1f + myNpc.fightSpeedAmp) / 2f;
            this.time = 0;
            this.GetComponent<SpriteRenderer>().sprite = sprite[nowStep];
            step = maxTime / (float)sprite.Length;
        }
    }
}