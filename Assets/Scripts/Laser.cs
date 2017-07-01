using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    public float moveSpeed;

    private GameObjectPool LaserPool;

    void Awake() {
        LaserPool = GameObject.FindWithTag("Laserpool").GetComponent<GameObjectPool>();
    }

	// Use this for initialization
	void Start () {
        LaserPool = GameObject.FindWithTag("Laserpool").GetComponent<GameObjectPool>();
    }
	
	// Update is called once per frame
	void Update () {
        //I just typed these two trig equations because I knew they would give me some sort of angle, and apparently this works
        //      float xDirectionIntensity = Mathf.Asin(transform.rotation.eulerAngles.x / transform.rotation.eulerAngles.z);
        //       float yDirectionIntensity = Mathf.Acos(transform.rotation.eulerAngles.y / transform.rotation.eulerAngles.z);
        //        transform.Translate(new Vector3(xDirectionIntensity, yDirectionIntensity,0f)*moveSpeed/Variables.speedDampener);
        transform.Translate(new Vector3(0f,1f,0f)*moveSpeed/Variables.speedDampener);
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
        } else if (!collision.gameObject.CompareTag("Wall")) {
            Destroy(collision.gameObject);
            LaserPool.addGameObject(gameObject);
        }
    }
}
