using UnityEngine;
using System.Collections;

public class GameEvent : MonoBehaviour {
    public bool isDefined = true;
    public string buttonText;//显示内容
    public string conText;//文本内容

    /******************************
     *对外开放的接口
     *
     *
     ******************************/

    public virtual void runEvent() {

    }

}
