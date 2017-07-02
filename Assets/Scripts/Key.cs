using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

    GameObjectPool keyPool;

    private void Start() {
        keyPool = GameObject.FindWithTag("KeyPool").GetComponent<GameObjectPool>();
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")){
            Variables.score += 50;
            Variables.keysObtained++;
            keyPool.AddGameObject(gameObject);
        }
    }
}
