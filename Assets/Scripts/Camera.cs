using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

    GameObject player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        checkDead();
	}

    //If player is dead, we're going to end the game and bring up the menu
    void checkDead() {
        if (!player.activeSelf) {
            //Debug.Log("You're dead kiddo");
        }
    }
}
