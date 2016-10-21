using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextEvent : MonoBehaviour {
	float aliveTime;
	Text text;
	TextChangeColor textChange;
	// Use this for initialization
	void Start () {
		aliveTime = 0;
		text = this.transform.GetComponent<Text> ();
		textChange = this.transform.GetComponent<TextChangeColor> ();
	}

	public void setNotice(string str, float time,bool isChangeColor){
		text.text = str;
		textChange.isChange = isChangeColor;
		aliveTime = time;
		this.gameObject.SetActive(true);
	}
	public void setNotice(string str, float time,float r,float g, float b){
		text.text = str;
		textChange.isChange = false;
		textChange.setColor (new Color(r,g,b));
		aliveTime = time;
		this.gameObject.SetActive(true);
	}
	public void setNotice(string str, float time,Color col){
		text.text = str;
		textChange.isChange = false;
		textChange.setColor (col);
		aliveTime = time;
		this.gameObject.SetActive(true);
	}
	// Update is called once per frame
	void Update () {
		this.aliveTime -= Time.deltaTime;
		if (this.aliveTime < 0f) {
			this.gameObject.SetActive(false);
		}
	}
}
