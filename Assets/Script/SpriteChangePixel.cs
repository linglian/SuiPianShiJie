using UnityEngine;
using System.Collections;

public class SpriteChangePixel : MonoBehaviour {
	public GameObject ca;
	public float width;
	public float height;
	// Use this for initialization
	void Start () {
		Camera cam = ca.GetComponent<Camera>();
		float ort = cam.orthographicSize;
		float wh = (float)Screen.width / (float)Screen.height;
		float normalWidth = ort * 2 * (width / height);
		float newHeight = normalWidth / wh;
		float ooW = newHeight / 2.0f / ort;
		Vector3 pos = transform.lossyScale;
		pos.x /= ooW;
		transform.localScale = pos;
	}
}