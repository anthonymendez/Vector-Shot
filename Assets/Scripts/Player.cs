using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float moveSpeed;
    public float shotDelay;

    private GameObject Child;
    private float timeSinceLastShot;
    private int lasersShot;
    private GameObjectPool LaserPool;

	// Use this for initialization
	void Start () {
        lasersShot = 0;
        timeSinceLastShot = 0;
        Child = transform.GetChild(0).gameObject;
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
        transform.Translate(new Vector3(x, y, 0f) * moveSpeed / Variables.speedDampener);

        //Incredibly hacky way to rotate the Object without messing up the relative Translate
        //Will have to find a better solution
        if (Mathf.Abs(x) > 0.05 || Mathf.Abs(y) > 0.05) {
            //Calculate the rotation we're traveling in
            float atan = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
            //Apply rotation with Slerp to make the rotation more smooth
            Child.transform.rotation = Quaternion.Slerp(Child.transform.rotation,Quaternion.Euler(0f, 0f, atan - 90),moveSpeed*Time.deltaTime);
            
        }
        
        trackShooting();
    }

    void trackShooting() {
        bool isShooting = Input.GetButton("Fire1") || Input.GetButtonDown("Fire1");
        if (isShooting && timeSinceLastShot >= shotDelay) {
            timeSinceLastShot = 0;
            GameObject shot = LaserPool.getGameObject();
            shot.transform.rotation = Child.transform.rotation;
            //We're going to edit the position of the shot here so it's right on our player
            Vector3 shotPosition = transform.position;
            float xDirectionIntensity = Mathf.Asin(transform.rotation.eulerAngles.x / transform.rotation.eulerAngles.z);
            float yDirectionIntensity = Mathf.Acos(transform.rotation.eulerAngles.y / transform.rotation.eulerAngles.z);
            shotPosition.x += (xDirectionIntensity * 0.25f);
            shotPosition.y += (yDirectionIntensity * 0.25f);
            shot.transform.position = shotPosition;
        }
    }
}
