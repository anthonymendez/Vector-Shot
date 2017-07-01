using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float moveSpeed;
    public float shotDelay;

    private GameObject Child;
    private float timeSinceLastShot;
    private int lasersShot;

	// Use this for initialization
	void Start () {
        lasersShot = 0;
        timeSinceLastShot = 0;
        Child = transform.GetChild(0).gameObject;
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

        //Calculate the rotation we're traveling in
        float atan = Mathf.Atan2(y, x) * Mathf.Rad2Deg;

        //Incredibly hacky way to rotate the Object without messing up the relative Translate
        //Will have to find a better solution
        if (Mathf.Abs(x) > 0.05 || Mathf.Abs(y) > 0.05)
            Child.transform.rotation = Quaternion.Euler(0f, 0f, atan - 90);

        trackShooting();
    }

    void trackShooting() {
        bool isShooting = Input.GetButton("Fire1") || Input.GetButtonDown("Fire1");
        if (isShooting && timeSinceLastShot >= shotDelay) {
            timeSinceLastShot = 0;

        }
    }
}
