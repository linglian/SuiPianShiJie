using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
	//移动相关
	private int moveMode = -1;
	public float moveSpeed  = 2.5f;
	public const int MOVEMODE_DOWN = 0;
	public const int MOVEMODE_UP = 1;
	public const int MOVEMODE_RIGHT = 2;
	public const int MOVEMODE_LEFT = 3;	

	/************************
	 *接口
	 *
	 *
	 ************************/

	//设置移动方向
	public void setMoveMode(int mode){
		this.moveMode = mode;
	}

	//停止移动
	public void stopMove(){
		this.moveMode = -1;
	}

	/************************
	 *不对外开放的函数
	 *
	 *
	 ************************/

	//更新函数
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
