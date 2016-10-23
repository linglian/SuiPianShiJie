using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainGame : MonoBehaviour {
	
	//地图相关
	public GameObject floor;
	public GameObject XuanXiang;
	public int Width = 50;
	public int Height = 50;

	//选项相关
	private LinkedList<GameObject> buttonList;
	private int choseXuanXiang;//当前选择的选项,默认为0

	//移动相关
	public GameObject npc;
	private Animator ani;
	private Move move;
	private float dur = 0.0f;
	private int mode = 0;

	//静态常量
	public const int MODE_MOVE = 0;
	public const int MODE_FIGHT = 1;
	public const int MODE_XUANXIANG = 2;
	public const int MODE_DUIHUA = 3;

	public const int KEY_UP = 0;
	public const int KEY_DOWN = 1;
	public const int KEY_LEFT = 2;
	public const int KEY_RIGHT = 3;
	public const int KEY_YES = 4;
	public const int KEY_NO = 5;

	/************************
	 *不对外开放的函数
	 *
	 *
	 ************************/

	//初始化函数，系统自动调用
	void Start () {
		buttonList = new LinkedList<GameObject> ();
		//初始化地图信息
		GameObject obj;
		Vector3 pos = Vector3.zero;
		for (int i = -Width/2; i < Width/2; i++) {
			for (int j = -Height/2; j < Height/2; j++) {
				pos.x = i*1.6f;
				pos.y = j*1.6f;
				obj = (GameObject)Instantiate (floor, pos, Quaternion.identity);
				obj.transform.SetParent (this.transform.Find ("GameFloor").transform);
			}
		}

		//初始化移动相关
		ani = npc.transform.GetComponent<Animator> ();
		move =npc.transform.GetComponent<Move> ();
	}

	//更新函数
	void FixedUpdate () {
		/*此为电脑键盘输入，测试用，实际运行时请注释掉此处
		if (Input.anyKey) {
			if(Input.GetKeyDown(KeyCode.W)){
				setKeyDown(KEY_UP);
			} else if(Input.GetKey(KeyCode.S)){
				setKeyDown(KEY_DOWN);
			} else if(Input.GetKey(KeyCode.D)){
				setKeyDown(KEY_RIGHT);
			} else if(Input.GetKey(KeyCode.A)){
				setKeyDown(KEY_LEFT);
			}
		}
		*/
	}


	//释放之前的按键
	private void destroyButton(){
		while (buttonList.count != 0) {
			Destroy (buttonList.pop ());
		}
	}

	//移动所控制的npc
	private void moveNpc(int mode){
		switch(mode){
		case Move.MOVEMODE_DOWN:
			ani.CrossFade("Down",dur);
			move.setMoveMode (Move.MOVEMODE_DOWN);
			break;
		case Move.MOVEMODE_UP:
			ani.CrossFade("Up",dur);
			move.setMoveMode (Move.MOVEMODE_UP);
			break;
		case Move.MOVEMODE_LEFT:
			ani.CrossFade("Left",dur);
			move.setMoveMode (Move.MOVEMODE_LEFT);
			break;
		case Move.MOVEMODE_RIGHT:
			ani.CrossFade("Right",dur);
			move.setMoveMode (Move.MOVEMODE_RIGHT);
			break;
		}
	}

	//方向键上的事件
	private void KeyUpEvent(){
		switch(this.mode){
		case MODE_MOVE:
			moveNpc (Move.MOVEMODE_UP);
			break;
		case MODE_XUANXIANG:
			if (choseXuanXiang != 0) {
				buttonList.get (--choseXuanXiang).GetComponent<Button>().Select ();
			}
			break;
		}
	}

	//方向键下的事件
	private void KeyDownEvent(){
		switch(this.mode){
		case MODE_MOVE:
			moveNpc (Move.MOVEMODE_DOWN);
			break;
		case MODE_XUANXIANG:
			if (choseXuanXiang != buttonList.count) {
				buttonList.get (++choseXuanXiang).GetComponent<Button>().Select ();
			}
			break;
		}
	}

	//方向键左的事件
	private void KeyLeftEvent(){
		switch(this.mode){
		case MODE_MOVE:
			moveNpc (Move.MOVEMODE_LEFT);
			break;
		case MODE_XUANXIANG:
			backMove ();
			break;
		}
	}

	//方向键右的事件
	private void KeyRightEvent(){
		switch(this.mode){
		case MODE_MOVE:
			moveNpc (Move.MOVEMODE_RIGHT);
			break;
		case MODE_XUANXIANG:
			backMove ();
			break;
		}
	}

	//YES按键的事件
	private void KeyYesEvent(){
		switch(this.mode){
		case MODE_MOVE:
			break;
		case MODE_XUANXIANG:
			buttonList.get (choseXuanXiang).GetComponent<XuanXiangEvent> ().OnClickListener ();
			buttonList.get (choseXuanXiang).GetComponent<Button> ().Select ();
			break;
		}
	}

	//NO按键的事件
	private void KeyNoEvent(){
		switch(this.mode){
		case MODE_MOVE:
			break;
		case MODE_XUANXIANG:
			backMove ();
			break;
		}
	}

	/************************
	 *public的接口
	 *
	 *
	 ************************/
	//按键接口
	public void backMove(){
		destroyButton ();
		this.mode = MODE_MOVE;
	}
	public void setKeyDown(int key){
		switch(key){
		case KEY_UP:
			KeyUpEvent ();
			break;
		case KEY_DOWN:
			KeyDownEvent ();
			break;
		case KEY_RIGHT:
			KeyRightEvent ();
			break;
		case KEY_LEFT:
			KeyLeftEvent ();
			break;
		case KEY_YES:
			KeyYesEvent ();
			break;
		case KEY_NO:
			KeyNoEvent ();
			break;
		}
	}

	//放置选项按键，传入的参数是GameEvent数组，需要继承GameEvent类来进行开发
	public void setButton(GameEvent []gameEvent){
		GameObject obj;
		Vector3 pos;
		XuanXiangEvent xxEvent;
		destroyButton ();
		for (int i = 0; i < gameEvent.Length; i++) {
			pos = Vector3.zero;
			obj = (GameObject)Instantiate (XuanXiang, pos, Quaternion.identity);
			obj.transform.SetParent (this.transform.Find ("GameGraphics").transform.Find ("UICanvas").transform.Find ("TextFrame").transform);
			RectTransform rectRra = obj.GetComponent<RectTransform> ();
			pos = Vector3.zero;
			pos.x = 0f;
			pos.y = 100f-i*50;
			pos.z = 0f;
			rectRra.anchoredPosition3D = pos;
			rectRra.localScale = Vector3.one;	
			Text text = obj.GetComponentInChildren<Text> ();
			text.text = (i+1)+","+gameEvent[i].text;
			xxEvent = obj.GetComponent<XuanXiangEvent> ();
			xxEvent.setGameEvent (gameEvent [i]);
			buttonList.add(obj);
		}
		this.mode = MODE_XUANXIANG;
		buttonList.get (0).GetComponent<Button> ().Select ();
		choseXuanXiang = 0;
	}

	public void setMode(int mode){
		this.mode = mode;
	}
	public int getMode(){
		return mode;
	}

}
