using UnityEngine;
using System.Collections;

public class FightEvent : GameEvent {
	
	void Start(){
		this.buttonText = "战斗";
		this.conText = "小伙子\n我可不是你的敌人哦！";
	}

	override public void runEvent(){
		//setNotice ("小伙子，我可不是你的敌人哦！", 2.5f,Color.red);
		setText(conText,this.gameObject);
	}
}
