using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cam : MonoBehaviour {

    [Header("Camera Follow Properties")]
    [Tooltip("If marked, camera will not move.")] [SerializeField] bool isStaticCamera = false;
    [Tooltip("Amount of time to center camera on player.\nUnits in seconds.\nRecommended: 0.01f")][SerializeField] float smoothTime = 0.01f;
    [Tooltip("Max Camera Follow Speed.\nRecommended: 75.0f")] [SerializeField] float maxSpeed = 75.0f;
    [Tooltip("Recommended: -10f\nDo not set to or above 0f")] [SerializeField] float zIndexPostion = -10f;

    [Header("Camera Shake Properties")]
    [SerializeField] float maxAngleToShake = 1f;
    [SerializeField] float maxOffsetToShake = 1f;

    [Header("Prefabs")]
    [SerializeField] GameObject deathMenuPrefab;

    [Header("System Variables")]
    [SerializeField] long seed = -1000;
    private float currentTraumaLevel = 0f;
    private GameObject[] playerArray;
    private Vector3 velocity;
    private bool checkDied;
    private Vector3 basePosition;
    private Vector3 baseAngle;

    void Awake() {

    }

    // Use this for initialization
    void Start () {
        velocity = Vector3.zero;
        checkDied = false;
        basePosition = transform.position;
        baseAngle = transform.eulerAngles;
    }

    void Update() {
        CheckDead();
    }

    // Updates regardless of Framerate
    void FixedUpdate () {
        if (!isStaticCamera) {
            SmoothFollowPlayer();
        }
        //ApplyCameraShake();
        //UpdateTraumaLevel();
    }

    private void SmoothFollowPlayer() {
        transform.position = Vector3.SmoothDamp(transform.position, playerArray[0].transform.position, ref velocity, smoothTime, maxSpeed, Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, zIndexPostion);
    }

    //If player is dead, we're going to end the game and bring up the menu
    void CheckDead() {
        //If the player is dead...
        playerArray = GameObject.FindGameObjectsWithTag("Player");
        List<int> playersAlive = new List<int>();
        if (playerArray.Length >= 1) {
            int countAlive = 0;
            foreach (GameObject player in playerArray) {
                if (player.activeSelf) {
                    playersAlive.Add(player.GetComponent<Player>().GetPlayerNumber());
                    countAlive++;
                }
            }
            Debug.Log(string.Format("AmountOfPlayersAlive: {0}", countAlive));
            if(countAlive <= 1) {
                Helper.playerScores[playersAlive[0] - 1]++;
                SceneManager.LoadScene(1);
            }
        }
        //if (isDead() && !checkDied) {
        //    Time.timeScale = 0;
        //    checkDied = true;
        //    CreateDeathMenu();
        //}
    }

    private void CreateDeathMenu() {
        Transform UI = GameObject.FindWithTag("UI").transform;
        Instantiate<GameObject>(deathMenuPrefab, UI, false);
    }

    public bool IsDead() {
        return !playerArray[0].activeSelf;
    }

    public void AddCameraShake(float trauma) {
        currentTraumaLevel += trauma;

        if (currentTraumaLevel > 1.0f) {
            currentTraumaLevel = 1.0f;
        }
    }

    private void ApplyCameraShake() {
        
        float angle = maxAngleToShake * Mathf.Pow(currentTraumaLevel, 2) * Mathf.PerlinNoise(Time.deltaTime, Time.deltaTime);
        float xOffset = maxOffsetToShake * Mathf.Pow(currentTraumaLevel, 2) * Mathf.PerlinNoise(Time.deltaTime, Time.deltaTime);
        float yOffset = maxOffsetToShake * Mathf.Pow(currentTraumaLevel, 2) * Mathf.PerlinNoise(Time.deltaTime, Time.deltaTime);

        //transform.position = basePosition + new Vector3(xOffset, yOffset, 0f);
        transform.eulerAngles = new Vector3(baseAngle.x, baseAngle.y, baseAngle.z + angle);
    }

    private void UpdateTraumaLevel() {
        if (currentTraumaLevel > 0f) {
            currentTraumaLevel -= 0.0005f;
        } else if (currentTraumaLevel < 0f) {
            currentTraumaLevel = 0f;
        }
    }

}
