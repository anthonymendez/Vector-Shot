using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float moveSpeed;
    public float rotateSpeed;
    public float shotDelay;
    
    private float timeSinceLastShot;
    private int lasersShot;
    private GameObjectPool LaserPool;

	// Use this for initialization
	void Start () {
        lasersShot = 0;
        timeSinceLastShot = 0;
        LaserPool = GameObject.FindWithTag("Laserpool").GetComponent<GameObjectPool>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate() {
        trackInput();
        //Keep track of our time since last shot through seconds
        timeSinceLastShot += Time.deltaTime;
    }

    void trackInput() {
        //Get X and Y movements
        float x = Input.GetAxis("Horizontal"), y = Input.GetAxis("Vertical");
        //Move the player with our specified input
        transform.Translate(new Vector3(0f, y, 0f) * moveSpeed / Variables.speedDampener);

        //Rotates the players with horizontal controls. A not so hacky solution...thank god.
        if (Mathf.Abs(x) > 0.05) {
            //Calculate the rotation we're traveling in
            float atan = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
            //Apply rotation with Slerp to make the rotation more smooth
            transform.Rotate(new Vector3(0f,0f,-x)*rotateSpeed,Space.Self);
        }
        
        trackShooting();
    }

    void trackShooting() {
        bool isShooting = Input.GetButton("Fire1") || Input.GetButtonDown("Fire1");
        if (isShooting && timeSinceLastShot >= shotDelay) {
            timeSinceLastShot = 0;
            GameObject shot = LaserPool.getGameObject();
            shot.transform.rotation = transform.rotation;
            //We're going to edit the position of the shot here so it's right on our player
            Vector3 shotPosition = transform.position;
            float x = transform.rotation.eulerAngles.x,
                  y = transform.rotation.eulerAngles.y,
                  z = transform.rotation.eulerAngles.z;

            shotPosition.x += (2.2f * Mathf.Sin(z));
            shotPosition.y += (2.2f * Mathf.Cos(z));
            shotPosition.z = 1f;
            shot.transform.position = shotPosition;
        }
    }
}
