using UnityEngine;
using System.Collections;

public class FightEvent : GameEvent {
    public GameObject skillPerfabs;//技能实例
    protected MainGame mainGame;
    public int id;
    public string skillName;//技能名字
    [SerializeField, MultilineAttribute(5)]
    public string skillText;//技能介绍
    public int skillLvl;//技能等级
    public int skillMaxLvl;//技能等级
    public float skillExp;//技能经验
    public float skillMaxExp;//技能最大经验
    public float skillHurtAmp;//伤害系数
    public float skillHurtAdd;//伤害附加


    /******************************
     *对继承类接口
     *调用其他类的功能请在这里调用
     *继承类不得自己调用
     ******************************/

    protected virtual void attackNpc(GameNpc enemyNpc,float hurt) {
            enemyNpc.minusHp(hurt);
            this.addExp(1);
    }

    protected void upGrade() {
        this.skillLvl++;
        skillMaxExp = skillLvl * skillLvl * 5;
        skillHurtAmp += 1.5f;
        skillHurtAdd += 5f;
    }
    protected  bool selectMainGame() {
        if (mainGame == null) {
            mainGame = GameObject.Find("Game").GetComponent<MainGame>();
        }
        return true;
    }


    /******************************
     *对外开放的接口
     *
     *
     ******************************/

    public virtual void skillEvent() {

    }

    public override void runEvent() {
        GameNpc myNpc = this.gameObject.GetComponent<GameNpc>();
        if (myNpc.isCanFight) {
            string skillName;
            if (id < 10) {
                skillName = "skill_0" + id;
            } else {
                skillName = "skill_" + id;
            }
            GameObject obj = (GameObject)Instantiate(Resources.Load("Prefabs\\Skill\\" + skillName), Vector3.zero, Quaternion.identity);
            obj.transform.SetParent(this.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.GetComponent<AnimatorEvent>().startFight(myNpc, this.gameObject.GetComponent<GameNpc>().enemyNpc, this);
            if (selectMainGame()) {
                mainGame.getTextEvent().setNotice(this.conText, 0.5f, true);
            }
            myNpc.speedTime = 0;
        } else {
            if (selectMainGame()) {
                mainGame.getTextEvent().setNotice("现在还不能释放技能喔！", 0.5f, true);
            }
        }
    }
    public void addExp(int exp) {
        this.skillExp += exp;
        while (this.skillLvl < this.skillMaxLvl && this.skillExp >= skillMaxExp) {
            this.skillExp -= skillMaxExp;
            upGrade();
        }
        if (this.skillExp >= this.skillMaxExp * 10f) {
            this.skillExp = this.skillMaxExp * 10f;
        }
    }
}
