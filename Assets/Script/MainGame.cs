using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Collections;

public class MainGame : MonoBehaviour {
    //状态相关
    public static bool isStop = false;
    public static bool isStart = false;
    public Text fpsText;
    public float autoSaveMaxTime = 1f;//自动保存游戏(秒)
    private float autoSaveTime = 0f;
    private float fpsTime = 0;
    private int fps = 0;
    //数据相关
    private GameData data;
    //摄像头相关
	Canvas gameCanvas;
    Camera gameCamera;
    Camera fightCamera;
    Camera uiCamera;
    //地图相关
    public GameObject floor;
    public int Width = 50;
    public int Height = 50;

    //虚拟按键选项相关
    public GameObject xuanXiang;
    public GameObject functionButton;
    private GameEvent[] buttonEvent;
    private int cutPoint = 0;//分割点
    private int buttonCount = 0;//按键总数
    private LinkedList<GameObject> buttonList;
    private int choseXuanXiang;//当前选择的选项,默认为0
    private int lastChoseXuanXiang;//上一个YES选择的选项，双击则放大对话框
    private float xuanXiangTime;
    public int buttonNumber = 7;

    //移动相关
    public GameObject npc;
    private GameEmptyObject nowObject;
    private Animator ani;
    private Move move;
    private float dur = 0.0f;
    private int mode = 0;

    //文本相关
    public GameObject textKuang;
    public GameObject textKuangMove;
    public GameObject textDoc;//介绍
    public GameObject statusPanel;//角色面板
    private Text textOfStatusPanel;//角色面板
    private bool isShowStatusPanel = false;
    private bool isTextDoc = false;
    private TextEvent textEvent;

    //战斗相关
    public GameObject status;
    public GameObject fightZiji;
    public GameObject fightDiren;
    private GameNpc enemyNpc;
    private GameNpc myNpc;
    private GameObject myHp;
    private GameObject myMp;
    private GameObject mySpeed;
    private GameObject enemyHp;
    private GameObject enemyMp;
    private GameObject enemySpeed;
    private AnimatorNormalFightAnimator ziji;
    private AnimatorNormalFightAnimator diren;

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
    public const int KEY_STATUSPANEL = 6;
    public const int KEY_BACKPACK = 7;

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
            string str;
            str = nowObject.ObjectName + "\n";
            str += "----------" + "\n";
            for (int i = 0; i < nowObject.introduce.Length; i++) {
                str += nowObject.introduce[i] + "\n";
            }
            this.mode = MainGame.MODE_MOVE;
            setText(str);
        }
    }

    //Button接口-用于直接点击Button时触发
    public void keyDownButton(GameObject gameObj) {
        choseXuanXiang = buttonList.find(gameObj);
        if (choseXuanXiang == -1) {
            choseXuanXiang = 0;
        }
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
            case KEY_STATUSPANEL:
                KeyStatusPanelEvent();
                break;
            case KEY_BACKPACK:
                KeyBackPackEvent();
                break;
        }
    }

    //放置选项按键，传入的参数是GameEvent数组，需要继承GameEvent类来进行开发
    public void setButton(GameEvent[] gameEvent) {
        buttonEvent = gameEvent;
        this.cutPoint = 0;
        this.buttonCount = 0;
        for (int i =0; i < gameEvent.Length; i++) {
            if (!gameEvent[i].isCanShow) {
                continue;
            }
            buttonCount++;
        }
        setButton(buttonEvent, cutPoint);
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
    //放置文本框，独立于游戏世界-基于gameObject
    public void setTextMove(string str,float time, GameObject gameObject) {
        setTextMove(str,time, gameObject.transform.position.x, gameObject.transform.position.y + 2f);
    }
    //放置文本框，独立于游戏世界-基于x,y
    public void setTextMove(string str, float time, float posX, float posY) {
        GameObject obj;
        Vector3 pos;
        pos = Vector3.zero;
        obj = (GameObject)Instantiate(textKuangMove, pos, Quaternion.identity);
        obj.transform.SetParent(this.transform.Find("TextCanvas").transform);
        RectTransform rectRra = obj.GetComponent<RectTransform>();
        pos = Vector3.zero;
        pos.x = posX;
        pos.y = posY;
        pos.z = 0f;
        rectRra.anchoredPosition3D = pos;
        Text text = obj.GetComponentInChildren<Text>();
        text.text = str;
        AutoMove autoMove = obj.GetComponent<AutoMove>();
        autoMove.startMove(time);
    }
    //放置文本，在文本里显示，即UI显示
    public void setText(string str) {
        isTextDoc = true;
        if (!textEvent.getIsText()) {
            this.textDoc.gameObject.SetActive(true);
        }
        this.textDoc.GetComponent<Text>().text = str;
    }
    //取消介绍框
    public void closeText() {
        this.textDoc.gameObject.SetActive(false);
        isTextDoc = false;
    }
    public void stopText() {
        this.textDoc.gameObject.SetActive(false);
    }
    public void startText() {
        if (isTextDoc) {
            this.textDoc.gameObject.SetActive(true);
        }
    }
    //设置现在正在面向的对象
    public void setNowObject(GameEmptyObject obj) {
        this.nowObject = obj;
    }
    //取消现在正在面向的对象
    public void closeNowObject() {
        closeText();
        this.nowObject = null;
    }
    /*******************
     *战斗相关
     *
     *
     *******************/
    //开始战斗-进入战斗后不可使用backMove()
    public void startFight(GameNpc enemyNpc) {
        //初始化战斗信息
        enemyNpc.isCanFight = false;
        enemyNpc.speedTime = 0;
        myNpc.isCanFight = false;
        myNpc.speedTime = 0;
        this.enemyNpc = enemyNpc;
        functionButton.transform.Find("StatusPanel").gameObject.SetActive(false);
        functionButton.transform.Find("BackPack").gameObject.SetActive(false);
        this.statusPanel.gameObject.SetActive(false);
        Vector3 pos = Vector3.zero;
        pos.x = 2.5f;
        pos.y = 0.5f;
        ziji.transform.localPosition = pos;
        ziji.GetComponent<Rigidbody>().mass = myNpc.lvl+myNpc.liliang;
        pos.x = -2.5f;
        pos.y = 0.5f;
        diren.transform.localPosition = pos;
        diren.GetComponent<Rigidbody>().mass = myNpc.lvl + myNpc.liliang;
        changeMode(MainGame.MODE_FIGHT);
    }

    //结束战斗-进入战斗后不可使用backMove()
    public void endFight(GameNpc winer,GameNpc loser) {
        //初始化战斗信息
        destroyButton();
        if (loser.isCanDie) {
            Destroy(loser.gameObject);
        } else {
            if (!loser.isNpc) {
                loser.deleteExp(loser.exp / 2f);
            }
            loser.hp = loser.maxHp;
            loser.mp = loser.maxMp;
            loser.isCanFight = false;
            loser.speedTime = 0;
        }
        if (winer.isAutoFullStaut) {
            winer.hp = winer.maxHp;
            winer.mp = winer.maxMp;
        }
        int n = winer.addExp(loser.exp * (loser.extraExpOdds / 100f + 1f));
        if (n > 0) {
            setTextMove("经验+" + loser.exp * (loser.extraExpOdds / 100f + 1f), 0.3f, loser.gameObject);
            setTextMove("等级+" + n, 0.3f, winer.gameObject);
            setTextMove("最大生命" + n * 5, 0.6f, winer.gameObject);
            setTextMove("最大法力" + n * 3, 0.9f, winer.gameObject);
            setTextMove("物理攻击上限" + n * 2, 1.2f, winer.gameObject);
            setTextMove("物理攻击下限" + n * 1, 1.5f, winer.gameObject);
            setTextMove("魔法攻击上限" + n * 1.2, 1.8f, winer.gameObject);
            setTextMove("魔法攻击下限" + n * 0.6, 2.1f, winer.gameObject);
        }
        winer.isCanFight = false;
        winer.speedTime = 0;
        this.enemyNpc = null;
        functionButton.transform.Find("StatusPanel").gameObject.SetActive(true);
        functionButton.transform.Find("BackPack").gameObject.SetActive(true);
        if (isShowStatusPanel) {
            this.statusPanel.gameObject.SetActive(true);
        }
        changeMode(MainGame.MODE_MOVE);
    }

    //普通攻击
    public void normalAttack() {
        if (myNpc.isCanFight) {
            ziji.startFight(myNpc, enemyNpc);
            ziji.runEvent();
        }
    }

    //获取战斗双方
    public GameNpc getEnemyNpc() {
        return this.enemyNpc;
    }
    public GameNpc getMyNpc() {
        return this.myNpc;
    }

    //第一次游戏
    public void firstBegin() {
        this.uiCamera.gameObject.SetActive(true);
    }

    //设置名字，并保存
    public void setNewPlayer(string name) {
        if (TextCheck.isUserName(name)&&name.Length<=8) {
        data.nowId = data.allIdCount++ + 1;
        PlayerPrefs.SetInt("nowId", data.nowId);
        PlayerPrefs.SetInt("AllIDCount", data.allIdCount);
        myNpc.npcName = name;
        LoadAndSave.saveNpc(myNpc, data.nowId);
        this.uiCamera.gameObject.SetActive(false);
        } else if (name.Length <= 8) {
            GameObject.Find("Game").transform.Find("UICamera").transform.Find("FistGameCanvas").transform.Find("NameInputField").transform.Find("Placeholder").GetComponent<Text>().text = "名字里不能带字符";
            GameObject.Find("Game").transform.Find("UICamera").transform.Find("FistGameCanvas").transform.Find("NameInputField").GetComponent<InputField>().text = "";
        } else {
            GameObject.Find("Game").transform.Find("UICamera").transform.Find("FistGameCanvas").transform.Find("NameInputField").transform.Find("Placeholder").GetComponent<Text>().text = "名字不能超过8位";
            GameObject.Find("Game").transform.Find("UICamera").transform.Find("FistGameCanvas").transform.Find("NameInputField").GetComponent<InputField>().text = "";
        }
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
        this.textOfStatusPanel = this.statusPanel.GetComponentInChildren<Text>();

        //摄像头相关
        gameCanvas = this.transform.Find("GameGraphics").transform.Find("UICanvas").GetComponent<Canvas>();
        gameCamera = this.transform.Find("GameCamera").GetComponent<Camera>();
        fightCamera = this.transform.Find("GameFight").transform.Find("FightCamera").GetComponent<Camera>();
        uiCamera = this.transform.Find("UICamera").GetComponent<Camera>();

        //战斗相关
        this.myNpc = npc.GetComponent<GameNpc>();
        this.myHp = status.transform.Find("MyHp").gameObject;
        this.myMp = status.transform.Find("MyMp").gameObject;
        this.mySpeed = status.transform.Find("MySpeed").gameObject;
        this.enemyHp = status.transform.Find("EnemyHp").gameObject;
        this.enemyMp = status.transform.Find("EnemyMp").gameObject;
        this.enemySpeed = status.transform.Find("EnemySpeed").gameObject;
        this.ziji = fightZiji.GetComponent<AnimatorNormalFightAnimator>();
        this.diren = fightDiren.GetComponent<AnimatorNormalFightAnimator>();

        //数据相关
        this.data = new GameData(this, myNpc);

        //开始界面

        this.textDoc.gameObject.SetActive(false);
        this.statusPanel.gameObject.SetActive(false);
        uiCamera.gameObject.SetActive(false);
		gameCanvas.gameObject.SetActive (false);
		gameCamera.gameObject.SetActive (false);
		fightCamera.gameObject.SetActive (false);
        textEvent.setNotice("任意键继续", 100f, true);

        
        //PlayerPrefs.SetInt("nowId", 0);
        //PlayerPrefs.SetInt("AllIdCount", 0);
	}

    void Update() {
        fpsTime += Time.deltaTime;
        fps++;
        if (fpsTime >= 1f) {
            fpsTime = 0;
            fpsText.text = fps.ToString();
            fps = 0;
        }
    }
    //更新函数
    void FixedUpdate() {
        if (Input.anyKey && !isStart) {
            isStart = true;
            data.init();
            gameStart();
        }
        this.autoSaveTime += Time.fixedDeltaTime;
        if (this.autoSaveTime >= this.autoSaveMaxTime) {
            this.autoSaveTime = 0;
            LoadAndSave.saveNpc(myNpc, myNpc.id);
        }
        /*******************
         *更新战斗相关
         *
         *
         *******************/
        if (this.mode == MainGame.MODE_FIGHT) {
            myNpc.speedTime += Time.fixedDeltaTime;
            if (myNpc.speedTime >= (this.myNpc.norFightSpeed * (1f + this.myNpc.fightSpeedAmp))) {
                myNpc.speedTime = (this.myNpc.norFightSpeed * (1f + this.myNpc.fightSpeedAmp));
                myNpc.isCanFight = true;
            } else {
                myNpc.isCanFight = false;
            }
            enemyNpc.speedTime += Time.fixedDeltaTime;
            if (enemyNpc.speedTime >= (this.enemyNpc.norFightSpeed * (1f + this.enemyNpc.fightSpeedAmp))) {
                enemyNpc.speedTime = (this.enemyNpc.norFightSpeed * (1f + this.enemyNpc.fightSpeedAmp));
                enemyNpc.isCanFight = true;
            } else {
                enemyNpc.isCanFight = false;
            }
            fightNpcAI(enemyNpc);
            switch (updateNpcStatus()) {
                case 0:
                    break;
                case 1://myNpc失败
                    textEvent.setNotice("很遗憾你输掉了战斗\n作为惩罚将扣除你当前经验的50%", 1.5f, true);
                    endFight(enemyNpc,myNpc);
                    break;
                case 2://enemyNpc失败
                    textEvent.setNotice("恭喜你赢得了战斗", 1.5f, true);
                    endFight(myNpc, enemyNpc);
                    break;
            }
        }
        
        /*******************
         *文本相关
         *
         *
         *******************/
        if (isShowStatusPanel) {
            uploadStatus(this.myNpc);
        }
        /*******************
         *更新按键
         *
         *
         *******************/
        if (xuanXiangTime < 1000f) {
            xuanXiangTime += Time.fixedDeltaTime;
        }
        if (this.mode != MainGame.MODE_MOVE) {
            buttonList.get(choseXuanXiang).GetComponent<Button>().Select();
        }
    }

    //电脑AI,战斗时
    private void fightNpcAI(GameNpc tNpc) {
        if (tNpc.isCanFight) {
            tNpc.speedTime = 0;
            diren.startFight(enemyNpc, myNpc);
            diren.runEvent();
        }
    }

    //更新战斗时双方的信息
    private int updateNpcStatus() {
        Vector3 scale = Vector3.one;
        int someoneIsDied = 0;//0为无，1为myNpc，2为enemyNpc;
        //更新敌人
        float mHp;
        if (this.myNpc.hp > 0f) {
            mHp = this.myNpc.hp / this.myNpc.maxHp;
        } else if (this.myNpc.hp > this.myNpc.maxHp) {
            mHp = 1f;
        } else {
            mHp = 0f;
            someoneIsDied = 1;
        }
        float mMp;
        if (this.myNpc.mp > 0f) {
            mMp = this.myNpc.mp / this.myNpc.maxMp;
        } else if (this.myNpc.mp > this.myNpc.maxMp) {
            mMp = 1f;
        } else {
            mMp = 0f;
        }
        mMp = this.myNpc.mp / this.myNpc.maxMp;
        float mSpeed = myNpc.speedTime / (this.myNpc.norFightSpeed * (1f + this.myNpc.fightSpeedAmp));
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
        } else if (this.enemyNpc.hp > this.enemyNpc.maxHp) {
            eHp = 1f;
        } else {
            eHp = 0f;
            someoneIsDied = 2;
        }
        float eMp;
        if (this.enemyNpc.mp > 0f) {
            eMp = this.enemyNpc.mp / this.enemyNpc.maxMp;
        } else if (this.enemyNpc.hp > this.enemyNpc.maxHp) {
            eMp = 1f;
        } else {
            eMp = 0f;
        }
        float eSpeed = enemyNpc.speedTime / (this.enemyNpc.norFightSpeed * (1f + this.enemyNpc.fightSpeedAmp));//注意，norFightSpeed不得为0
        scale.x = 1f * eHp;
        this.enemyHp.transform.localScale = scale;
        scale.x = 1f * eMp;
        this.enemyMp.transform.localScale = scale;
        scale.x = 1f * eSpeed;
        this.enemySpeed.transform.localScale = scale;
        return someoneIsDied;
    }

    //转换模式-该模式转换具有强制性，一般情况下还是使用back系列函数来返回
    private void changeMode(int mode) {
        switch (mode) {
            case MainGame.MODE_DUIHUA:
                break;
            case MainGame.MODE_FIGHT:
                switch (this.mode) {
                    case MODE_MOVE:
                        this.gameCamera.gameObject.SetActive(false);
                        break;
                }
                this.fightCamera.gameObject.SetActive(true);
                this.setButton(npc.GetComponents<FightEvent>());
                this.mode = MainGame.MODE_FIGHT;
                break;
            case MainGame.MODE_MOVE:
                switch (this.mode) {
                    case MODE_FIGHT:
                        this.fightCamera.gameObject.SetActive(false);
                        break;
                }
                this.gameCamera.gameObject.SetActive(true);
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
            default:
                ani.CrossFade("Wait", dur);
                move.stopMove();
                break;
        }
    }

    //方向键上的事件
    private void KeyUpEvent() {
        switch (this.mode) {
            case MODE_MOVE:
                moveNpc(Move.MOVEMODE_UP);
                break;
            case MODE_FIGHT:
            case MODE_XUANXIANG:
                if (choseXuanXiang > 0) {
                    GameObject obj = buttonList.get(choseXuanXiang);
                    if (obj != null && !obj.Equals(default(GameObject))) {
                        buttonList.get(--choseXuanXiang).GetComponent<Button>().Select();
                    }
                } else if (cutPoint > 0) {
                    setButton(buttonEvent, --cutPoint);
                    choseXuanXiang = buttonNumber-1;
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
            case MODE_FIGHT:
            case MODE_XUANXIANG:
                if (choseXuanXiang < buttonList.count-1) {
                    GameObject obj = buttonList.get(choseXuanXiang);
                    if (obj != null && !obj.Equals(default(GameObject))) {
                        buttonList.get(++choseXuanXiang).GetComponent<Button>().Select();
                    }
                } else if (cutPoint < (int)(buttonCount / buttonNumber)) {
                    setButton(buttonEvent, ++cutPoint);
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
                if (cutPoint > 0) {
                    cutPoint--;
                    setButton(buttonEvent, cutPoint);
                }
                break;
            case MODE_FIGHT:
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
                if (cutPoint < (int)(buttonCount / buttonNumber)) {
                    cutPoint++;
                    setButton(buttonEvent, cutPoint);
                }
                break;
            case MODE_FIGHT:
                break;
        }
    }

    //YES按键的事件
    private void KeyYesEvent() {
        GameObject obj;
        switch (this.mode) {
            case MODE_MOVE:
                if (this.nowObject != null) {
                    this.nowObject.CollisionEvent();
                    closeText();
                }
                break;
            case MODE_FIGHT:
            case MODE_XUANXIANG:
                obj = buttonList.get(choseXuanXiang);
                if (obj != null && !obj.Equals(default(GameObject))) {
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
                moveNpc(-1);
                break;
            case MODE_XUANXIANG:
                backMove();
                break;
        }
    }

    //StatusPanel按键的事件
    private void KeyStatusPanelEvent() {
        switch (this.mode) {
            case MODE_MOVE:
            case MODE_XUANXIANG:
            case MODE_FIGHT:
                if (isShowStatusPanel) {
                    closeStatusPanel();
                } else {
                    showStatusPanel(this.myNpc);
                }
                break;
        }
    }
    //BackPackl按键的事件
    private void KeyBackPackEvent() {
        switch (this.mode) {
            case MODE_MOVE:

                break;
            case MODE_XUANXIANG:

                break;
            case MODE_FIGHT:

                break;
        }
    }
    //显示玩家详细信息
    private void showStatusPanel(GameNpc npc) {
        this.statusPanel.gameObject.SetActive(true);
        isShowStatusPanel = true;
    }
    //关闭玩家详细信息
    private void closeStatusPanel() {
        this.statusPanel.gameObject.SetActive(false);
        isShowStatusPanel = false;
    }
    //更新
    private void uploadStatus(GameNpc npc) {
        this.textOfStatusPanel.text = NpcStatus.getStatus(npc);
    }
    //进行战斗时调用
    private void keyEventOfFight() {
        if (xuanXiangTime <= 0.2f) {
            textEvent.setNotice("你点的太快了,休息一会吧！", 0.4f, Color.black);
        } else {
            xuanXiangTime = 0;
            GameObject obj = buttonList.get(choseXuanXiang);
            if (obj != null && !obj.Equals(default(GameObject))) {
                if (myNpc.isCanFight) {
                    myNpc.speedTime = 0;
                    textEvent.setNotice(obj.GetComponent<XuanXiangEvent>().getGameEvent().conText, 0.4f, true);
                } else {
                    textEvent.setNotice("你现在还不能释放技能", 0.4f, true);
                }
            }
        }
    }

    //进行场景对话时调用
    private void keyEventOfXuanXiang() {
        if (xuanXiangTime <= 0.2f) {
            textEvent.setNotice("你点的太快了！", 0.75f, Color.black);
        } else {
            xuanXiangTime = 0;
            if (lastChoseXuanXiang != choseXuanXiang) {
                lastChoseXuanXiang = choseXuanXiang;
            } else {
                GameObject obj = buttonList.get(choseXuanXiang);
                if (obj != null && !obj.Equals(default(GameObject))) {
                    textEvent.setNotice(obj.GetComponent<XuanXiangEvent>().getGameEvent().conText, 0.85f, false);
                }
                backMove();
            }
        }
    }

    private void setButton(GameEvent[] gameEvent, int cutPoint) {
        destroyButton();
        GameObject obj;
        Vector3 pos;
        XuanXiangEvent xxEvent;
        int n = 0;
        for (int i = cutPoint * buttonNumber; i < gameEvent.Length && n < buttonNumber; i++) {
            if (!gameEvent[i].isCanShow) {
                continue;
            }
            pos = Vector3.zero;
            obj = (GameObject)Instantiate(xuanXiang, pos, Quaternion.identity);
            obj.transform.SetParent(this.transform.Find("GameGraphics").transform.Find("UICanvas").transform.Find("TextFrame").transform);
            RectTransform rectRra = obj.GetComponent<RectTransform>();
            pos = Vector3.zero;
            pos.x = 0f;
            pos.y = 145f - n * 60;
            pos.z = 0f;
            rectRra.anchoredPosition3D = pos;
            rectRra.localScale = Vector3.one;
            Text text = obj.GetComponentInChildren<Text>();
            text.text = (n + cutPoint * buttonNumber + 1) + "." + gameEvent[i].buttonText;
            xxEvent = obj.GetComponent<XuanXiangEvent>();
            xxEvent.setGameEvent(gameEvent[i]);
            buttonList.add(obj);
            n++;
        }
        changeMode(MainGame.MODE_XUANXIANG);
        buttonList.get(0).GetComponent<Button>().Select();
        choseXuanXiang = 0;
        lastChoseXuanXiang = -1;
    }

    //开始游戏
    private void gameStart(){
		gameCanvas.gameObject.SetActive (true);
		gameCamera.gameObject.SetActive (true);
		fightCamera.gameObject.SetActive (false);
        textEvent.setNotice("任意键继续", 0.1f, true);
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
