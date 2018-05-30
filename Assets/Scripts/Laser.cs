using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    public const int ID = 0;
    public float moveSpeed;
    public bool isFriendly;

    GameObjectPool laserPool, meleeEnemyPool, rangedEnemyPool;
    Player player;
    Rigidbody2D physics;
    AudioSource shipExplosionSound;

    void Awake() {
        shipExplosionSound = GetComponent<AudioSource>();
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
            laserPool.AddGameObject(gameObject);
        } else if (collision.gameObject.CompareTag("REnemy") && isFriendly) {
            rangedEnemyPool.AddGameObject(collision.gameObject);
            AudioSource.PlayClipAtPoint(shipExplosionSound.clip, transform.position);
            Variables.score += 10;
            laserPool.AddGameObject(gameObject);
        } else if (collision.gameObject.CompareTag("MEnemy") && isFriendly) {
            meleeEnemyPool.AddGameObject(collision.gameObject);
            AudioSource.PlayClipAtPoint(shipExplosionSound.clip, transform.position);
            Variables.score += 10;
            laserPool.AddGameObject(gameObject);
        } else if (collision.gameObject.CompareTag("Player") && !isFriendly) {
            AudioSource.PlayClipAtPoint(shipExplosionSound.clip, transform.position);
            collision.gameObject.SetActive(false);
            laserPool.AddGameObject(gameObject);
        }
    }
}
