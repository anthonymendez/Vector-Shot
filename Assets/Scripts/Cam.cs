using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour {

    [Header("Camera Follow Properties")]
    [Tooltip("If marked, camera will not move.")] [SerializeField] bool isStaticCamera = false;
    [Tooltip("Amount of time to center camera on player.\nUnits in seconds.\nRecommended: 0.01f")][SerializeField] float smoothTime = 0.01f;
    [Tooltip("Max Camera Follow Speed.\nRecommended: 75.0f")] [SerializeField] float maxSpeed = 75.0f;
    [Tooltip("Recommended: -10f\nDo not set to or above 0f")] [SerializeField] float zIndexPostion = -10f;

    [Header("Prefabs")]
    [SerializeField] GameObject deathMenuPrefab;

    [Header("System Variables")]
    private GameObject player;
    private Vector3 velocity;
    private bool checkDied;

    // Use this for initialization
    void Start () {
        player = GameObject.FindWithTag("Player");
        velocity = Vector3.zero;
        checkDied = false;
    }

    void Update() {
        CheckDead();
    }

    // Updates regardless of Framerate
    void FixedUpdate () {
        if (!isStaticCamera) {
            SmoothFollowPlayer();
        }
    }

    private void SmoothFollowPlayer() {
        transform.position = Vector3.SmoothDamp(transform.position, player.transform.position, ref velocity, smoothTime, maxSpeed, Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, zIndexPostion);
    }

    //If player is dead, we're going to end the game and bring up the menu
    void CheckDead() {
        //If the player is dead...
        if (isDead() && !checkDied) {
            Time.timeScale = 0;
            checkDied = true;
            CreateDeathMenu();
        }
    }

    private void CreateDeathMenu() {
        Transform UI = GameObject.FindWithTag("UI").transform;
        Instantiate<GameObject>(deathMenuPrefab, UI, false);
    }

    public bool isDead() {
        return !player.activeSelf;
    }
}
