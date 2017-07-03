﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour {

    GameObject player;

    Vector3 offset;
    Vector3 velocity;
    

    // Use this for initialization
    void Start () {
        player = GameObject.FindWithTag("Player");
        offset = transform.position - player.transform.position;
        velocity = Vector3.zero;
    }

    void Update() {
        CheckDead();
    }

    // Updates regardless of Framerate
    void FixedUpdate () {
        transform.position = Vector3.SmoothDamp(transform.position,player.transform.position,ref velocity,0.25f, 50.0f, Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
    }

    //If player is dead, we're going to end the game and bring up the menu
    void CheckDead() {
        if (!player.activeSelf) {
            //Debug.Log("You're dead kiddo");
        }
    }
}