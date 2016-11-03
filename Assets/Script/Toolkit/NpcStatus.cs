using UnityEngine;
using System.Collections;

public class NpcStatus {
    public static string getStatus(GameNpc npc) {
        Color color = Color.black;
        string str;
        //LVL
        color.r = 1f - npc.lvl / npc.maxLvl;
        color.g = 1f - npc.lvl / npc.maxLvl;
        color.b = 1f - npc.lvl / npc.maxLvl;
        str = "等级​:"
            + "<color=" + ColorToHex.colorToHex(color) + ">" + npc.lvl + "</color>" + "/" + npc.maxLvl;
        //EXP
        color.r = 1f - npc.exp / npc.maxExp;
        color.g = 1f - npc.exp / npc.maxExp;
        color.b = 1f - npc.exp / npc.maxExp;
        str += "(" + npc.exp + "/" + npc.maxExp + ")\n";
        //HP
        color.r = 1f - npc.hp / npc.maxHp;
        color.g = npc.hp / npc.maxHp;
        color.b = 0f;
        str += "HP:"
            + "<color=" + ColorToHex.colorToHex(color) + ">" + npc.hp + "</color>" + "/" + npc.maxHp + "    ";
        //MP
        color.r = 1f - npc.mp / npc.maxMp;
        color.g = npc.mp / npc.maxMp;
        color.b = 0f;
        str += " MP :"
            + "<color=" + ColorToHex.colorToHex(color) + ">" + npc.mp + "</color>" + "/" + npc.maxMp + "\n";
        //物理攻击
        str += " 物理伤害 :"
            + "<color=#FF6400FF>" + npc.pAttLow + "</color>" + "/" + npc.pAttHigh + "    ";
        //法术攻击
        str += " 法术伤害 :"
            + "<color=#3D00FFFF>" + npc.mAttLow + "</color>" + "/" + npc.mAttHigh + "\n";
        //物理防御
        str += " 物理防御 :"
            + "<color=#FF6400FF>" + npc.pDefPower + "</color>    ";
        //法术防御
        str += " 法术防御 :"
            + "<color=#3D00FFFF>" + npc.mDefPower + "</color>\n";
        //力量
        color.r = 1f;
        color.g = npc.liliang / npc.lvl * 50;
        color.b = 1f;
        str += " 力量 :"
            + "<color=" + ColorToHex.colorToHex(color) + ">" + npc.liliang + "</color>" + "    ";
        //敏捷
        color.r = 1f;
        color.g = npc.minjie / npc.lvl * 50;
        color.b = 1f;
        str += " 敏捷 :"
            + "<color=" + ColorToHex.colorToHex(color) + ">" + npc.minjie + "</color>" + "    ";
        //智力
        color.r = 1f;
        color.g = npc.zhili / npc.lvl * 50;
        color.b = 1f;
        str += " 智力 :"
            + "<color=" + ColorToHex.colorToHex(color) + ">" + npc.zhili + "</color>" + "    ";
        //体质
        color.r = 1f;
        color.g = npc.tizhi / npc.lvl * 50;
        color.b = 1f;
        str += " 体质 :"
            + "<color=" + ColorToHex.colorToHex(color) + ">" + npc.tizhi + "</color>" + "\n";
        //攻击速度
        color = Color.yellow;
        str += " 速度 :"
            + "<color=" + ColorToHex.colorToHex(color) + ">" + npc.norFightSpeed + "</color>" + "+" + "<color=#FF0000FF>" + (npc.fightSpeedAmp * npc.norFightSpeed) + "</color>    ";
        //闪避概率
        str += " 闪避几率 :"
            + "<color=#00DB89FF>" + npc.doeOdds + "</color>    ";
        //物理暴击概率
        str += " 物暴几率 :"
            + "<color=#FF6400FF>" + npc.pVioHurtOdds + "</color>\n";
        //物理暴击加成
        str += " 物暴伤害加成 :"
            + "<color=#FF6400FF>" + npc.pVioHurtAmp + "</color>    ";
        //物理暴击附加
        str += " 物暴伤害附加 :"
            + "<color=#FF6400FF>" + npc.pVioHurtAdd + "</color>\n";
        //物理暴击时无视防御力几率
        str += " 物暴无视防御几率 :"
            + "<color=#FF6400FF>" + npc.pVioHurtMissDefOdds + "</color>\n";
        //物理暴击时无视防御力比例
        str += " 物暴无视防御比例 :"
            + "<color=#FF6400FF>" + npc.pVioHurtMissDefAmp + "</color>    ";
        //物理暴击时无视防御力附加
        str += " 物暴无视防御附加 :"
            + "<color=#FF6400FF>" + npc.pVioHurtMissDefAdd + "</color>\n";
        //每秒回复HP
        str += " 每秒回复HP :"
            + "<color=#00FF00FF>" + npc.recoverHpPerSecond + "</color>+" + "<color=#FF0000FF>" + (npc.autoHeal * npc.maxHp / 100f) + "</color>    ";
        //击中回复HP
        str += " 每秒回复HP :"
            + "<color=#00FF00FF>" + npc.recoverHpPerHit + "</color>\n";
        //每秒回复MP
        str += " 每秒回复MP :"
            + "<color=#3D00FFFF>" + npc.recoverMpPerSecond + "</color>+" + "<color=#FF0000FF>" + (npc.autoMuse * npc.maxMp / 100f) + "</color>    ";
        //击中回复MP
        str += " 每秒回复MP :"
            + "<color=#3D00FFFF>" + npc.recoverMpPerHit + "</color>\n";

        return str;
    }
}
