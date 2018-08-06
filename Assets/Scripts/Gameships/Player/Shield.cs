using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    [Header("Shield Health Properties")]
    [SerializeField] int maxShieldHealth = 3;
    [SerializeField] float timeToRegenLevel = 1f;

    [Header("Shield Coloring")]
    [SerializeField] Color[] shieldLevelColors = new Color[] { Color.black, Color.red, Color.yellow, Color.blue };

    [Header("Layer Mask Properties")]
    [SerializeField] LayerMask collidableLayerMasks;
    [SerializeField] LayerMask bonkableLayerMasks;

    [Header("Sound Properties")]
    [SerializeField] AudioClip bonkClip;
    
    
    private int shieldHealth;
    private SpriteRenderer spriteRenderer;
    private float currentRegenLevelTime = 0f;
    private bool isDamaged = false;

    public int GetShieldHealth() { return shieldHealth; }
    public bool ShieldAvailable() { return shieldHealth > 0; }
    public void RegenShield() { shieldHealth = maxShieldHealth;  }
    public void DamageShield(int damage) { shieldHealth -= damage; }

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        shieldHealth = maxShieldHealth;
    }

    void FixedUpdate() {
        UpdateShieldColor();
        CheckShield();
        if (isDamaged) {
            RegenShieldLevel();
        }
    }

    private void UpdateShieldColor() {
        spriteRenderer.color = shieldLevelColors[shieldHealth];
    }

    private void CheckShield() {
        isDamaged = shieldHealth > 0 && shieldHealth < maxShieldHealth;
    }

    private void RegenShieldLevel() {
        currentRegenLevelTime += Time.deltaTime;

        if (currentRegenLevelTime >= timeToRegenLevel) {
            shieldHealth++;
            currentRegenLevelTime = 0f;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        bool isObjectCollidable = collidableLayerMasks.Contains(collision.gameObject.layer);
        Debug.Log(string.Format("isObjectCollidable: {0}", isObjectCollidable));

        if (isObjectCollidable) {
            GameObject collidingObject = collision.gameObject;
            bool isObjectBonkable = bonkableLayerMasks.Contains(collidingObject.layer);
            Debug.Log(string.Format("collidingObject: {0}; isObjectBonkable: {1}", collidingObject.name, isObjectBonkable));

            if (isObjectBonkable) {
                if (collidingObject.Equals(transform.parent.gameObject)) {
                    return;
                }

                if (collidingObject.CompareTag("Shield")) {
                    collidingObject = collidingObject.transform.parent.gameObject;
                }

                Bonkable objectToBonk = collidingObject.GetComponent<Bonkable>();
                objectToBonk.SetBonked(true);
                
                Vector2 normalDirection = transform.rotation * Vector2.one;
                float forceOut = 20f;
                Vector2 bonkForce = normalDirection * forceOut;
                Vector2 positionToBonk = collision.contacts[0].point;

                AudioSource.PlayClipAtPoint(bonkClip, transform.position);

                collidingObject.GetComponent<Rigidbody2D>().AddForceAtPosition(bonkForce, positionToBonk, ForceMode2D.Impulse);
                Debug.Log(string.Format("BonkForce: {0}, Direction: {1}", bonkForce, normalDirection));

                shieldHealth = 0;
            } else {
                if (collidingObject.CompareTag("LaserShot")) {
                    shieldHealth--;
                }
            }
        }
    }

}