using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Laser : MonoBehaviour {

    public LaserOrigin laserOrigin;
    public int shotFromPlayer = -1;

    [SerializeField] AudioClip shipExploding, hitShield;
    [SerializeField] float moveSpeed;
    [SerializeField] List<string> collidableGameObjectTags = new List<string>() {
        "LaserShot", "MEnemy", "REnemy", "Wall", "Player", "Shield"
    };

    GameObjectPool laserPool, meleeEnemyPool, rangedEnemyPool;
    Rigidbody2D physics;

    void Awake() {
        laserPool = GameObject.FindWithTag("Laserpool").GetComponent<GameObjectPool>();
        meleeEnemyPool = GameObject.FindWithTag("MEnemyPool").GetComponent<GameObjectPool>();
        rangedEnemyPool = GameObject.FindWithTag("REnemyPool").GetComponent<GameObjectPool>();
        physics = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start () {
        laserPool = GameObject.FindWithTag("Laserpool").GetComponent<GameObjectPool>();
        meleeEnemyPool = GameObject.FindWithTag("MEnemyPool").GetComponent<GameObjectPool>();
        rangedEnemyPool = GameObject.FindWithTag("REnemyPool").GetComponent<GameObjectPool>();
        physics = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        //Much better way of calculating movement
        Transform temp = transform;
        temp.Translate(new Vector3(0f, 1f, 0f) * moveSpeed / Variables.speedDampener);
        physics.MovePosition(temp.position);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        //Check if colliding object is laser, if so, each laser 
        //calls this method and adds themselves to the pool
        if (laserPool == null) {
            laserPool = GameObject.FindWithTag("Laserpool").GetComponent<GameObjectPool>();
        }
        if (HitCollidableObject(collision)) {

            string collisionTag = collision.gameObject.tag;
            if (collisionTag.Equals("REnemy") || collisionTag.Equals("MEnemy")) {
                HandleEnemyCollision(collision, collisionTag);
            } else if (collisionTag.Equals("Shield")) {
                HandleShieldCollision(collision);
            } else if (collisionTag.Equals("Player")) {
                HandlePlayerCollision(collision);
            } else {
                AddToLaserPool();
            }
        } 
    }

    private void HandleEnemyCollision(Collision2D collision, string collisionTag) {
        if (laserOrigin.IsLaserFromEnemy()) {
            return;
        }

        AddToLaserPool();
        PerformDestroyEnemy();

        bool isREnemy = collisionTag.Equals("REnemy");
        if (isREnemy) {
            rangedEnemyPool.AddGameObject(collision.gameObject);
        } else {
            meleeEnemyPool.AddGameObject(collision.gameObject);
        }
    }

    private void HandleShieldCollision(Collision2D collision) {
        Shield shieldBeingHit = collision.gameObject.GetComponent<Shield>();
        Player shieldOwner = shieldBeingHit.transform.parent.GetComponent<Player>();
        int playerNumber = shieldOwner.GetPlayerNumber();

        if(playerNumber != shotFromPlayer) {
            AddToLaserPool();
            AudioSource.PlayClipAtPoint(hitShield, transform.position);
            shieldBeingHit.DamageShield(1);
        } else {
            // Ignore if we hit and were shot from the same player
        }
    }
    private void HandlePlayerCollision(Collision2D collision) {
        int playerNumber = collision.gameObject.GetComponent<Player>().GetPlayerNumber();

        if (playerNumber != shotFromPlayer) {
            AddToLaserPool();
            AudioSource.PlayClipAtPoint(shipExploding, transform.position);
            collision.gameObject.SetActive(false);
        } else {
            // Ignore if we hit and were shot from the same player
        }
    }

    private void PerformDestroyEnemy() {
        AudioSource.PlayClipAtPoint(shipExploding, transform.position);
        Variables.score += 10;
    }

    private bool HitCollidableObject(Collision2D collision) {
        string collisionTag = collision.gameObject.tag;
        foreach (string tag in collidableGameObjectTags) {
            if (collisionTag.Equals(tag))
                return true;
        }
        return false;
    }

    private void AddToLaserPool() {
        laserPool.AddGameObject(gameObject);
    }
}
