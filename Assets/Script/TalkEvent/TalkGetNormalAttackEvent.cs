using UnityEngine;
using System.Collections;

public class TalkGetNormalAttackEvent: TalkEvent {

    void Start() {
        if (isDefined) {
            this.buttonText = "学习普通攻击";
            this.conText = "小伙子\n来享受战斗的乐趣吧！";
        }
    }

    override public void runEvent() {
        bool isCanStudy = true;
        FightEvent[] f = getNpc().GetComponents<FightEvent>();
        for (int i = 0; i < f.Length; i++) {
            if (f[i].id == 2) {//在这里写技能的id
                isCanStudy = false;
                break;
            }
        }
        if (isCanStudy) {
            getNpc().gameObject.AddComponent<FightNormalAttack>();
            this.setNotice("恭喜！\n学会了普通攻击", 1f, true);
        } else {
            this.setNotice("小伙子！\n你学过普通攻击吧...", 1f, true);
        }
        this.isCanShow = false;
        backMove();
    }
}
