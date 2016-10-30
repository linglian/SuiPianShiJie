using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextEvent : MonoBehaviour {
    private bool isText = false;
    private float aliveTime;
    private Text text;
    private TextChangeColor textChange;
    private MainGame mainGame;
    /******************************
     *对外开放的接口
     *
     *
     ******************************/
	public void setNotice(string str, float time,bool isChangeColor){
		text.text = str;
		textChange.isChange = isChangeColor;
        aliveTime = time;
        startNotice();
	}
	public void setNotice(string str, float time,float r,float g, float b){
		text.text = str;
		textChange.isChange = false;
		textChange.setColor (new Color(r,g,b));
        aliveTime = time;
        startNotice();
	}
	public void setNotice(string str, float time,Color col){
		text.text = str;
		textChange.isChange = false;
		textChange.setColor (col);
		aliveTime = time;
        startNotice();
    }
    public bool getIsText() {
        return this.isText;
    }

    /******************************
     *不对外开放的函数
     *
     *
     ******************************/
    private void startNotice() {
        this.gameObject.SetActive(true);
        this.isText = true;
        mainGame.stopText();
    }
    private void stopNotice() {
        this.gameObject.SetActive(false);
        this.isText = false;
        mainGame.startText();
    }

    // Use this for initialization
    void Start() {
        aliveTime = 0;
        text = this.transform.GetComponent<Text>();
        textChange = this.transform.GetComponent<TextChangeColor>();
        mainGame = GameObject.Find("Game").GetComponent<MainGame>();
    }

	// Update is called once per frame
	void Update () {
        if (this.aliveTime < 0f) {
            stopNotice();
        } else {
            this.aliveTime -= Time.deltaTime;
        }
	}
}
