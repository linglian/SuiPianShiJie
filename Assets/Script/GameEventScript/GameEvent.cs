using UnityEngine;
using System.Collections;

public class GameEvent : MonoBehaviour {
	public string text;
	protected TextEvent notice;
	public virtual void runEvent(){
		
	}
	protected void setNotice(string str,float time,bool isChangeColor){
		if (notice == null) {
			notice = GameObject.Find ("Game").transform.Find ("GameGraphics").transform.Find ("NoticeCanvas").transform.Find ("Notice").transform.GetComponent<TextEvent> ();
		}
		notice.setNotice (str,time,isChangeColor);
	}
	protected void setNotice(string str, float time,Color col){
		if (notice == null) {
			notice = GameObject.Find ("Game").transform.Find ("GameGraphics").transform.Find ("NoticeCanvas").transform.Find ("Notice").transform.GetComponent<TextEvent> ();
		}
		notice.setNotice (str,time,col);
	}
	protected void setNotice(string str, float time,float r,float g, float b){
		if (notice == null) {
			notice = GameObject.Find ("Game").transform.Find ("GameGraphics").transform.Find ("NoticeCanvas").transform.Find ("Notice").transform.GetComponent<TextEvent> ();
		}
		notice.setNotice (str,time,r,g,b);
	}
}
