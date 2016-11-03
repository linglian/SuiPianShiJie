using UnityEngine;
using System.Collections;

public class FightEvent : GameEvent {
    protected MainGame mainGame;
    public int id;
    public string skillName;//技能名字
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
    protected void normalAttack() {
        if (selectMainGame()) {
            mainGame.normalAttack();
            this.addExp(1);
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

    /******************************
     *不对外开放的函数
     *
     *
     ******************************/

    private void upGrade() {
        this.skillLvl++;
        skillMaxExp = skillLvl * skillLvl * 5;
        skillHurtAmp += 1.5f;
        skillHurtAdd += 5f;
    }
    private bool selectMainGame() {
        if (mainGame == null) {
            mainGame = GameObject.Find("Game").GetComponent<MainGame>();
        }
        return true;
    }

}
