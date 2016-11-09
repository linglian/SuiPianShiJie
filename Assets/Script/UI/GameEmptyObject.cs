using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameEmptyObject : MonoBehaviour {

    public string ObjectName;//名字
    [SerializeField, MultilineAttribute(5)]
    public string introduce;//简介

    void OnCollisionEnter2D(Collision2D other) {
        GameObject.Find("Game").GetComponent<MainGame>().setNowObject(this);
        collisionEvent();
    }
    void OnCollisionExit2D(Collision2D other) {
        GameObject.Find("Game").GetComponent<MainGame>().closeNowObject();
    }

    /******************************
     *对外开放的接口
     *
     *
     ******************************/
    public virtual void collisionEvent() {

    }
}