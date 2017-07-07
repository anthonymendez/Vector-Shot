using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

    public GameObject[] ignoreCollisions;

    public GameObject WinMenu;

    GameObjectPool keyPool;
    AudioSource keyPickupSound;

    private void Start() {
        keyPool = GameObject.FindWithTag("KeyPool").GetComponent<GameObjectPool>();
        keyPickupSound = GetComponent<AudioSource>();
        foreach (GameObject gObj in ignoreCollisions) {

        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")){
            Variables.score += 50;
            Variables.keysObtained++;
            AudioSource.PlayClipAtPoint(keyPickupSound.clip,transform.position);
            keyPool.AddGameObject(gameObject);
            if(Variables.keysObtained == 8) {
                WinMenu.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }
}
