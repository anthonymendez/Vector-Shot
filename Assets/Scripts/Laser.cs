using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    public const int ID = 0;

    public bool isFriendly;

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
            AddToLaserPool();
            Debug.Log(string.Format("Laser hit: {0}; Laser is Friendly: {1}", collision.gameObject.tag, isFriendly));
            if (collision.gameObject.CompareTag("REnemy") && isFriendly) {
                rangedEnemyPool.AddGameObject(collision.gameObject);
                AudioSource.PlayClipAtPoint(shipExploding, transform.position);
                Variables.score += 10;
            } else if (collision.gameObject.CompareTag("MEnemy") && isFriendly) {
                meleeEnemyPool.AddGameObject(collision.gameObject);
                AudioSource.PlayClipAtPoint(shipExploding, transform.position);
                Variables.score += 10;
            } else if (collision.gameObject.CompareTag("Shield") && !isFriendly) {
                AudioSource.PlayClipAtPoint(hitShield, transform.position);
                collision.gameObject.GetComponent<Shield>().DamageShield(1);
            } else if (collision.gameObject.CompareTag("Player") && !isFriendly) {
                AudioSource.PlayClipAtPoint(shipExploding, transform.position);
                collision.gameObject.SetActive(false);
            } 
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
