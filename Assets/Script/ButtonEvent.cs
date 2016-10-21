using UnityEngine;
using System.Collections;

public class ButtonEvent : MonoBehaviour {
	private GameEvent[] gameEvent;
	MainGame mainGame;
	// Use this for initialization
	void Start () {
		gameEvent = this.GetComponents<GameEvent> ();
		mainGame = GameObject.Find ("Game").GetComponent<MainGame> ();
	}

	void OnCollisionEnter2D (Collision2D other) {
		mainGame.setButton (gameEvent);
	}
}
