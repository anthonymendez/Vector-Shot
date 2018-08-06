using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : AbstractEnemy {

    public const int ID = 3;
    new void Start() {
        base.Start();
    }

    // Update is called once per frame
    new void Update() {
        base.Update();
    }

    new void FixedUpdate() {
        base.FixedUpdate();
    }

    void OnCollisionEnter2D(Collision2D collision) {
        GameObject collidingGameObject = collision.gameObject;

        if (collidingGameObject.CompareTag(player.tag)) {
            player.SetActive(false);
        }
    }
}
