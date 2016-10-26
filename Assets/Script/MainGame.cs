using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Collections;

public class MainGame : MonoBehaviour {
    //摄像头相关
    Camera gameCamera;
    Camera fightCamera;

    //地图相关
    public GameObject floor;
    public int Width = 50;
    public int Height = 50;

    //虚拟按键选项相关
    public GameObject xuanXiang;
    private LinkedList<GameObject> buttonList;
    private int choseXuanXiang;//当前选择的选项,默认为0
    private int lastChoseXuanXiang;//上一个YES选择的选项，双击则放大对话框
    private float xuanXiangTime;

    //移动相关
    public GameObject npc;
    private Animator ani;
    private Move move;
    private float dur = 0.0f;
    private int mode = 0;

    //文本相关
    public GameObject textKuang;
    private TextEvent textEvent;

    //战斗相关
    private GameNpc enemyNpc;
    public GameNpc myNpc;
    public GameObject status;
    private GameObject myHp;
    private GameObject myMp;
    private GameObject mySpeed;
    private float mySpeedTime;
    private bool myIsCanFight;
    private GameObject enemyHp;
    private GameObject enemyMp;
    private GameObject enemySpeed;
    private float enemySpeedTime;
    private bool enemyIsCanFight;

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

    /******************************
     *对外开放的接口
     *
     *
     ******************************/

    /*******************
     *用户输入相关
     *
     *
     *******************/
    //返回移动状态
    public void backMove() {
        if (this.mode != MainGame.MODE_FIGHT) {
            destroyButton();
            this.mode = MainGame.MODE_MOVE;
        }
    }

    //Button接口-用于直接点击Button时触发
    public void keyDownButton(GameObject gameObj) {
        choseXuanXiang = buttonList.find(gameObj);
        switch (this.mode) {
            case MainGame.MODE_FIGHT:
                keyEventOfFight();
                break;
            case MainGame.MODE_XUANXIANG:
                keyEventOfXuanXiang();
                break;
        }
        xuanXiangTime = 0;
    }
    //虚拟按键接口
    public void setKeyDown(int key) {
        switch (key) {
            case KEY_UP:
                KeyUpEvent();
                break;
            case KEY_DOWN:
                KeyDownEvent();
                break;
            case KEY_RIGHT:
                KeyRightEvent();
                break;
            case KEY_LEFT:
                KeyLeftEvent();
                break;
            case KEY_YES:
                KeyYesEvent();
                break;
            case KEY_NO:
                KeyNoEvent();
                break;
        }
    }

    //放置选项按键，传入的参数是GameEvent数组，需要继承GameEvent类来进行开发
    public void setButton(GameEvent[] gameEvent) {
        GameObject obj;
        Vector3 pos;
        XuanXiangEvent xxEvent;
        Thread.Sleep(200);
        destroyButton();
        for (int i = 0; i < gameEvent.Length; i++) {
            pos = Vector3.zero;
            obj = (GameObject)Instantiate(xuanXiang, pos, Quaternion.identity);
            obj.transform.SetParent(this.transform.Find("GameGraphics").transform.Find("UICanvas").transform.Find("TextFrame").transform);
            RectTransform rectRra = obj.GetComponent<RectTransform>();
            pos = Vector3.zero;
            pos.x = 0f;
            pos.y = 100f - i * 50;
            pos.z = 0f;
            rectRra.anchoredPosition3D = pos;
            rectRra.localScale = Vector3.one;
            Text text = obj.GetComponentInChildren<Text>();
            text.text = (i + 1) + "," + gameEvent[i].buttonText;
            xxEvent = obj.GetComponent<XuanXiangEvent>();
            xxEvent.setGameEvent(gameEvent[i]);
            buttonList.add(obj);
        }
        changeMode(MainGame.MODE_XUANXIANG);
        buttonList.get(0).GetComponent<Button>().Select();
        choseXuanXiang = 0;
        lastChoseXuanXiang = -1;
    }
    
    /*******************
     *文本提示相关
     *
     *
     *******************/

    //放置文本框，独立于游戏世界-基于gameObject
    public void setText(string str, GameObject gameObject) {
        setText(str, gameObject.transform.position.x, gameObject.transform.position.y + 2f);
    }
    //放置文本框，独立于游戏世界-基于x,y
    public void setText(string str, float posX, float posY) {
        GameObject obj;
        Vector3 pos;
        pos = Vector3.zero;
        obj = (GameObject)Instantiate(textKuang, pos, Quaternion.identity);
        obj.transform.SetParent(this.transform.Find("TextCanvas").transform);
        RectTransform rectRra = obj.GetComponent<RectTransform>();
        pos = Vector3.zero;
        pos.x = posX;
        pos.y = posY;
        pos.z = 0f;
        rectRra.anchoredPosition3D = pos;
        Text text = obj.GetComponentInChildren<Text>();
        text.text = str;
    }

    
    /*******************
     *战斗相关
     *
     *
     *******************/

    //开始战斗-进入战斗后不可使用backMove()
    public void startFight(GameNpc enemyNpc) {
        //初始化战斗信息
        this.mySpeedTime = 0;
        this.enemySpeedTime = 0;
        this.myIsCanFight = false;
        this.enemyIsCanFight = false;
        this.enemyNpc = enemyNpc;
        changeMode(MainGame.MODE_FIGHT);
    }

    //获取战斗双方
    public GameNpc getEnemyNpc() {
        if (this.mode == MainGame.MODE_FIGHT) {
            return this.enemyNpc;
        }
        return null;
    }
    public GameNpc getMyNpc() {
        if (this.mode == MainGame.MODE_FIGHT) {
            return this.myNpc;
        }
        return null;
    }

    /************************
     *不对外开放的函数
     *
     *
     ************************/

    
    /*******************
     *基础相关
     *
     *
     *******************/
    //初始化函数，系统自动调用
    void Start() {
        buttonList = new LinkedList<GameObject>();
        //初始化地图信息
        GameObject obj;
        Vector3 pos = Vector3.zero;
        for (int i = -Width / 2; i < Width / 2; i++) {
            for (int j = -Height / 2; j < Height / 2; j++) {
                pos.x = i * 1.6f;
                pos.y = j * 1.6f;
                obj = (GameObject)Instantiate(floor, pos, Quaternion.identity);
                obj.transform.SetParent(this.transform.Find("GameFloor").transform);
            }
        }

        //初始化移动相关
        ani = npc.transform.GetComponent<Animator>();
        move = npc.transform.GetComponent<Move>();

        //文本相关
        textEvent = GameObject.Find("Game").transform.Find("GameGraphics").transform.Find("NoticeCanvas").transform.Find("Notice").transform.GetComponent<TextEvent>();

        //摄像头相关
        gameCamera = this.transform.Find("GameCamera").GetComponent<Camera>();
        fightCamera = this.transform.Find("GameFight").transform.Find("FightCamera").GetComponent<Camera>();

        //战斗相关
        this.myNpc = npc.GetComponent<GameNpc>();
        this.myHp = status.transform.Find("MyHp").gameObject;
        this.myMp = status.transform.Find("MyMp").gameObject;
        this.mySpeed = status.transform.Find("MySpeed").gameObject;
        this.enemyHp = status.transform.Find("EnemyHp").gameObject;
        this.enemyMp = status.transform.Find("EnemyMp").gameObject;
        this.enemySpeed = status.transform.Find("EnemySpeed").gameObject;

    }

    //更新函数
    void FixedUpdate() {
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
        xuanXiangTime += Time.fixedDeltaTime;
        if (this.mode == MainGame.MODE_FIGHT) {
            if (!this.myIsCanFight) {
                this.mySpeedTime += Time.fixedDeltaTime;
                if (this.mySpeedTime >= (this.myNpc.norFightSpeed * (1f + this.myNpc.fightSpeedAmp))) {
                    this.mySpeedTime = (this.myNpc.norFightSpeed * (1f + this.myNpc.fightSpeedAmp));
                    myIsCanFight = true;
                }
            }
            if (!this.enemyIsCanFight) {
                this.enemySpeedTime += Time.fixedDeltaTime;
                if (this.enemySpeedTime >= (this.enemyNpc.norFightSpeed * (1f + this.enemyNpc.fightSpeedAmp))) {
                    this.enemySpeedTime = (this.enemyNpc.norFightSpeed * (1f + this.enemyNpc.fightSpeedAmp));
                    enemyIsCanFight = true;
                }
            }
            updateNpcStatus();
        }
    }

    //更新战斗时双方的信息
    private void updateNpcStatus() {
        Vector3 scale = Vector3.one;
        //更新敌人
        float mHp;
        if (this.myNpc.hp > 0f) {
            mHp = this.myNpc.hp / this.myNpc.maxHp;
        } else {
            mHp = 0f;
        }
        float mMp;
        if (this.myNpc.mp > 0f) {
            mMp = this.myNpc.mp / this.myNpc.maxMp;
        } else {
            mMp = 0f;
        }
        mMp = this.myNpc.mp / this.myNpc.maxMp;
        float mSpeed = this.mySpeedTime / (this.myNpc.norFightSpeed * (1f + this.myNpc.fightSpeedAmp));
        scale.x = 1f * mHp;
        this.myHp.transform.localScale = scale;
        scale.x = 1f * mMp;
        this.myMp.transform.localScale = scale;
        scale.x = 1f * mSpeed;
        this.mySpeed.transform.localScale = scale;
        //更新敌人
        float eHp;
        if (this.enemyNpc.hp > 0f) {
            eHp = this.enemyNpc.hp / this.enemyNpc.maxHp;
        } else {
            eHp = 0f;
        }
        float eMp;
        if (this.enemyNpc.mp > 0f) {
            eMp = this.enemyNpc.mp / this.enemyNpc.maxMp;
        } else {
            eMp = 0f;
        }
        float eSpeed = this.enemySpeedTime / (this.enemyNpc.norFightSpeed * (1f + this.enemyNpc.fightSpeedAmp));//注意，norFightSpeed不得为0
        scale.x = 1f * eHp;
        this.enemyHp.transform.localScale = scale;
        scale.x = 1f * eMp;
        this.enemyMp.transform.localScale = scale;
        scale.x = 1f * eSpeed;
        this.enemySpeed.transform.localScale = scale;
    }

    //转换模式-该模式转换具有强制性，一般情况下还是使用back系列函数来返回
    private void changeMode(int mode) {
        this.lastChoseXuanXiang = -1;
        switch (mode) {
            case MainGame.MODE_DUIHUA:
                break;
            case MainGame.MODE_FIGHT:
                this.mode = MainGame.MODE_FIGHT;
                this.fightCamera.gameObject.SetActive(true);
                this.gameCamera.gameObject.SetActive(false);
                this.setButton(npc.GetComponents<FightEvent>());
                break;
            case MainGame.MODE_MOVE:
                this.mode = MainGame.MODE_MOVE;
                break;
            case MainGame.MODE_XUANXIANG:
                if (this.mode != MainGame.MODE_FIGHT) {
                    this.mode = MainGame.MODE_XUANXIANG;
                }
                break;
        }
    }
    /*******************
     *控制相关
     *
     *
     *******************/
    //释放之前的按键
    private void destroyButton() {
        while (buttonList.count != 0) {
            Destroy(buttonList.pop());
        }
    }

    //移动所控制的npc
    private void moveNpc(int mode) {
        switch (mode) {
            case Move.MOVEMODE_DOWN:
                ani.CrossFade("Down", dur);
                move.setMoveMode(Move.MOVEMODE_DOWN);
                break;
            case Move.MOVEMODE_UP:
                ani.CrossFade("Up", dur);
                move.setMoveMode(Move.MOVEMODE_UP);
                break;
            case Move.MOVEMODE_LEFT:
                ani.CrossFade("Left", dur);
                move.setMoveMode(Move.MOVEMODE_LEFT);
                break;
            case Move.MOVEMODE_RIGHT:
                ani.CrossFade("Right", dur);
                move.setMoveMode(Move.MOVEMODE_RIGHT);
                break;
        }
    }

    //方向键上的事件
    private void KeyUpEvent() {
        switch (this.mode) {
            case MODE_MOVE:
                moveNpc(Move.MOVEMODE_UP);
                break;
            case MODE_XUANXIANG:
                if (choseXuanXiang != 0) {
                    GameObject obj = buttonList.get(choseXuanXiang);
                    if (!obj.Equals(default(GameObject)))
                        buttonList.get(--choseXuanXiang).GetComponent<Button>().Select();
                    else
                        backMove();
                }
                break;
        }
    }

    //方向键下的事件
    private void KeyDownEvent() {
        switch (this.mode) {
            case MODE_MOVE:
                moveNpc(Move.MOVEMODE_DOWN);
                break;
            case MODE_XUANXIANG:
                if (choseXuanXiang != buttonList.count) {
                    GameObject obj = buttonList.get(choseXuanXiang);
                    if (!obj.Equals(default(GameObject)))
                        buttonList.get(++choseXuanXiang).GetComponent<Button>().Select();
                    else
                        backMove();
                }
                break;
        }
    }

    //方向键左的事件
    private void KeyLeftEvent() {
        switch (this.mode) {
            case MODE_MOVE:
                moveNpc(Move.MOVEMODE_LEFT);
                break;
            case MODE_XUANXIANG:
                backMove();
                break;
        }
    }

    //方向键右的事件
    private void KeyRightEvent() {
        switch (this.mode) {
            case MODE_MOVE:
                moveNpc(Move.MOVEMODE_RIGHT);
                break;
            case MODE_XUANXIANG:
                backMove();
                break;
        }
    }

    //YES按键的事件
    private void KeyYesEvent() {
        GameObject obj;
        switch (this.mode) {
            case MODE_MOVE:
                break;
            case MODE_XUANXIANG:
                obj = buttonList.get(choseXuanXiang);
                if (!obj.Equals(default(GameObject))) {
                    obj.GetComponent<XuanXiangEvent>().OnClickListener();
                    obj.GetComponent<Button>().Select();
                }
                break;
        }
    }

    //NO按键的事件
    private void KeyNoEvent() {
        switch (this.mode) {
            case MODE_MOVE:
                break;
            case MODE_XUANXIANG:
                backMove();
                break;
        }
    }

    //进行战斗时调用
    private void keyEventOfFight() {
        GameObject obj = buttonList.get(choseXuanXiang);
        if (obj != null && !obj.Equals(default(GameObject))) {
            textEvent.setNotice(obj.GetComponent<XuanXiangEvent>().getGameEvent().conText, 2.5f, false);
        }
    }

    //进行场景对话时调用
    private void keyEventOfXuanXiang() {
        if (xuanXiangTime <= 0.2f && lastChoseXuanXiang != choseXuanXiang) {
            textEvent.setNotice("你点的太快了！", 0.75f, Color.black);
            backMove();
        } else {
            if (lastChoseXuanXiang != choseXuanXiang) {
                lastChoseXuanXiang = choseXuanXiang;
            } else {
                GameObject obj = buttonList.get(choseXuanXiang);
                if (obj != null && !obj.Equals(default(GameObject))) {
                    textEvent.setNotice(obj.GetComponent<XuanXiangEvent>().getGameEvent().conText, 2.5f, false);
                }
                backMove();
            }
        }
    }
    /******************************
	 *封装函数
	 *
	 *
	 ******************************/

    public void setMode(int mode) {
        this.mode = mode;
    }
    public int getMode() {
        return mode;
    }

}
