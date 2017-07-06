using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    public float moveSpeed;
    public bool isFriendly;

    GameObjectPool laserPool, meleeEnemyPool, rangedEnemyPool;
    Player player;
    Rigidbody2D physics;

    void Awake() {
        laserPool = GameObject.FindWithTag("Laserpool").GetComponent<GameObjectPool>();
        meleeEnemyPool = GameObject.FindWithTag("MEnemyPool").GetComponent<GameObjectPool>();
        rangedEnemyPool = GameObject.FindWithTag("REnemyPool").GetComponent<GameObjectPool>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
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
        string tag = collision.gameObject.tag;
        //Check if colliding object is laser, if so, each laser 
        //calls this method and adds themselves to the pool
        if (laserPool == null) {
            laserPool = GameObject.FindWithTag("Laserpool").GetComponent<GameObjectPool>();
        }

        if (collision.gameObject.CompareTag("Wall")) {
            if (isFriendly) {
                player.shotsAvailable++;
            }
            laserPool.AddGameObject(gameObject);
        } else if (collision.gameObject.CompareTag("REnemy") && isFriendly) {
            player.shotsAvailable++;
            rangedEnemyPool.AddGameObject(collision.gameObject);
            Variables.score += 10;
            laserPool.AddGameObject(gameObject);
        } else if (collision.gameObject.CompareTag("MEnemy") && isFriendly) {
            player.shotsAvailable++;
            meleeEnemyPool.AddGameObject(collision.gameObject);
            Variables.score += 10;
            laserPool.AddGameObject(gameObject);
        } else if (collision.gameObject.CompareTag("Player") && !isFriendly) {
            collision.gameObject.SetActive(false);
            laserPool.AddGameObject(gameObject);
        }
    }
}
