using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    
    public float moveSpeed;
    public float rotateSpeed;
    public float shotDelay;
    
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
        //Move the player with our specified input
        Transform tempTrans = transform;
        tempTrans.Translate(new Vector3(0f, y, 0f) * moveSpeed / Variables.speedDampener);
        float zRot = transform.rotation.eulerAngles.z;
        physics.MovePosition(tempTrans.position);
        physics.MoveRotation(zRot - x * rotateSpeed / Variables.speedDampener);   
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
