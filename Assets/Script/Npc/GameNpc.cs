using UnityEngine;
using System.Collections;

/*******************
 *在这里规定，此处所有的数据成员除了在MainGame类里有set和get权限，其他不得使用set
 *为了配合Unity方便调属性和游戏速度，不使用封装函数
 *
 *******************/
public class GameNpc : MonoBehaviour {
	//生命显示相关
	private GameObject statusHp;
	GameObject obj;
	Vector3 hpVector;

	//基础属性
	public float hp;//生命
	public float maxHp = 10f;//最大生命
	public float mp;//法力
	public float maxMp = 2f;//最大法力
	public float pAttLow = 1f;//物理攻击下限
	public float pAttHigh = 3f;//物理攻击上限
	public float mAttLow = 1f;//法术攻击下限
	public float mAttHigh = 1f;//法术攻击上限
	public float pDefPower = 1f;//物理防御
	public float mDefPower = 0f;//法术防御
	public int lvl = 1;//等级
	public int maxLvl = 10;//最大等级
	public float exp = 0f;//经验
	public float maxExp = 100;//最大经验
	public float dropExpOdds = 0;//经验掉落率
	public float money = 50f;//金钱
	public float maxMoney = 10000f;//最大金钱
	public float dropMonOdds = 0f;//金钱掉落率
	public int liliang = 0;//力量
	public int minjie = 0;//敏捷
	public int zhili = 0;//智力
	public int tizhi = 0;//体质
	public float norFightSpeed = 2.5f;//基础攻击速度
	public float fightSpeedAmp = 0f;//攻击速度加成

	//特殊属性
	public float pVioHurtAmp = 50f;//物理暴击伤害增幅
	public float pVioHurtAdd = 0f;//物理暴击伤害附加
	public float pVioHurtOdds = 0f;//物理暴击概率
	public float pVioHurtMissDefAmp = 0f;//物理暴击时无视防御力增幅
	public float pVioHurtMissDefAdd = 0f;//物理暴击时无视防御力附加
	public float pVioHurtMissDefOdds = 0f;//物理暴击时无视防御力概率
	public float doeOdds = 0f;//闪避几率
	public float recoverHpPerSecond = 0f;//每秒回复生命
	public float recoverMpPerSecond = 0f;//每秒回复法力
	public float recoverHpPerHit = 0f;//击中回复生命
	public float recoverMpPerHit = 0f;//击中回复法力
	public float extraItemOdds = 0f;//物品掉落加成
	public float extraExpOdds = 0f;//经验加成
	public float extraMoneyOdds = 0f;//金钱加成

	//状态属性
    public bool isDie = false;
    public float speedTime = 0;
    public bool isCanFight = false;
    public bool isNpc = true;

	/************************
	 *不对外开放的函数
	 *
	 *
	 ************************/

	//初始化函数
	void Start () {
		this.hp = maxHp;
        this.mp = maxMp;
        statusHp = GameObject.Find("MainGame").transform.Find("Game").GetComponent<GameModel>().statusHp;
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
	
	//刷新函数
	void FixedUpdate () {
		hpVector.x = (10f/this.transform.localScale.x)*(hp*0.7f+mp*0.3f) / maxHp;
		obj.transform.localScale = hpVector;
	}

    private void upGrade() {
        lvl++;
        maxExp = lvl * lvl * 20;
        this.maxHp += 5;
        this.maxMp += 3;
        this.hp = maxHp;
        this.mp = maxMp;
        this.pAttLow += 1;
        this.pAttHigh += 2;
        this.mAttLow += 1;
        this.mAttHigh += 2;
    }
    /************************
     *对外开放的接口
     *
     *
     ************************/

    //一般攻击
	public void normalAttack(GameNpc npc){
		npc.hp-= getPhysicsRealAttack(this,npc);
	}

	//获得攻击
	public static float getPhysicsRealAttack(GameNpc attNpc,GameNpc defNpc){
		float pAtt = Random.Range (attNpc.pAttLow, attNpc.pAttHigh);
		float pDef = defNpc.pDefPower;
		if (Random.Range (1, 100) <= attNpc.pVioHurtOdds) {//暴击时
			pAtt *= ((attNpc.pVioHurtAmp/100f)+1f);
            pAtt += attNpc.pVioHurtAdd;
			if (Random.Range (1, 100) >= attNpc.pVioHurtMissDefOdds) {
				if (1 - attNpc.pVioHurtMissDefAmp / 100f <= 0) {
					pDef = 0;
				}
				else {
					pDef *= 1 - attNpc.pVioHurtMissDefAmp / 100f;
				}
				if (attNpc.pVioHurtMissDefAdd >= pDef) {
					pDef = 0;
				} else {
					pDef -= attNpc.pVioHurtMissDefAdd;
				}
			}
		}
		pAtt -= pDef;
		return pAtt;
	}

    public void addExp(float aExp) {
        this.exp += aExp;
        while (this.exp >= maxExp) {
            this.exp -= maxExp;
            upGrade();
        }
    }

    public void deleteExp(float dExp) {
        this.exp -= dExp;
        if (this.exp < 0) {
            this.exp = 0;
        }
    }
	/************************
	* 封装函数
	* 
	* 
	************************/


}
