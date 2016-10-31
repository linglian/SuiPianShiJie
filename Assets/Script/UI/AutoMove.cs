using UnityEngine;
using System.Collections;

public class AutoMove : MonoBehaviour {
    public float dieTime = 0.5f;
    public float posXSpeed = 0.1f;
    public float posYSpeed = 0.1f;
    RectTransform rectRra;
    Vector3 pos;
    private float time;
    private float delTime;
    private bool isStart;

    /******************************
     *对外开放的接口
     *
     *
     ******************************/
    public void startMove(float time) {
        this.delTime = time;
        this.time = 0;
        this.isStart = true;
    }
    /******************************
     *不对外开放的函数
     *
     *
     ******************************/
	// Use this for initialization
	void Start () {
        this.rectRra = this.GetComponent<RectTransform>();
        this.pos = this.transform.position;
	}

    // Update is called once per frame
    void Update() {
        if (isStart) {
            time += Time.deltaTime;
            if (time >= delTime) {
                pos.x += posXSpeed * Time.deltaTime;
                pos.y += posYSpeed * Time.deltaTime;
                rectRra.anchoredPosition3D = pos;
            }
            if (time >= delTime + dieTime) {
                Destroy(this.gameObject);
            }
        }
	}
}
