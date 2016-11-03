using UnityEngine;
using System.Collections;

public class GameData {
    public GameNpc npc;
    public int nowId;
    public int allIdCount;
    private MainGame mainGame;

    public GameData(MainGame mainGame,GameNpc npc) {
        this.mainGame = mainGame;
        this.npc = npc;
        this.allIdCount = PlayerPrefs.GetInt("AllIDCount");
    }

    public void init() {
        int nowId = PlayerPrefs.GetInt("nowId");
        if (nowId != 0) {
            if (LoadAndSave.loadNpc(this.npc, nowId) == 0) {
                mainGame.firstBegin();
            }
        } else {
            mainGame.firstBegin();
        }

    }
}

