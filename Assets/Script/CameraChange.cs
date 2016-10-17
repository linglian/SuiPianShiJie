using UnityEngine;
using System.Collections;

public class CameraChange : MonoBehaviour {
	public GameObject ca;
	// Use this for initialization
	void Start () {
		Camera cam = ca.GetComponent<Camera>();
		float ort = cam.orthographicSize;
		float wh = (float)Screen.width / (float)Screen.height;
		float normalWidth = ort * 2 * (640f / 960f);
		float newHeight = normalWidth / wh;
		cam.orthographicSize = newHeight/2.0f;
		Debug.Log ("cam.orthographicSize = " +cam.orthographicSize);

	}
}