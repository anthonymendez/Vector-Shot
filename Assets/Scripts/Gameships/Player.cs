using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] float moveSpeed;
    [SerializeField] float rotateSpeed;
    [SerializeField] float shotDelay;
    [SerializeField] float reloadTime;
    [SerializeField] Camera mainCamera;
    [SerializeField] int shotsOnMap;
    [SerializeField] int shotsAvailable;
    [SerializeField] GameObject activeShots;
    [SerializeField] GameObjectPool laserPool;

    float timeSinceLastShot;
    Rigidbody2D physics;
    AudioSource laserShootSound, reloadSound;
    bool isReloading;
    float reloadingTime;

    public int GetShotsAvailable() {
        return shotsAvailable;
    }

    // Use this for initialization
    void Start () {
        reloadingTime = 0;
        timeSinceLastShot = 0;
        laserShootSound = GetComponents<AudioSource>()[0];
        reloadSound = GetComponents<AudioSource>()[1];
        physics = GetComponent<Rigidbody2D>();
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        Time.timeScale = 1;
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
        ProcessTranslation();
        ProcessRotation();
        ProcessShooting();
    }

    private void ProcessTranslation() {
        //Get X and Y movements
        float moveX = Input.GetAxis("Horizontal"), 
              moveY = Input.GetAxis("Vertical");
        Transform newTrasnfrom = transform;
        //Vertical and Horizontal to move in all directions
        Vector2 movement = new Vector2(moveX, moveY);
        //Set rotation to 0 so we move relative to camera
        newTrasnfrom.rotation = Quaternion.Euler(0f, 0f, 0f);
        newTrasnfrom.Translate(movement * moveSpeed / Variables.speedDampener);

        physics.MovePosition(newTrasnfrom.position);
    }

    private void ProcessRotation() {
        //Mouse position to aim our ship
        float cameraDistance = mainCamera.transform.position.y - transform.position.y;
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraDistance));

        float angleRadian = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x);
        float angleDegrees = Mathf.Rad2Deg * angleRadian;

        transform.rotation = Quaternion.Euler(0f, 0f, angleDegrees - 90);
    }

    void ProcessShooting() {
        bool reloadKeyDown = Input.GetKeyDown(KeyCode.R);
        if (reloadKeyDown) {
            isReloading = true;
            reloadSound.Play();
            reloadingTime = 0;
        }

        bool isShooting = Input.GetButton("Fire1") || Input.GetButtonDown("Fire1");
        if (!isReloading && isShooting && shotsAvailable > 0 && timeSinceLastShot >= shotDelay) {
            timeSinceLastShot = 0;
            GameObject shot = laserPool.GetGameObject();
            shot.GetComponent<Laser>().isFriendly = true;
            shot.transform.rotation = transform.rotation;
            //We're going to edit the position of the shot here so it's right in front our player
            shot.transform.position = transform.position;
            shot.transform.Translate(0f,2.5f,0f);
            shot.transform.parent = activeShots.transform;
            laserShootSound.Play();
            shotsAvailable--;
        } else if (isReloading) {
            if (reloadingTime > reloadTime) {
                isReloading = false;
                shotsAvailable = shotsOnMap;
            } else {
                reloadingTime += Time.deltaTime;
            }
        }
    }

}
