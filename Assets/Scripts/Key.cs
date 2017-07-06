using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

    public GameObject[] ignoreCollisions;

    public GameObject WinMenu;

    GameObjectPool keyPool;

    private void Start() {
        keyPool = GameObject.FindWithTag("KeyPool").GetComponent<GameObjectPool>();

        foreach (GameObject gObj in ignoreCollisions) {

        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")){
            Variables.score += 50;
            Variables.keysObtained++;
            keyPool.AddGameObject(gameObject);

            if(Variables.keysObtained == 8) {
                WinMenu.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }
}
