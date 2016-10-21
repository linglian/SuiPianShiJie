using UnityEngine;
using System.Collections;

public class NpcMove : MonoBehaviour {
	public GameObject npc;
	private Animator ani;
	private Move move;
	private float dur = 0.0f;
	// Use this for initialization
	void Start () {
		ani = npc.transform.GetComponent<Animator> ();
		move =npc.transform.GetComponent<Move> ();
	}

	public void moveNpc(int mode){
		switch(mode){
		case Move.MOVEMODE_DOWN:
			ani.CrossFade("Down",dur);
			move.setMoveMode (Move.MOVEMODE_DOWN);
			break;
		case Move.MOVEMODE_UP:
			ani.CrossFade("Up",dur);
			move.setMoveMode (Move.MOVEMODE_UP);
			break;
		case Move.MOVEMODE_LEFT:
			ani.CrossFade("Left",dur);
			move.setMoveMode (Move.MOVEMODE_LEFT);
			break;
		case Move.MOVEMODE_RIGHT:
			ani.CrossFade("Right",dur);
			move.setMoveMode (Move.MOVEMODE_RIGHT);
			break;
		}
	}
	// Update is called once per frame
	void Update () {
		if (Input.anyKey) {
			if(Input.GetKey(KeyCode.W)){
				moveNpc(Move.MOVEMODE_UP);
			}if(Input.GetKey(KeyCode.S)){
				moveNpc(Move.MOVEMODE_DOWN);
			} else if(Input.GetKey(KeyCode.D)){
				moveNpc(Move.MOVEMODE_RIGHT);
			} else if(Input.GetKey(KeyCode.A)){
				moveNpc(Move.MOVEMODE_LEFT);
			}
		}
	}
}
