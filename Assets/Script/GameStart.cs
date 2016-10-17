using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {
	Canvas gameCanvas;
	// Use this for initialization
	void Start () {
		gameCanvas = this.transform.Find ("GameCanvas").GetComponent<Canvas> ();
		gameCanvas.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
