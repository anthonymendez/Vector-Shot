using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour {

    public float MaxLOSDistance;

    RaycastHit2D sight;
    Rigidbody2D physics;
    GameObject player;

    Vector3 lastSeenPlayer;

	// Use this for initialization
	void Start () {
        
    }

    // Update is called once per frame
    void Update () {
        makeSureStuffIsInitialized();
        RayTracking();
    }

    void FixedUpdate() {
        makeSureStuffIsInitialized();
        trackMovement();
    }

    void RayTracking() {
        Vector3 currentPosition = transform.position;
        Vector3 direction = (player.transform.position - transform.position).normalized;
        
        Debug.DrawRay(currentPosition, direction * MaxLOSDistance, Color.red);

        sight = Physics2D.Raycast(currentPosition, direction, MaxLOSDistance);
        if (sight.collider != null) {
            if (sight.collider.gameObject != gameObject) {
                Debug.Log("Rigidbody Collider is: " + sight.collider);
            }
        }
    }

    void trackMovement() {
        
        

    }

    void makeSureStuffIsInitialized() {
        if (player == null) {
            player = GameObject.FindWithTag("Player");
        }

        if (physics == null) {
            physics = GetComponent<Rigidbody2D>();
        }
    }
}
