using UnityEngine;
using System.Collections;

public class SpriteChangePixel : MonoBehaviour {
	public float width = 1080;
	public float height = 1920;
	// Use this for initialization
	void Start () {
		Camera cam = GameObject.Find("GameCamera").GetComponent<Camera>();
		float ort = cam.orthographicSize;
		float wh = (float)Screen.width / (float)Screen.height;
		float normalWidth = ort * 2 * (width / height);
		float newHeight = normalWidth / wh;
		float ooW = newHeight / 2.0f / ort;
        RectTransform rectRra =this.GetComponent<RectTransform>();
        if (rectRra != null) {
            Vector3 pos = rectRra.localScale;
            pos.x /= ooW;
            rectRra.localScale = pos;
        } else {
            Vector3 pos = transform.localScale;
            pos.x /= ooW;
            transform.localScale = pos;
        }
		Destroy (this);
	}
}