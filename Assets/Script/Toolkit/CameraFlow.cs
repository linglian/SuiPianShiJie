using UnityEngine;
using System.Collections;

public class CameraFlow : MonoBehaviour {
	public GameObject flowObject;
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 pos = flowObject.transform.position;
		pos.z = -10f;
		this.transform.position = pos;
	}
}
