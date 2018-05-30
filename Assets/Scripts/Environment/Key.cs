using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {
    
    [SerializeField] GameObject UI;
    [SerializeField] GameObjectPool keyPool;
    [SerializeField] int pointsPerPickup = 50;
    AudioSource keyPickupSound;

    private void Start() {
        keyPool = GameObject.FindWithTag("KeyPool").GetComponent<GameObjectPool>();
        keyPickupSound = GetComponent<AudioSource>();
        UI = GameObject.FindWithTag("UI");
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            GivePlayerPoints();
            CheckIfWin();
        }
    }

    private void GivePlayerPoints() {
        Variables.score += pointsPerPickup;
        Variables.keysObtained++;
        AudioSource.PlayClipAtPoint(keyPickupSound.clip, transform.position);
        keyPool.AddGameObject(gameObject);
    }

    private void CheckIfWin() {
        if (Variables.keysObtained == 8) {
            UI.GetComponentInChildren<WinMenu>().WMUI.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
