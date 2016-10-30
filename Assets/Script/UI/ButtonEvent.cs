using UnityEngine;
using System.Collections;

public class ButtonEvent : GameEmptyObject {
	private GameEvent[] gameEvent;
	MainGame mainGame;
	// Use this for initialization
	void Start () {
		gameEvent = this.GetComponents<TalkEvent> ();
		mainGame = GameObject.Find ("Game").GetComponent<MainGame> ();
	}

    public override void CollisionEvent() {
        mainGame.setButton(gameEvent);
    }
}
