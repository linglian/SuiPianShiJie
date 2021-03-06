﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class XuanXiangEvent : MonoBehaviour {
	GameEvent gameEvent = null;
	void Start(){
		Button button = this.GetComponent<Button> ();
		UnityAction ac = new UnityAction(this.OnClickListener);
		button.onClick.AddListener(ac);
	}

	public void OnClickListener(){
            if (gameEvent != null) {
                gameEvent.runEvent();
                GameObject.Find("Game").GetComponent<MainGame>().keyDownButton(this.gameObject);
            }
	}

	public void setGameEvent(GameEvent gameEvent){
		this.gameEvent = gameEvent;
	}
		
	public GameEvent getGameEvent(){
		return gameEvent;
	}
}
