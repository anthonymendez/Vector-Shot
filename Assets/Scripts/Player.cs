﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    
    public float moveSpeed;
    public float rotateSpeed;
    public float shotDelay;
    public bool isSpaceLike;
    
    private float timeSinceLastShot;
    private int lasersShot;
    private GameObjectPool laserPool;
    private Rigidbody2D physics;

    // Use this for initialization
    void Start () {
        lasersShot = 0;
        timeSinceLastShot = 0;
        laserPool = GameObject.FindWithTag("Laserpool").GetComponent<GameObjectPool>();
        physics = GetComponent<Rigidbody2D>();
    }
    
    // Update is called once per frame
    void Update () {
        
    }

    void FixedUpdate() {
        TrackInput();
        //Keep track of our time since last shot through seconds
        timeSinceLastShot += Time.deltaTime;
    }

    void TrackInput() {
        //Get X and Y movements
        float x = Input.GetAxis("Horizontal"), y = Input.GetAxis("Vertical");
        Transform tempTrans = transform;
        if (isSpaceLike) {
            //Vertical to move back and forth
            //Horizontal to move left and right
            tempTrans.Translate(new Vector3(0f, y, 0f) * moveSpeed / Variables.speedDampener);
            float zRot = transform.rotation.eulerAngles.z;
            physics.MovePosition(tempTrans.position);
            physics.MoveRotation(zRot - x * rotateSpeed / Variables.speedDampener);
        } else {
            //Vertical and Horizontal to move in all directions
            Vector2 movement = new Vector2(x,y);
            //Set rotation to 0 so we move relative to camera
            tempTrans.rotation = Quaternion.Euler(0f, 0f, 0f);
            tempTrans.Translate(movement * moveSpeed / Variables.speedDampener);

            physics.MovePosition(tempTrans.position);

            //Mouse position to aim our ship
            float cameraDistance = Variables.mainCamera.transform.position.y - transform.position.y;
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Variables.mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraDistance));

            float angleRadian = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x);
            float angleDegrees = Mathf.Rad2Deg * angleRadian;

            transform.rotation = Quaternion.Euler(0f, 0f, angleDegrees-90);
        }

        TrackShooting();
    }

    void TrackShooting() {
        bool isShooting = Input.GetButton("Fire1") || Input.GetButtonDown("Fire1");
        if (isShooting && timeSinceLastShot >= shotDelay) {
            timeSinceLastShot = 0;
            GameObject shot = laserPool.GetGameObject();
            shot.transform.rotation = transform.rotation;
            //We're going to edit the position of the shot here so it's right in front our player
            shot.transform.position = transform.position;
            shot.transform.Translate(0f,2.5f,0f);
        }
    }

}
