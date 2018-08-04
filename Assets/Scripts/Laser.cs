using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    public const int ID = 0;

    public bool isFriendly;

    [SerializeField] float moveSpeed;
    [SerializeField] List<string> collidableGameObjectTags = new List<string>() {"Laserpool", "MEnemyPool", "REnemyPool", "Wall", "Player" };

    GameObjectPool laserPool, meleeEnemyPool, rangedEnemyPool;
    Rigidbody2D physics;
    AudioSource shipExplosionSound;

    void Awake() {
        shipExplosionSound = GetComponent<AudioSource>();
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
        string tag = collision.gameObject.tag;
        //Check if colliding object is laser, if so, each laser 
        //calls this method and adds themselves to the pool
        if (laserPool == null) {
            laserPool = GameObject.FindWithTag("Laserpool").GetComponent<GameObjectPool>();
        }
        if (HitCollidableObject(collision)) {
            AddToLaserPool();
        } else if (collision.gameObject.CompareTag("REnemy") && isFriendly) {
            rangedEnemyPool.AddGameObject(collision.gameObject);
            AudioSource.PlayClipAtPoint(shipExplosionSound.clip, transform.position);
            Variables.score += 10;
            AddToLaserPool();
        } else if (collision.gameObject.CompareTag("MEnemy") && isFriendly) {
            meleeEnemyPool.AddGameObject(collision.gameObject);
            AudioSource.PlayClipAtPoint(shipExplosionSound.clip, transform.position);
            Variables.score += 10;
            AddToLaserPool();
        } else if (collision.gameObject.CompareTag("Player") && !isFriendly) {
            AudioSource.PlayClipAtPoint(shipExplosionSound.clip, transform.position);
            collision.gameObject.SetActive(false);
            AddToLaserPool();
        }
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
