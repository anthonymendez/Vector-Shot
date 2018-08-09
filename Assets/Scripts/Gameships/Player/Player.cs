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
        Debug.Log("Changing player number to: " + pN);
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
        bool pressedPause = ButtonOrAxisDown(pauseInputName);

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
        float moveX = GetAxis(moveHorizontalInputName), 
              moveY = GetAxis(moveVerticalInputName);
        Transform newTransform = transform;
        //Vertical and Horizontal to move in all directions
        Vector2 movement = new Vector2(moveX, moveY);
        //Set rotation to 0 so we move relative to camera
        newTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
        newTransform.Translate(movement * moveSpeed / Variables.speedDampener);

        physics.MovePosition(newTransform.position);
    }

    private void ProcessRotation() {
        float xLook = GetAxis(lookHorizontalInputName);
        float yLook = GetAxis(lookVerticalInputName);
        Debug.Log(string.Format("Rotating: {0}", lookHorizontalInputName.ToInputConverter(playerNumber)));
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
        Debug.Log(string.Format("Using Shield: {0}", useShieldInputName.ToInputConverter(playerNumber)));
        bool activateShield = ButtonOrAxis(useShieldInputName);
        shieldGameObject.SetActive(activateShield);
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
        bool reloadKeyDown = ButtonOrAxisDown(reloadLasersInputName);
        if (reloadKeyDown) {
            isReloading = true;
            reloadSound.Play();
            reloadingTime = 0;
        }

        bool isShooting = ButtonOrAxis(shootLaserInputName);
        bool shieldIsNotActive = !shieldGameObject.activeSelf;
        if (!isReloading && shieldIsNotActive && isShooting && shotsAvailable > 0 && timeSinceLastShot >= shotDelay) {
            Debug.Log(string.Format("Shooting from: {0}", shootLaserInputName.ToInputConverter(playerNumber)));
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

    private float GetAxis(string inputName) {
        Debug.Log("GetAxis: " + playerNumber);
        return Input.GetAxis(inputName.ToInputConverter(playerNumber));
    }

    private float GetAxisRaw(string inputName) {
        Debug.Log("GetAxisRaw: " + playerNumber);
        return Input.GetAxisRaw(inputName.ToInputConverter(playerNumber));
    }

    private bool ButtonOrAxisDown(string inputName) {
        Debug.Log("ButtonOrAxisDown: " + playerNumber);
        return Input.GetButtonDown(inputName.ToInputConverter(playerNumber)) ||
               GetAxisRaw(inputName) > Mathf.Epsilon;
    }

    private bool ButtonOrAxis(string inputName) {
        Debug.Log("ButtonOrAxis: " + playerNumber);
        return Input.GetButton(inputName.ToInputConverter(playerNumber)) ||
               ButtonOrAxisDown(inputName);
    }

}
