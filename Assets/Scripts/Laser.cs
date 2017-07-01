using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    public float moveSpeed;

    GameObjectPool LaserPool;
    Rigidbody2D physics;

    void Awake() {
        LaserPool = GameObject.FindWithTag("Laserpool").GetComponent<GameObjectPool>();
        physics = GetComponent<Rigidbody2D>();
    }

	// Use this for initialization
	void Start () {
        LaserPool = GameObject.FindWithTag("Laserpool").GetComponent<GameObjectPool>();
        physics = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        //Much better way of calculating movement
        Transform temp = transform;
        temp.Translate(new Vector3(0f,1f,0f)*moveSpeed/Variables.speedDampener);
        physics.MovePosition(temp.position);
	}

    void OnCollisionEnter2D(Collision2D collision) {
        string tag = collision.gameObject.tag;
        //Check if colliding object is laser, if so, each laser 
        //calls this method and adds themselves to the pool
        if (LaserPool == null) {
            LaserPool = GameObject.FindWithTag("Laserpool").GetComponent<GameObjectPool>();
        }

        if (collision.gameObject.CompareTag("LaserShot")) {
            LaserPool.addGameObject(this.gameObject);
            //For any other object that isn't a wall, 
            //we destroy the object since everything in this game 
            //is 1 a hit kill
        } else if (collision.gameObject.CompareTag("Wall")) {
            LaserPool.addGameObject(gameObject);
        } else {
            Destroy(collision.gameObject);
            LaserPool.addGameObject(gameObject);
        }
    }
}
