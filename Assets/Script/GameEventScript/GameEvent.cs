using UnityEngine;
using System.Collections;

public class GameEvent : MonoBehaviour {
    public string buttonText;//显示内容
    public string conText;//文本内容
    protected TextEvent notice;
    protected MainGame mainGame;

    /*******************
     *对外接口-此项唯一
     *
     *
     *******************/
    public virtual void runEvent() {

    }

    /*******************
     *对继承类接口-调用其他类的功能请在这里调用，继承类不得自己调用
     *
     *
     *******************/

    //开始战斗
    protected void startFight() {
        if (selectMainGame()) {
            mainGame.startFight(this.GetComponentInParent<GameNpc>());
        }
    }

    //发送文本，在物体顶部显示()
    protected void setText(string str, GameObject gameObj) {
        if (selectMainGame()) {
            mainGame.setText(str, gameObj);
        }
    }

    //发送公告，显示在屏幕中间偏下方
    protected void setNotice(string str, float time, bool isChangeColor) {
        if (selectNotice()) {
            notice.setNotice(str, time, isChangeColor);
        }
    }

    protected void setNotice(string str, float time, Color col) {
        if (selectNotice()) {
            notice.setNotice(str, time, col);
        }
    }
    protected void setNotice(string str, float time, float r, float g, float b) {
        if (selectNotice()) {
            notice.setNotice(str, time, r, g, b);
        }
    }

    /*******************
     *不对外开放的函数
     *
     *
     *******************/

    private bool selectMainGame() {
        if (mainGame == null) {
            mainGame = GameObject.Find("Game").GetComponent<MainGame>();
        }
        return true;
    }

    private bool selectNotice() {
        if (notice == null) {
            notice = GameObject.Find("Game").transform.Find("GameGraphics").transform.Find("NoticeCanvas").transform.Find("Notice").transform.GetComponent<TextEvent>();
        }
        return true;
    }
}
