using UnityEngine;
using System.Collections;

public class AutoDie : MonoBehaviour {
	public float time;
	private float nowTime;
	// Update is called once per frame
	void FixedUpdate () {
		nowTime += Time.fixedDeltaTime;
		if (nowTime >= time) {
			Destroy (this.gameObject);
		}
	}
}
