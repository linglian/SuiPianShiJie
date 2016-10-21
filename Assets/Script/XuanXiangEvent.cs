using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class XuanXiangEvent : MonoBehaviour {
	GameEvent gameEvent = null;
	void Start(){
		Button button = this.transform.GetComponent<Button> ();
		UnityAction ac = new UnityAction(this.OnClickListener);
		button.onClick.AddListener(ac);
	}
	public void OnClickListener(){
		if (gameEvent!=null) {
			gameEvent.runEvent ();
		}
	}
	public void setGameEvent(GameEvent gameEvent){
		this.gameEvent = gameEvent;
	}
}
