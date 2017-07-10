using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Wall : MonoBehaviour {

    public float zRotationAfterAwake;

    SpriteRenderer sprite;
    Rigidbody2D mainPhysics;

    // Use this for initialization
    void Awake () {
        //Thank you vexe http://answers.unity3d.com/questions/530178/how-to-get-a-component-from-an-object-and-add-it-t.html
        Rigidbody2D childPhysics;

        sprite = GetComponent<SpriteRenderer>();
        mainPhysics = GetComponent<Rigidbody2D>();

        Vector2 spriteSize = new Vector2(sprite.bounds.size.x / transform.localScale.x, sprite.bounds.size.y / transform.localScale.y);

        GameObject childPrefab = new GameObject();
        SpriteRenderer childSprite = childPrefab.AddComponent<SpriteRenderer>();
        childPhysics = childPrefab.AddComponent<Rigidbody2D>();
        childPrefab.transform.position = transform.position;
        childPrefab.tag = "Wall";
        childPrefab.layer = gameObject.layer;
        childSprite.sprite = sprite.sprite;
        childPhysics.isKinematic = true;
        childPhysics.useFullKinematicContacts = true;
        childPhysics.sharedMaterial = mainPhysics.sharedMaterial;
        childPrefab.AddComponent<PolygonCollider2D>();

        GameObject child;
        for (int i = 1, l = (int)(Mathf.Round(sprite.bounds.size.y)); i < l; i++) {
            child = Instantiate<GameObject>(childPrefab);
            child.transform.position = transform.position - (new Vector3(0f, spriteSize.y, 0f) * i);
            child.transform.parent = transform;
        }

        childPrefab.transform.parent = transform;

        sprite.enabled = false;

        transform.rotation = Quaternion.Euler(0f,0f,zRotationAfterAwake);
    }
}
