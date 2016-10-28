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
    public GameObject fightZiji;
    public GameObject fightDiren;
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
        }
    }

    //放置选项按键，传入的参数是GameEvent数组，需要继承GameEvent类来进行开发
    public void setButton(GameEvent[] gameEvent) {
        GameObject obj;
        Vector3 pos;
        XuanXiangEvent xxEvent;
        Thread.Sleep(200);
        destroyButton();
        int n = 0;
        for (int i = 0; i < gameEvent.Length; i++,n++) {
            if (!gameEvent[i].isCanShow) {
                continue;
            }
            pos = Vector3.zero;
            obj = (GameObject)Instantiate(xuanXiang, pos, Quaternion.identity);
            obj.transform.SetParent(this.transform.Find("GameGraphics").transform.Find("UICanvas").transform.Find("TextFrame").transform);
            RectTransform rectRra = obj.GetComponent<RectTransform>();
            pos = Vector3.zero;
            pos.x = 0f;
            pos.y = 100f - n * 50;
            pos.z = 0f;
            rectRra.anchoredPosition3D = pos;
            rectRra.localScale = Vector3.one;
            Text text = obj.GetComponentInChildren<Text>();
            text.text = (n + 1) + "," + gameEvent[i].buttonText;
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
        if (enemyNpc.isNpc) {
            enemyNpc.hp = enemyNpc.maxHp;
            enemyNpc.mp = enemyNpc.maxMp;
        }
        enemyNpc.isCanFight = false;
        enemyNpc.speedTime = 0;
        myNpc.isCanFight = false;
        myNpc.speedTime = 0;
        this.enemyNpc = enemyNpc;
        changeMode(MainGame.MODE_FIGHT);
    }

    //结束战斗-进入战斗后不可使用backMove()
    public void endFight() {
        //初始化战斗信息
        destroyButton();
        enemyNpc.isCanFight = false;
        enemyNpc.speedTime = 0;
        myNpc.isCanFight = false;
        myNpc.speedTime = 0;
        this.enemyNpc = null;
        changeMode(MainGame.MODE_MOVE);
    }

    //普通攻击
    public void normalAttack() {
        if (myNpc.isCanFight) {
            ziji.startFight((this.myNpc.norFightSpeed * (1f + this.myNpc.fightSpeedAmp)));
            myNpc.normalAttack(enemyNpc);
        }
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
        this.ziji = fightZiji.GetComponent<AnimatorNormalFightAnimator>();
        this.diren = fightDiren.GetComponent<AnimatorNormalFightAnimator>();

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
                    this.enemyNpc.addExp(myNpc.exp * (myNpc.extraExpOdds / 100f + 1f));
                    this.enemyNpc.deleteExp(this.enemyNpc.exp / 2f);
                    textEvent.setNotice("很遗憾你输掉了战斗\n作为惩罚将扣除你当前经验的50%", 1.5f, true);
                    this.myNpc.hp = this.myNpc.maxHp;
                    this.myNpc.mp = this.myNpc.maxMp;
                    endFight();
                    break;
                case 2://enemyNpc失败
                    this.myNpc.addExp(enemyNpc.exp * (enemyNpc.extraExpOdds / 100f + 1f));
                    textEvent.setNotice("恭喜你赢得了战斗", 1.5f, true);
                    endFight();
                    break;
            }
        }
        if (this.mode != MainGame.MODE_MOVE) {
            buttonList.get(choseXuanXiang).GetComponent<Button>().Select();
        }
    }

    //电脑AI,战斗时
    private void fightNpcAI(GameNpc tNpc) {
        if (tNpc.isCanFight) {
            tNpc.speedTime = 0;
            diren.startFight((this.enemyNpc.norFightSpeed * (1f + this.enemyNpc.fightSpeedAmp)));
            tNpc.normalAttack(this.myNpc);
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
                backMove();
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
            if (myNpc.isCanFight) {
                myNpc.speedTime = 0;
                textEvent.setNotice(obj.GetComponent<XuanXiangEvent>().getGameEvent().conText, 0.4f, true);
            } else {
                textEvent.setNotice("你现在还不能释放技能", 0.4f, true);
            }
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
                    textEvent.setNotice(obj.GetComponent<XuanXiangEvent>().getGameEvent().conText, 0.85f, false);
                }
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
