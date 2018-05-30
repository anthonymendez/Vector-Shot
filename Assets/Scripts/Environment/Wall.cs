using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[SelectionBase]
public class Wall : MonoBehaviour {

    [SerializeField] int gridSize = 5;
    [SerializeField] SpriteRenderer wallSpriteRenderer;

    void Update() {
        SnapToGrid();
        ScaleToGrid();
    }

    private void SnapToGrid() {
        Vector2 newPosition = new Vector2 (
            Mathf.RoundToInt(transform.position.x / gridSize),
            Mathf.RoundToInt(transform.position.y / gridSize)
        );

        newPosition *= gridSize;

        transform.position = newPosition;
    }

    private void ScaleToGrid() {
        Vector2 size = wallSpriteRenderer.size;

        Vector2 newSize = new Vector2 (
            Mathf.RoundToInt(size.x / gridSize),
            Mathf.RoundToInt(size.y / gridSize)
        );

        newSize *= gridSize;

        wallSpriteRenderer.size = newSize;
    }
}
