using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TalkEvent : GameEvent {
	void Start(){
		this.text = "交谈";
	}
	override public void runEvent(){
		setNotice ("小伙子，找我有什么事么", 2.5f, true);
	}
}
