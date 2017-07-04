using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour {

    public float maxLOSDistance;
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
        MakeSureStuffIsInitialized();
        RayTracking();
    }

    void FixedUpdate() {
        MakeSureStuffIsInitialized();
        TrackMovement();
    }

    void RayTracking() {
        Vector3 currentPosition = transform.position;
        Vector3 direction = (player.transform.position - transform.position).normalized;
        
        Debug.DrawRay(currentPosition, direction * maxLOSDistance, Color.red);

        sight = Physics2D.Raycast(currentPosition, direction, maxLOSDistance);
        if (sight.collider != null) {
            if (sight.collider.gameObject != gameObject) {
//                Debug.Log("Rigidbody Collider is: " + sight.collider);
            }
        }
    }

    void TrackMovement() {
        if (sight.collider != null && sight.collider.gameObject != gameObject && sight.collider.gameObject.CompareTag("Player")) {
            //This is genius, thank you abar http://answers.unity3d.com/questions/585035/lookat-2d-equivalent-.html
            transform.up = player.transform.position - transform.position;
            Transform temporary = transform;
            temporary.Translate(new Vector3(0f, 1f, 0f) * moveSpeed / Variables.speedDampener);
            physics.MovePosition(temporary.position);
        } else {
            physics.MovePosition(transform.position);
            
        }
    }

    void MakeSureStuffIsInitialized() {
        if (player == null) {
            player = GameObject.FindWithTag("Player");
        }

        if (physics == null) {
            physics = GetComponent<Rigidbody2D>();
        }

        if (smoothVelocity == null) {
            smoothVelocity = Vector3.zero;
        }

        if (player.GetComponent<Player>().isSpaceLike) {
            maxLOSDistance = 30f;
            moveSpeed = 30f;
        } else {
            maxLOSDistance = 20f;
            moveSpeed = 20;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collision.gameObject.SetActive(false);
        }
    }
}
