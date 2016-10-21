using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainGame : MonoBehaviour {
	public GameObject floor;
	public GameObject XuanXiang;
	public int Width = 50;
	public int Height = 50;
	Queue buttonQueue;
	// Use this for initialization
	void Start () {
		buttonQueue = new Queue ();
		GameObject obj;
		Vector3 pos = Vector3.zero;
		for (int i = -Width/2; i < Width/2; i++) {
			for (int j = -Height/2; j < Height/2; j++) {
				pos.x = i*1.6f;
				pos.y = j*1.6f;
				obj = (GameObject)Instantiate (floor, pos, Quaternion.identity);
				obj.transform.SetParent (this.transform.Find ("GameFloor").transform);
			}
		}
	}
	public void DestroyButton(){
		while (buttonQueue.Count != 0) {
			Destroy ((GameObject)buttonQueue.Dequeue ());
		}
	}
	public void setButton(GameEvent []gameEvent){
		GameObject obj;
		Vector3 pos;
		XuanXiangEvent xxEvent;
		DestroyButton ();
		for (int i = 0; i < gameEvent.Length; i++) {
			pos = Vector3.zero;
			obj = (GameObject)Instantiate (XuanXiang, pos, Quaternion.identity);
			obj.transform.SetParent (this.transform.Find ("GameGraphics").transform.Find ("UICanvas").transform.Find ("TextFrame").transform);
			RectTransform rectRra = obj.GetComponent<RectTransform> ();
			pos = Vector3.zero;
			pos.x = 0f;
			pos.y = 100f-i*50;
			pos.z = 0f;
			rectRra.anchoredPosition3D = pos;
			rectRra.localScale = Vector3.one;	
			Text text = obj.GetComponentInChildren<Text> ();
			text.text = (i+1)+","+gameEvent[i].text;
			xxEvent = obj.GetComponent<XuanXiangEvent> ();
			xxEvent.setGameEvent (gameEvent [i]);
			buttonQueue.Enqueue (obj);
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
