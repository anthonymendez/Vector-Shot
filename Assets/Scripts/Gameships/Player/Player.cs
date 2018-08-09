using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, Bonkable {

    [Header("Control Properties")]
    [SerializeField] string moveHorizontalInputName = "Horizontal";
    [SerializeField] string moveVerticalInputName = "Vertical";
    [SerializeField] string lookHorizontalInputName = "LookHorizontal";
    [SerializeField] string lookVerticalInputName = "LookVertical";
    [SerializeField] bool disableKeyboardAndMouse = false;
    [SerializeField] float lookStickDeadzone = 0.2f;
    [SerializeField] string shootLaserInputName = "Fire1";
    [SerializeField] string useShieldInputName = "Fire2";
    [SerializeField] string reloadLasersInputName = "ReloadAmmo";
    [SerializeField] string pauseInputName = "Start";
    bool isBonked = false;
    Vector3 oldMousePosition = Vector3.zero;
    Quaternion oldRotation = Quaternion.identity;


    [Header("Multiplayer Properties")]
    [SerializeField] int playerNumber = 1;
    [SerializeField] public Color shipColor = Color.white;

    [Header("Movement Properties")]
    [SerializeField] float moveSpeed = 35f;

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
    private SpriteRenderer spriteRenderer;

    private int shotsAvailable = 5;
    bool isReloading;
    float timeSinceLastShot;
    float reloadingTime;
    float shieldRechargingTime;

    public int GetPlayerNumber() {
        return playerNumber;
    }

    public void SetPlayerNumber(int pN) {
        playerNumber = pN;
    }

    public Color GetColor() {
        return shipColor;
    }

    public void SetColor(Color newColor) {
        shipColor = newColor;
        spriteRenderer.color = newColor;
    }

    public int GetShotLimit() {
        return shotLimit;
    }

    public int GetShotsAvailable() {
        return shotsAvailable;
    }

    public void SetBonked(bool isBonked) {
        this.isBonked = isBonked;
    }

    // Use this for initialization
    void Awake () {
        InitializePlayerComponents();
        InitializePlayerValues();
    }

    private void InitializePlayerComponents() {
        laserShootSound = GetComponents<AudioSource>()[0];
        reloadSound = GetComponents<AudioSource>()[1];
        physics = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>()[0];
        activeShots = GameObject.FindWithTag("ActiveLaser");
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        laserPool = GameObject.FindWithTag("Laserpool").GetComponent<GameObjectPool>();
        shield = shieldGameObject.GetComponent<Shield>();
    }

    private void InitializePlayerValues() {
        reloadingTime = 0;
        timeSinceLastShot = 0;
        shieldRechargingTime = 0;
        spriteRenderer.color = shipColor;
    }

    // Update is called once per frame
    void Update () {
        ProcessPausing();
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

    private void ProcessPausing() {
        bool pressedPause = Input.GetButtonDown(pauseInputName.ToInputConverter(playerNumber));

        if (!pressedPause)
            return;

        if(Time.timeScale == 0f) {
            Time.timeScale = 1f;
        } else {
            Time.timeScale = 0f;
        }
    }

    private void ProcessTranslation() {
        //Get X and Y movements
        float moveX = Input.GetAxis(moveHorizontalInputName.ToInputConverter(playerNumber)), 
              moveY = Input.GetAxis(moveVerticalInputName.ToInputConverter(playerNumber));
        Transform newTransform = transform;
        //Vertical and Horizontal to move in all directions
        Vector2 movement = new Vector2(moveX, moveY);
        //Set rotation to 0 so we move relative to camera
        newTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
        newTransform.Translate(movement * moveSpeed / Variables.speedDampener);

        physics.MovePosition(newTransform.position);
    }

    private void ProcessRotation() {
        float xLook = Input.GetAxis(lookHorizontalInputName.ToInputConverter(playerNumber));
        float yLook = Input.GetAxis(lookVerticalInputName.ToInputConverter(playerNumber));
        bool usingLookStick = (Math.Abs(xLook) > lookStickDeadzone) || (Math.Abs(yLook) > lookStickDeadzone);

        Vector3 newMousePosition = Input.mousePosition;
        bool mouseIsMoving = newMousePosition != oldMousePosition;

        if (!disableKeyboardAndMouse && !usingLookStick && mouseIsMoving) {
            //Mouse position to aim our ship
            Debug.Log(string.Format("Using Keyboard and Mouse Controls on P{0}", playerNumber));
            float cameraDistance = mainCamera.transform.position.y - transform.position.y;
            Vector3 mousePosition = newMousePosition;
            mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraDistance));

            float angleRadian = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x);
            float angleDegrees = Mathf.Rad2Deg * angleRadian;

            transform.rotation = Quaternion.Euler(0f, 0f, angleDegrees - 90);
            oldRotation = transform.rotation;
        } else if (usingLookStick){
            // Use stick direction to aim our ship
            Debug.Log(string.Format("Using Look Stick Controls on P{0}", playerNumber));
            float angleRadian = Mathf.Atan2(yLook, -xLook);
            float angleDegrees = Mathf.Rad2Deg * angleRadian;

            transform.rotation = Quaternion.Euler(0f, 0f, angleDegrees - 90);
            oldRotation = transform.rotation;
        } else {
            transform.rotation = oldRotation;
        }
    }

    private void ProcessShieldStatus() {
        if (shield.ShieldAvailable()) {
            UseShield();
        } else {
            ShieldRecharge();
        }
    }

    private void UseShield() {
        bool activateShield = Input.GetButton(useShieldInputName.ToInputConverter(playerNumber));

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
        bool reloadKeyDown = Input.GetButtonDown(reloadLasersInputName.ToInputConverter(playerNumber));
        if (reloadKeyDown) {
            isReloading = true;
            reloadSound.Play();
            reloadingTime = 0;
        }

        if (shieldGameObject.activeSelf) {
            return;
        }

        bool isShooting = Input.GetButton(shootLaserInputName.ToInputConverter(playerNumber)) || 
                          Input.GetButtonDown(shootLaserInputName.ToInputConverter(playerNumber));
        if (!isReloading && isShooting && shotsAvailable > 0 && timeSinceLastShot >= shotDelay) {
            timeSinceLastShot = 0;
            GameObject shot = laserPool.GetGameObject();
            shot.GetComponent<Laser>().laserOrigin = LaserOrigin.Player;
            shot.GetComponent<Laser>().shotFromPlayer = playerNumber;
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
