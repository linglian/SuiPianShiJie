using UnityEngine;
using System.Collections;

public class CameraFlow : MonoBehaviour {
	public GameObject flowObject;
    public float warpX;
    public float warpY;
    public float warpZ;
	
	// Update is called once per frame
	void FixedUpdate () {
        if (flowObject == null) {
            Destroy(this);
        }
        Vector3 pos = flowObject.transform.position;
        pos.x += warpX;
        pos.y += warpY;
        pos.z += warpZ;
		this.transform.position = pos;
	}
}
