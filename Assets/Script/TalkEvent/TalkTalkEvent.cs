using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TalkTalkEvent : TalkEvent {
	void Start(){
        if (isDefined) {
            this.buttonText = "交谈";
            this.conText = "小伙子\n找我有什么事么！";
        }
	}
	override public void runEvent(){
		setText(conText,this.gameObject);
	}
}
