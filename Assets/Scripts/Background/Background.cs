using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

    public GameObject lights, lightsPrefab;

    void Awake () {
	}

    void Start() {
        
    }

    void Update() {
        lights.SetActive(lightsPrefab.activeSelf);
    }
}
