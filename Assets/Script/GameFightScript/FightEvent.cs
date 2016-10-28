using UnityEngine;
using System.Collections;

public class FightEvent : GameEvent {
    protected MainGame mainGame;
    public string skillName;//技能名字
    public string skillLvl;//技能等级
    public string skillExp;//技能经验
    public string skillMaxExp;//技能最大经验
    public string skillText;//技能介绍


    /******************************
     *对继承类接口
     *调用其他类的功能请在这里调用
     *继承类不得自己调用
     ******************************/
    protected void normalAttack() {
        if (selectMainGame()) {
            mainGame.normalAttack();
        }
    }
    


    /******************************
     *不对外开放的函数
     *
     *
     ******************************/
    private bool selectMainGame() {
        if (mainGame == null) {
            mainGame = GameObject.Find("Game").GetComponent<MainGame>();
        }
        return true;
    }

}
