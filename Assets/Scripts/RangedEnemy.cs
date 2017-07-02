using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour {

    public float MaxLOSDistance;
    public float moveSpeed;
    public float shotDelay;

    RaycastHit2D sight;
    Rigidbody2D physics;
    GameObject player;
    GameObjectPool LaserPool;
    float timeSinceLastShot;

    Vector3 lastSeenPlayer;
    Vector3 smoothVelocity;

    // Update is called once per frame
    void Update () {
        makeSureStuffIsInitialized();
        RayTracking();
    }

    void FixedUpdate() {
        makeSureStuffIsInitialized();
        trackMovement();
        timeSinceLastShot += Time.deltaTime;
    }

    void RayTracking() {
        Vector3 currentPosition = transform.position;
        Vector3 direction = (player.transform.position - transform.position).normalized;
        
        Debug.DrawRay(currentPosition, direction * MaxLOSDistance, Color.green);

        sight = Physics2D.Raycast(currentPosition, direction, MaxLOSDistance);
        if (sight.collider != null) {
            if (sight.collider.gameObject != gameObject) {
//                Debug.Log("Rigidbody Collider is: " + sight.collider);
            }
        }
    }

    void trackMovement() {
        if (sight.collider != null && sight.collider.gameObject != gameObject && sight.collider.gameObject.CompareTag("Player")) {
            //This is genius, thank you abar http://answers.unity3d.com/questions/585035/lookat-2d-equivalent-.html
            transform.up = player.transform.position - transform.position;
            Transform temporary = transform;
            temporary.Translate(new Vector3(0f, 1f, 0f) * moveSpeed / Variables.speedDampener);
            physics.MovePosition(temporary.position);
            //Track shooting here because we don't want to shoot if we're not in range
            trackShooting();
        } else {
            physics.MovePosition(transform.position);
            
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

        if (LaserPool == null) {
            LaserPool = GameObject.FindWithTag("Laserpool").GetComponent<GameObjectPool>();
        }
    }

    void trackShooting() {
        if (timeSinceLastShot > shotDelay) {
            timeSinceLastShot = 0;
            GameObject shot = LaserPool.getGameObject();
            shot.transform.rotation = transform.rotation;
            //We're going to edit the position of the shot here so it's right in front our player
            shot.transform.position = transform.position;
            shot.transform.Translate(0f, 2.5f, 0f);
        }
    }
}
