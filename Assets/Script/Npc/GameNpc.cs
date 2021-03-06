﻿using UnityEngine;
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
    [SerializeField, HeaderAttribute("-----基础属性------"), Space(15), TooltipAttribute("id[唯一性]")]
    public int id;//id,由系统生成
    public string npcName;//姓名，玩家自己填写
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
    [SerializeField, Range(0, 100)]
	public float dropExpOdds = 0;//经验掉落率
	public float money = 50f;//金钱
    public float maxMoney = 10000f;//最大金钱
    [SerializeField, Range(0, 100)]
	public float dropMoneyOdds = 0f;//金钱掉落率
	public int liliang = 0;//力量
	public int minjie = 0;//敏捷
	public int zhili = 0;//智力
	public int tizhi = 0;//体质
	public float norFightSpeed = 2.5f;//基础攻击速度
	public float fightSpeedAmp = 0f;//攻击速度加成

    //特殊属性
    [SerializeField, HeaderAttribute("------特殊属性------"), Space(15), Range(0, 100)]
    public float pVioHurtOdds = 0f;//物理暴击概率
	public float pVioHurtAmp = 50f;//物理暴击伤害增幅
    public float pVioHurtAdd = 0f;//物理暴击伤害附加
    [SerializeField, Range(0, 100)]
    public float pVioHurtMissDefOdds = 0f;//物理暴击时无视防御力概率
	public float pVioHurtMissDefAmp = 0f;//物理暴击时无视防御力增幅
    public float pVioHurtMissDefAdd = 0f;//物理暴击时无视防御力附加
    [SerializeField, Range(0, 100)]
	public float doeOdds = 0f;//闪避几率
	public float recoverHpPerSecond = 0f;//每秒回复生命
	public float recoverMpPerSecond = 0f;//每秒回复法力
	public float recoverHpPerHit = 0f;//击中回复生命
	public float recoverMpPerHit = 0f;//击中回复法力
    public float extraItemOdds = 0f;//物品掉落加成
    public float extraExpOdds = 0f;//经验掉落加成
    public float extraMoneyOdds = 0f;//金钱掉落加成

    //状态属性
    [SerializeField, HeaderAttribute("------状态属性------"), Space(15)]
    private Vector3 lastPos;
    public int mode = 0;
    public GameNpc enemyNpc;
    public bool isDie = false;
    public float speedTime = 0;
    public bool isCanFight = false;
    public bool isNpc = true;
    public bool isCanDie = true;
    public bool isAutoFullStaut = true;
    [SerializeField, Range(0, 100)]
    public float autoHeal = 0;//附加回血百分比
    [SerializeField, Range(0, 100)]
    public float autoMuse = 0;//附加回蓝百分比

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
        this.addHp(this.maxHp * (autoHeal / 100f) * Time.fixedDeltaTime);
        this.addMp(this.maxMp * (autoMuse / 100f) * Time.fixedDeltaTime);
	}

    //升级
    private void upGrade() {
        lvl++;
        maxExp = lvl * lvl * 20;
        this.maxHp += 5;
        this.maxMp += 3;
        this.hp = maxHp;
        this.mp = maxMp;
        this.pAttLow += 1;
        this.pAttHigh += 2;
        this.mAttLow += 0.6f;
        this.mAttHigh += 1.2f;
        this.pDefPower += 0.3f;
        this.mDefPower += 0.15f;
    }
    /************************
     *对外开放的接口
     *
     *
     ************************/
    //传送基于父亲
    public void moveToPosOfParent(Vector3 v){
        lastPos = this.transform.position;
        this.transform.localPosition = v;
    }
    //传送
    public void moveToPosOf(Vector3 v) {
        lastPos = this.transform.position;
        this.transform.position = v;
    }
    public void backLastPos() {
        this.transform.position = lastPos;
    }

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

    public int getFightPower() {
        float power = 0;
        power += this.maxHp;
        power += this.maxMp;
        power += this.pAttLow*3;
        power += this.pAttHigh*5;
        power += this.mAttLow * 3;
        power += this.mAttHigh * 5;
        power += this.pDefPower * 4;
        power += this.mDefPower * 4;
        return (int)power;
    }
    //获得攻击
    public static float getMagicRealAttack(GameNpc attNpc, GameNpc defNpc) {
        float pAtt = Random.Range(attNpc.pAttLow, attNpc.pAttHigh);
        float pDef = defNpc.pDefPower;
        if (Random.Range(1, 100) <= attNpc.pVioHurtOdds) {//暴击时
            pAtt *= ((attNpc.pVioHurtAmp / 100f) + 1f);
            pAtt += attNpc.pVioHurtAdd;
            if (Random.Range(1, 100) >= attNpc.pVioHurtMissDefOdds) {
                if (1 - attNpc.pVioHurtMissDefAmp / 100f <= 0) {
                    pDef = 0;
                } else {
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

	/************************
	* 封装函数
	* 
	* 
	************************/

    //增加经验
    public int addExp(float aExp) {
        this.exp += aExp;
        int n = 0;
        while (this.lvl < this.maxLvl && this.exp >= maxExp) {
            this.exp -= maxExp;
            upGrade();
            n++;
        }
        if (this.exp >= this.maxExp * 10f) {
            this.exp = this.maxExp * 10f;
        }
        return n;
    }

    //减少经验
    public bool deleteExp(float dExp) {
        this.exp -= dExp;
        if (this.exp < 0) {
            this.exp = 0;
            return false;
        }
        return true;
    }

    //增加金钱
    public int addMoney(float aMoney) {
        this.money += aMoney;
        int n = 0;
        while (this.lvl < this.maxLvl && this.money >= maxMoney) {
            this.money -= maxMoney;
            upGrade();
            n++;
        }
        if (this.money >= this.maxMoney * 10f) {
            this.money = this.maxMoney * 10f;
        }
        return n;
    }

    //减少金钱
    public void deleteMoney(float dMoney) {
        this.money -= dMoney;
        if (this.money < 0) {
            this.money = 0;
        }
    }

    public void addHp(float hp) {
        this.hp += hp;
        if (this.hp >= this.maxHp) {
            this.hp = this.maxHp;
        }
    }

    public void minusHp(float hp) {
        this.hp -= hp;
        if (this.hp < 0) {
            this.hp = 0;
        }
    }
    public void addMp(float mp) {
        this.mp += hp;
        if (this.mp >= this.maxMp) {
            this.mp = this.maxMp;
        }
    }

    public void minusMp(float mp) {
        this.mp -= mp;
        if (this.mp < 0) {
            this.mp = 0;
        }
    }
}
