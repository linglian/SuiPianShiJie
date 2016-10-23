using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TalkEvent : GameEvent {
	void Start(){
		this.buttonText = "交谈";
		this.conText = "小伙子\n找我有什么事么！";
	}
	override public void runEvent(){
		//setNotice ("小伙子，找我有什么事么", 2.5f, true);
		setText(conText,this.gameObject);
	}
}
