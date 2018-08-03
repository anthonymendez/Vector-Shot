using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEnemy : MonoBehaviour {
    [SerializeField] protected GameObject player;
    [SerializeField] protected GameObjectPool enemyPool;
    [SerializeField] protected Rigidbody2D physics;
    [SerializeField] protected float maxLOSDistance;
    [SerializeField] protected float moveSpeed;

    protected RaycastHit2D sight;
    protected Vector3 playerLastSeen;
    protected Vector3 smoothVelocity;



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
        RayTracking();
        TrackMovement();
    }

    void RayTracking() {
        Vector3 currentPosition = transform.position;
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Vector3 sightRange = direction * maxLOSDistance;

        sight = Physics2D.Raycast(currentPosition, direction, maxLOSDistance);

        bool nullColliderCheck = sight.collider != null;

        if (nullColliderCheck) {
            bool colliderIsNotThisGameObject = sight.collider.gameObject != gameObject;

            if (colliderIsNotThisGameObject) {
                Debug.Log("Rigidbody Collider is: " + sight.collider);
            }
        }

        Debug.DrawRay(currentPosition, sightRange, Color.red);
    }

    void TrackMovement() {
        if (IsLookingAtPlayer()) {
            playerLastSeen = player.transform.position;
        }

        if (IsNotAtLastSeenPlayerLocation()) {
            //This is genius, thank you abar http://answers.unity3d.com/questions/585035/lookat-2d-equivalent-.html
            Transform newTransform = transform;
            newTransform.up = playerLastSeen - transform.position;
            newTransform.Translate(new Vector3(0f, 1f, 0f) * moveSpeed / Variables.speedDampener);
            physics.MovePosition(newTransform.position);
        }
    }

    private bool IsNotAtLastSeenPlayerLocation() {
        float distanceFromPlayer = (playerLastSeen - transform.position).magnitude;
        float radiusOfLocation = 0.1f;

        return distanceFromPlayer > radiusOfLocation;
    }

    private bool IsLookingAtPlayer() {
        bool nullColliderCheck = sight.collider != null;
        bool colliderIsNotThisGameObject = (sight.collider.gameObject != gameObject);
        bool colliderIsPlayer = sight.collider.gameObject.Equals(player);

        return nullColliderCheck && colliderIsNotThisGameObject && colliderIsPlayer;
    }
}
