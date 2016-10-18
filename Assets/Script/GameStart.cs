using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameStart : MonoBehaviour {
	Canvas gameCanvas;
	Camera gameCamera;
	Text text;
	// Use this for initialization
	void Start () {
		gameCanvas = this.transform.Find ("GameGraphics").transform.Find("GameCanvas").GetComponent<Canvas> ();
		gameCanvas.gameObject.SetActive (false);
		gameCamera = this.transform.Find ("GameCamera").GetComponent<Camera> ();
		gameCamera.gameObject.SetActive (false);
		text = this.transform.Find ("GameGraphics").transform.Find ("NoticeCanvas").transform.Find("Notice").GetComponent<Text> ();
		text.gameObject.SetActive (true);
		text.text = "任意键开始";
	}
	void gameStart(){
		gameCanvas.gameObject.SetActive (true);
		gameCamera.gameObject.SetActive (true);
		text.gameObject.SetActive (false);
	}
	// Update is called once per frame
	void Update () {
		if (Input.anyKey) {
			gameStart ();
		}
	}
}
