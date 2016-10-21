using UnityEngine;
using System.Collections;

public class GameNpc : MonoBehaviour {
	public float maxHp = 10;
	public float maxMp = 2;
	public GameObject statusHp;
	GameObject obj;
	Vector3 hpVector;
	float hp;
	float mp;
	// Use this for initialization
	void Start () {
		this.hp = maxHp;
		this.mp = maxMp;
		Vector3 pos = this.transform.position;
		obj = (GameObject)Instantiate (statusHp,pos,Quaternion.identity);
		obj.transform.SetParent (this.transform);
		pos.y += 1f;
		hpVector  = this.transform.localScale;
		hpVector.x = 10f/hpVector.x;
		hpVector.y = 5f/hpVector.y;
		obj.transform.localScale = hpVector;
		obj.transform.position = pos;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		hpVector.x = (10f/this.transform.localScale.x)*hp / maxHp;
		obj.transform.localScale = hpVector;
	}
}
