using UnityEngine;
using System.Collections;

public class FastMoveSpeed : MonoBehaviour {

    public float addSpeed = 1f;

    void OnTriggerEnter2D(Collider2D other) {
        Move m = other.gameObject.GetComponent<Move>();
        if (m!= null) {
            m.moveSpeed += addSpeed;
        }
    }
    void OnTriggerExit2D(Collider2D other) {
        Move m = other.gameObject.GetComponent<Move>();
        if (m != null) {
            m.moveSpeed -= addSpeed;
        }
    }
}
