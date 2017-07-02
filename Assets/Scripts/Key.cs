using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

    GameObjectPool KeyPool;

    private void Start() {
        KeyPool = GameObject.FindWithTag("KeyPool").GetComponent<GameObjectPool>();
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")){
            Variables.Score += 50;
            Variables.keysObtained++;
            KeyPool.addGameObject(gameObject);
        }
    }
}
