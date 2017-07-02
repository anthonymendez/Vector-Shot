using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour {

    public float MaxLOSDistance;
    public float moveSpeed;

    RaycastHit2D sight;
    Rigidbody2D physics;
    GameObject player;

    Vector3 lastSeenPlayer;
    Vector3 smoothVelocity;

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
        if (sight.collider != null && sight.collider.gameObject != gameObject && sight.collider.gameObject.CompareTag("Player")) {
            //This is genius, thank you abar http://answers.unity3d.com/questions/585035/lookat-2d-equivalent-.html
            transform.up = player.transform.position - transform.position;
        }
    }

    void makeSureStuffIsInitialized() {
        if (player == null) {
            player = GameObject.FindWithTag("Player");
        }

        if (physics == null) {
            physics = GetComponent<Rigidbody2D>();
        }

        if (smoothVelocity == null) {
            smoothVelocity = Vector3.zero;
        }
    }
}
