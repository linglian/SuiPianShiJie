using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GoBack : MonoBehaviour {
	TextEvent text;
	void Start(){
		text = GameObject.Find ("Game").transform.Find ("GameGraphics").transform.Find ("NoticeCanvas").transform.Find ("Notice").transform.GetComponent<TextEvent> ();
	}
	void OnTriggerEnter2D(Collider2D other){
		other.transform.position = Vector3.zero;
		text.setNotice ("上帝将你传回了原点",2.5f,true);
	}
}
