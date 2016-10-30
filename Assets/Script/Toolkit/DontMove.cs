using UnityEngine;
using System.Collections;

public class DontMove : MonoBehaviour {

	void OnCollisionEnter2D ( Collision2D other) {
		this.transform.GetComponent<Move> ().stopMove();
		this.transform.GetComponent<Animator> ().CrossFade("Wait",0f);;
	}
}
