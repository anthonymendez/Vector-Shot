using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[SelectionBase]
public class Wall : MonoBehaviour {

    [SerializeField] int gridSize = 5;
    
    void Update() {
        SnapToGrid();
    }

    private void SnapToGrid() {
        Vector2 newPosition = new Vector2 (
            Mathf.RoundToInt(transform.position.x / gridSize),
            Mathf.RoundToInt(transform.position.y / gridSize)
        );

        newPosition *= gridSize;

        transform.position = newPosition;
    }
}
