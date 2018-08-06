using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEnemy : MonoBehaviour, Bonkable {
    [Header("General Properties")]
    [SerializeField] protected GameObject player;
    [SerializeField] protected GameObjectPool enemyPool;
    [SerializeField] protected Rigidbody2D physics;

    [Header("Tracking Properties")]
    [SerializeField] protected float maxLOSDistance;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected LayerMask masksDetectedByRaycast;
    [SerializeField] [Range(0f, 5f)] protected float bonkStunTime = 5f;

    [Header("Debug Properties")]
    [SerializeField] protected Color rayColor;

    protected RaycastHit2D sight;
    protected Vector3 playerLastSeen;
    protected Vector3 smoothVelocity;

    protected bool bonked = false;
    protected float bonkTimer = 0f;

    // Use this for initialization
    protected void Start () {
        smoothVelocity = Vector3.zero;
        player = GameObject.FindGameObjectWithTag(player.tag);
        playerLastSeen = transform.position;
    }

    // Update is called once per frame
    protected void Update () {

    }

    protected void FixedUpdate() {
        if (bonked) {
            TrackTimeSinceBonked();
        }
        RayTracking();
        TrackMovement();
    }

    private void TrackTimeSinceBonked() {
        bonkTimer += Time.fixedDeltaTime;
        Debug.Log(string.Format("Bonk Timer: {0}", bonkTimer));

        if (bonkTimer > bonkStunTime) {
            bonked = false;
            bonkTimer = 0;
        }
    }

    void RayTracking() {
        Vector3 currentPosition = transform.position;
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Vector3 sightRange = direction * maxLOSDistance;

        sight = Physics2D.Raycast(currentPosition, direction, maxLOSDistance, masksDetectedByRaycast);

        bool nullColliderCheck = sight.collider != null;
        if (nullColliderCheck) {
            Debug.Log("Rigidbody Collider is: " + sight.collider);
        }

        Debug.DrawRay(currentPosition, sightRange, rayColor);
    }

    void TrackMovement() {
        if (IsLookingAtPlayer()) {
            playerLastSeen = player.transform.position;
        }

        if (!bonked && IsNotAtLastSeenPlayerLocation()) {
            //This is genius, thank you abar http://answers.unity3d.com/questions/585035/lookat-2d-equivalent-.html
            Transform newTransform = transform;
            newTransform.up = playerLastSeen - transform.position;
            newTransform.Translate(new Vector3(0f, 1f, 0f) * moveSpeed / Variables.speedDampener);
            physics.angularVelocity = 0f;
            physics.velocity = Vector2.up * moveSpeed / Variables.speedDampener;
        } else if (!bonked){
            physics.velocity = Vector2.zero;
        }
    }

    protected bool IsNotAtLastSeenPlayerLocation() {
        float distanceFromPlayer = (playerLastSeen - transform.position).magnitude;
        float radiusOfLocation = 0.25f;

        return distanceFromPlayer > radiusOfLocation;
    }

    protected bool IsLookingAtPlayer() {
        bool colliderIsNotNull = sight.collider != null;
        if (colliderIsNotNull) {
            bool colliderIsNotThisGameObject = (sight.collider.gameObject != gameObject);
            bool colliderIsPlayer = sight.collider.gameObject.CompareTag("Player") || 
                                    sight.collider.gameObject.CompareTag("Shield");

            return colliderIsNotThisGameObject && colliderIsPlayer;
        }

        return false;
    }

    public void SetBonked(bool isBonked) {
        bonked = isBonked;
        bonkTimer = 0f;
    }
}
