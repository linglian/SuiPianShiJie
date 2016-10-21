using UnityEngine;
using System.Collections;

public class FightEvent : GameEvent {
	
	void Start(){
		this.text = "战斗";
	}

	override public void runEvent(){
		setNotice ("小伙子，我可不是你的敌人哦！", 2.5f,Color.red);
	}
}
