using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAmmoHUD : MonoBehaviour {

    private Player player;
    private SpriteRenderer spriteRenderer;
    private float initialSpriteTiledWidth = 0.8f;

    void Start() {
        InitializeComponents();
        InitializeValues();
    }

    private void InitializeComponents() {
        player = GetComponentInParent<Player>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void InitializeValues() {
        initialSpriteTiledWidth = spriteRenderer.size.x;
    }

    // Update is called once per frame
    void Update () {
        int ammoCount = player.GetShotsAvailable();
        float tileWidthPerAmmo = initialSpriteTiledWidth / player.GetShotLimit();
        float newWidth = (ammoCount * tileWidthPerAmmo);

        spriteRenderer.size = new Vector2(newWidth, spriteRenderer.size.y);
	}
}
