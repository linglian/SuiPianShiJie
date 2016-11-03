using UnityEngine;
using System.Collections;

public class LoadAndSave : MonoBehaviour {
    public static int skillCount = 1;

    /******************************
     *对外开放的接口
     *
     *
     ******************************/
    public static int saveNpc(GameNpc npc, int id) {
        string sId = id.ToString();
        //存储基本信息
        PlayerPrefs.SetInt(sId + "_id", id);
        PlayerPrefs.SetString(sId + "_name", npc.npcName);
        PlayerPrefs.SetFloat(sId + "_x", npc.transform.position.x);
        PlayerPrefs.SetFloat(sId + "_y", npc.transform.position.y);
        PlayerPrefs.SetFloat(sId + "_z", npc.transform.position.z);
        PlayerPrefs.SetFloat(sId + "_hp", npc.hp);
        PlayerPrefs.SetFloat(sId + "_maxHp", npc.maxHp);
        PlayerPrefs.SetFloat(sId + "_mp", npc.mp);
        PlayerPrefs.SetFloat(sId + "_maxMp", npc.maxMp);
        PlayerPrefs.SetFloat(sId + "_pAttlow", npc.pAttLow);
        PlayerPrefs.SetFloat(sId + "_pAttHigh", npc.pAttHigh);
        PlayerPrefs.SetFloat(sId + "_mAttlow", npc.mAttLow);
        PlayerPrefs.SetFloat(sId + "_mAttHigh", npc.mAttHigh);
        PlayerPrefs.SetFloat(sId + "_pDefPower", npc.pDefPower);
        PlayerPrefs.SetFloat(sId + "_mDefPower", npc.mDefPower);
        PlayerPrefs.SetInt(sId + "_lvl", npc.lvl);
        PlayerPrefs.SetInt(sId + "_maxLvl", npc.maxLvl);
        PlayerPrefs.SetFloat(sId + "_exp", npc.exp);
        PlayerPrefs.SetFloat(sId + "_maxExp", npc.maxExp);
        PlayerPrefs.SetFloat(sId + "_money", npc.money);
        PlayerPrefs.SetFloat(sId + "_maxMoney", npc.maxMoney);
        PlayerPrefs.SetInt(sId + "_liliang", npc.liliang);
        PlayerPrefs.SetInt(sId + "_minjie", npc.minjie);
        PlayerPrefs.SetInt(sId + "_zhili", npc.zhili);
        PlayerPrefs.SetInt(sId + "_tizhi", npc.tizhi);
        PlayerPrefs.SetFloat(sId + "_norFightSpeed", npc.norFightSpeed);
        PlayerPrefs.SetFloat(sId + "_fightSpeedAmp", npc.fightSpeedAmp);
        PlayerPrefs.SetFloat(sId + "_pVioHurtAmp", npc.pVioHurtAmp);
        PlayerPrefs.SetFloat(sId + "_pVioHurtAdd", npc.pVioHurtAdd);
        PlayerPrefs.SetFloat(sId + "_pVioHurtOdds", npc.pVioHurtOdds);
        PlayerPrefs.SetFloat(sId + "_pVioHurtMissDefAmp", npc.pVioHurtMissDefAmp);
        PlayerPrefs.SetFloat(sId + "_pVioHurtMissDefAdd", npc.pVioHurtMissDefAdd);
        PlayerPrefs.SetFloat(sId + "_pVioHurtMissDefOdds", npc.pVioHurtMissDefOdds);
        PlayerPrefs.SetFloat(sId + "_doeOdds", npc.doeOdds);
        PlayerPrefs.SetFloat(sId + "_recoverHpPerSecond", npc.recoverHpPerSecond);
        PlayerPrefs.SetFloat(sId + "_recoverMpPerSecond", npc.recoverMpPerSecond);
        PlayerPrefs.SetFloat(sId + "_recoverHpPerHit", npc.recoverHpPerHit);
        PlayerPrefs.SetFloat(sId + "_recoverMpPerHit", npc.recoverMpPerHit);
        PlayerPrefs.SetFloat(sId + "_extraItemOdds", npc.extraItemOdds);
        PlayerPrefs.SetFloat(sId + "_extraExpOdds", npc.extraExpOdds);
        PlayerPrefs.SetFloat(sId + "_extraMoneyOdds", npc.extraMoneyOdds);

        //存储技能
        FightEvent[] f = npc.GetComponents<FightEvent>();
        for (int i = 0; i < f.Length; i++) {
            PlayerPrefs.SetInt(sId + "_Skill" + "_" + f[i].id, f[i].id);
            PlayerPrefs.SetString(sId + "_Skill" + "_" + f[i].id + "_name", f[i].skillName);
            PlayerPrefs.SetString(sId + "_Skill" + "_" + f[i].id + "_text", f[i].skillText);
            PlayerPrefs.SetInt(sId + "_Skill" + "_" + f[i].id + "_lvl", f[i].skillLvl);
            PlayerPrefs.SetInt(sId + "_Skill" + "_" + f[i].id + "_maxLvl", f[i].skillMaxLvl);
            PlayerPrefs.SetFloat(sId + "_Skill" + "_" + f[i].id + "_exp", f[i].skillExp);
            PlayerPrefs.SetFloat(sId + "_Skill" + "_" + f[i].id + "_maxExp", f[i].skillMaxExp);
            PlayerPrefs.SetFloat(sId + "_Skill" + "_" + f[i].id + "_hurtAmp", f[i].skillHurtAmp);
            PlayerPrefs.SetFloat(sId + "_Skill" + "_" + f[i].id + "_hurtAdd", f[i].skillHurtAdd);
        }
        return 1;
    }

    public static int loadNpc(GameNpc npc, int id) {
        string sId = id.ToString();
        //读取基本信息
        npc.id = PlayerPrefs.GetInt(sId + "_id");
        if (npc.id == 0) {
            return 0;
        }
        npc.npcName = PlayerPrefs.GetString(sId + "_name");
        Vector3 pos = Vector3.zero;
        pos.x = PlayerPrefs.GetFloat(sId + "_x");
        pos.y = PlayerPrefs.GetFloat(sId + "_y");
        pos.z = PlayerPrefs.GetFloat(sId + "_z");
        npc.transform.position = pos;
        npc.hp = PlayerPrefs.GetFloat(sId + "_hp");
        npc.maxHp = PlayerPrefs.GetFloat(sId + "_maxHp");
        npc.mp = PlayerPrefs.GetFloat(sId + "_mp");
        npc.maxMp = PlayerPrefs.GetFloat(sId + "_maxMp");
        npc.pAttLow = PlayerPrefs.GetFloat(sId + "_pAttlow");
        npc.pAttHigh = PlayerPrefs.GetFloat(sId + "_pAttHigh");
        npc.mAttLow = PlayerPrefs.GetFloat(sId + "_mAttlow");
        npc.mAttHigh = PlayerPrefs.GetFloat(sId + "_mAttHigh");
        npc.pDefPower = PlayerPrefs.GetFloat(sId + "_pDefPower");
        npc.mDefPower = PlayerPrefs.GetFloat(sId + "_mDefPower");
        npc.lvl = PlayerPrefs.GetInt(sId + "_lvl");
        npc.maxLvl = PlayerPrefs.GetInt(sId + "_maxLvl");
        npc.exp = PlayerPrefs.GetFloat(sId + "_exp");
        npc.maxExp = PlayerPrefs.GetFloat(sId + "_maxExp");
        npc.money = PlayerPrefs.GetFloat(sId + "_money");
        npc.maxMoney = PlayerPrefs.GetFloat(sId + "_maxMoney");
        npc.liliang = PlayerPrefs.GetInt(sId + "_liliang");
        npc.minjie = PlayerPrefs.GetInt(sId + "_minjie");
        npc.zhili = PlayerPrefs.GetInt(sId + "_zhili");
        npc.tizhi = PlayerPrefs.GetInt(sId + "_tizhi");
        npc.norFightSpeed = PlayerPrefs.GetFloat(sId + "_norFightSpeed");
        npc.fightSpeedAmp = PlayerPrefs.GetFloat(sId + "_fightSpeedAmp");
        npc.pVioHurtAmp = PlayerPrefs.GetFloat(sId + "_pVioHurtAmp");
        npc.pVioHurtAdd = PlayerPrefs.GetFloat(sId + "_pVioHurtAdd");
        npc.pVioHurtOdds = PlayerPrefs.GetFloat(sId + "_pVioHurtOdds");
        npc.pVioHurtMissDefAmp = PlayerPrefs.GetFloat(sId + "_pVioHurtMissDefAmp");
        npc.pVioHurtMissDefAdd = PlayerPrefs.GetFloat(sId + "_pVioHurtMissDefAdd");
        npc.pVioHurtMissDefOdds = PlayerPrefs.GetFloat(sId + "_pVioHurtMissDefOdds");
        npc.doeOdds = PlayerPrefs.GetFloat(sId + "_doeOdds");
        npc.recoverHpPerSecond = PlayerPrefs.GetFloat(sId + "_recoverHpPerSecond");
        npc.recoverMpPerSecond = PlayerPrefs.GetFloat(sId + "_recoverMpPerSecond");
        npc.recoverHpPerHit = PlayerPrefs.GetFloat(sId + "_recoverHpPerHit");
        npc.recoverMpPerHit = PlayerPrefs.GetFloat(sId + "_recoverMpPerHit");
        npc.extraItemOdds = PlayerPrefs.GetFloat(sId + "_extraItemOdds");
        npc.extraExpOdds = PlayerPrefs.GetFloat(sId + "_extraExpOdds");
        npc.extraMoneyOdds = PlayerPrefs.GetFloat(sId + "_extraMoneyOdds");
        loadSkill(npc,id);
        return 1;
    }

    private static void loadSkill(GameNpc npc, int id) {
        string sId = id.ToString();
        int skillId;
        for (int i = 0; i < skillCount; i++) {
            skillId = PlayerPrefs.GetInt(sId + "_Skill" + "_" + (i + 1));
            FightEvent temp = null;
            //找到技能
            if (skillId != 0) {
                switch (skillId) {
                    case 1:
                        npc.gameObject.AddComponent<FightNormalAttack>();
                        temp = npc.gameObject.GetComponent<FightNormalAttack>();
                        break;
                }
                if (temp != null) {
                    temp.skillName = PlayerPrefs.GetString(sId + "_Skill" + "_" + skillId + "_name");
                    temp.skillText = PlayerPrefs.GetString(sId + "_Skill" + "_" + skillId + "_text");
                    temp.skillLvl = PlayerPrefs.GetInt(sId + "_Skill" + "_" + skillId + "_lvl");
                    temp.skillMaxLvl = PlayerPrefs.GetInt(sId + "_Skill" + "_" + skillId + "_maxLvl");
                    temp.skillExp = PlayerPrefs.GetFloat(sId + "_Skill" + "_" + skillId + "_exp");
                    temp.skillMaxExp = PlayerPrefs.GetFloat(sId + "_Skill" + "_" + skillId + "_maxExp");
                    temp.skillHurtAmp = PlayerPrefs.GetFloat(sId + "_Skill" + "_" + skillId + "_hurtAmp");
                    temp.skillHurtAdd = PlayerPrefs.GetFloat(sId + "_Skill" + "_" + skillId + "_hurtAdd");
                }
            }
        }
    }
}
