using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour {

    public float maxLOSDistance;
    public float moveSpeed;
    public float shotDelay;

    RaycastHit2D sight;
    Rigidbody2D physics;
    GameObject player;
    GameObjectPool laserPool;
    float timeSinceLastShot;

    Vector3 lastSeenPlayer;
    Vector3 smoothVelocity;

    private void Start() {
        
    }

    // Update is called once per frame
    void Update () {
        MakeSureStuffIsInitialized();
        RayTracking();
    }

    void FixedUpdate() {
        MakeSureStuffIsInitialized();
        TrackMovement();
        timeSinceLastShot += Time.deltaTime;
    }

    void RayTracking() {
        Vector3 currentPosition = transform.position;
        Vector3 direction = (player.transform.position - transform.position).normalized;
        
        Debug.DrawRay(currentPosition, direction * maxLOSDistance, Color.green);

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
            //Track shooting here because we don't want to shoot if we're not in range
            TrackShooting();
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

        if (laserPool == null) {
            laserPool = GameObject.FindWithTag("Laserpool").GetComponent<GameObjectPool>();
        }

        if (!player.GetComponent<Player>().isSpaceLike) {
            maxLOSDistance = 30f;
            moveSpeed = 10;
            shotDelay = 2;
        } else {
            maxLOSDistance = 20;
            moveSpeed = 0;
            shotDelay = 3;
        }
    }

    void TrackShooting() {
        if (timeSinceLastShot > shotDelay) {
            timeSinceLastShot = 0;
            GameObject shot = laserPool.GetGameObject();
            shot.transform.rotation = transform.rotation;
            //We're going to edit the position of the shot here so it's right in front our player
            shot.transform.position = transform.position;
            shot.transform.Translate(0f, 2.5f, 0f);
        }
    }
}
