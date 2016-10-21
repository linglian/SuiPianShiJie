using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
	private int moveMode = -1;
	public float moveSpeed  = 2.5f;
	public const int MOVEMODE_DOWN = 0;
	public const int MOVEMODE_UP = 1;
	public const int MOVEMODE_RIGHT = 2;
	public const int MOVEMODE_LEFT = 3;	
	public void setMoveMode(int mode){
		this.moveMode = mode;
	}
	public void stopMove(){
		this.moveMode = -1;
	}
	// Update is called once per frame
	void FixedUpdate () {
		switch(moveMode){
		case MOVEMODE_DOWN:
			transform.Translate (Vector3.down * moveSpeed * Time.fixedDeltaTime);
			break;
		case MOVEMODE_UP:
			transform.Translate (Vector3.up * moveSpeed * Time.fixedDeltaTime);
			break;
		case MOVEMODE_RIGHT:
			transform.Translate (Vector3.right * moveSpeed * Time.fixedDeltaTime);
			break;
		case MOVEMODE_LEFT :
			transform.Translate (Vector3.left * moveSpeed * Time.fixedDeltaTime);
			break;
		}
	}
}
