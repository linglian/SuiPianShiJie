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
        getNpc().gameObject.AddComponent<FightNormalAttack>();
        this.isCanShow = false;
        this.setNotice("恭喜！\n学会了普通攻击", 1f, true);
        backMove();
    }
}
