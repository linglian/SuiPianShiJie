using UnityEngine;
using System.Collections;

public class AutoHeal : MonoBehaviour {
    public float healOdds = 1f;

    void OnTriggerEnter2D(Collider2D other) {
        GameNpc npc = other.gameObject.GetComponent<GameNpc>();
        if (npc != null) {
            npc.autoHeal += healOdds;
        }
    }
    void OnTriggerExit2D(Collider2D other) {
        GameNpc npc = other.gameObject.GetComponent<GameNpc>();
        if (npc != null) {
            npc.autoHeal -= healOdds;
        }
    }
}
