using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextChangeColor : MonoBehaviour {
	Text text;
	public bool isChange = true;
	public float minColor = 1;
	public float maxColor = 254;
	float addColorR;
	float addColorG;
	float addColorB;
	Color color;
	// Use this for initialization
	void Start () {
		text = this.transform.GetComponent<Text> ();
		if (text.color.r >= maxColor) {
			addColorR = -0.01f;
		} else {
			addColorR = 0.01f;
		}
		if (text.color.g >= maxColor) {
			addColorG = -0.02f;
		} else {
			addColorG = 0.02f;
		}
		if (text.color.b >= maxColor) {
			addColorB = -0.03f;
		} else {
			addColorB = 0.03f;
		}

		maxColor /= 255f;
		minColor /= 255f;
		color = text.color;
	}
	public void setColor(Color col){
		text.color = col;
	}
	// Update is called once per frame
	void FixedUpdate () {
		if (isChange) {
			color.r += addColorR;
			color.g += addColorG;
			color.b += addColorB;
			text.color = color;
			if (text.color.r >= maxColor || text.color.r <= minColor) {
				addColorR = -addColorR;
			}
			if (text.color.g >= maxColor || text.color.g <= minColor) {
				addColorG = -addColorG;
			}
			if (text.color.b >= maxColor || text.color.b <= minColor) {
				addColorB = -addColorB;
			}
		}
	}
}
