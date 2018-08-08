using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : AbstractEnemy {

    public const int ID = 2;

    [SerializeField] float timeBetweenShots;
    [SerializeField] protected GameObjectPool laserPool;

    protected float timeSinceLastShot;

    new void Start() {
        base.Start();
    }

    // Update is called once per frame
    new void Update () {
        base.Update();
    }

    new void FixedUpdate() {
        base.FixedUpdate();
        TrackShooting();
    }

    void TrackShooting() {
        if (IsLookingAtPlayer() && CanShootNow()) {
            ShootLaser();
            timeSinceLastShot = 0;
        }

        timeSinceLastShot += Time.deltaTime;
    }

    private void ShootLaser() {
        GameObject shot = laserPool.GetGameObject();
        shot.transform.rotation = transform.rotation;
        shot.transform.position = transform.position;
        shot.GetComponent<Laser>().laserOrigin = LaserOrigin.Enemy;
        shot.transform.Translate(0f, 2.5f, 0f);
    }

    private bool CanShootNow() {
        return timeSinceLastShot > timeBetweenShots;
    }
}
