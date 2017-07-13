﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicPolyCollider : MonoBehaviour {

    // parent game objects position, needs to be at the center of the ring
    public Vector2 center;

    // resizable list of vector2s
    public List<Vector2> points = new List<Vector2>();
    
    // Use this for initialization
    void Start() {
        PolygonCollider2D thisCollider2D = GetComponent<PolygonCollider2D>();
        center = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);

        foreach (Transform child in transform) {
            PolygonCollider2D childCollider2D = child.GetComponent<PolygonCollider2D>();
            Vector2[] childPoints = childCollider2D.points;

            foreach (Vector2 childPoint in childPoints) {
                float x = childPoint.x + child.position.x;
                float y = childPoint.y + child.position.y;

                points.Add(new Vector2(x, y));
            }

        }

        thisCollider2D.points = points.ToArray();

    }
}
