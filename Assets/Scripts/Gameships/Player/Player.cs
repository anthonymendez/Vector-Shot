using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [Header("Movement Properties")]
    [SerializeField] float moveSpeed = 35f;
    [SerializeField] float rotateSpeed = 500f;

    [Header("Shooting Properties")]
    [SerializeField] float shotDelay = 0.25f;
    [SerializeField] float reloadTime = 0.5f;
    [SerializeField] int shotLimit = 5;

    [Header("Shield Properties")]
    [SerializeField] float shieldRegenTime = 5f;
    [SerializeField] GameObject shieldGameObject;

    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject activeShots;
    [SerializeField] GameObjectPool laserPool;
    
    private Shield shield;
    private AudioSource laserShootSound, reloadSound;
    private Rigidbody2D physics;

    private int shotsAvailable = 5;
    bool isReloading;
    float timeSinceLastShot;
    float reloadingTime;
    float shieldRechargingTime;

    public int GetShotsAvailable() {
        return shotsAvailable;
    }

    // Use this for initialization
    void Start () {
        reloadingTime = 0;
        timeSinceLastShot = 0;
        shieldRechargingTime = 0;
        laserShootSound = GetComponents<AudioSource>()[0];
        reloadSound = GetComponents<AudioSource>()[1];
        physics = GetComponent<Rigidbody2D>();
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        shield = shieldGameObject.GetComponent<Shield>();
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
        ProcessShieldStatus();
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

    private void ProcessShieldStatus() {
        if (shield.ShieldAvailable()) {
            UseShield();
        } else {
            ShieldRecharge();
        }
    }

    private void UseShield() {
        bool activateShield = Input.GetButton("Fire2");

        if (activateShield) {
            shieldGameObject.SetActive(true);
        } else {
            shieldGameObject.SetActive(false);
        }
    }

    private void ShieldRecharge() {
        shieldGameObject.SetActive(false);
        shieldRechargingTime += Time.deltaTime;

        if (shieldRechargingTime >= shieldRegenTime) {
            shield.RegenShield();
            shieldRechargingTime = 0f;
        }
    }

    private void ProcessShooting() {
        if (shieldGameObject.activeSelf) {
            return;
        }

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
                shotsAvailable = shotLimit;
            } else {
                reloadingTime += Time.deltaTime;
            }
        }
    }

}
