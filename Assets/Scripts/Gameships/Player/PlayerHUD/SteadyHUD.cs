using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteadyHUD : MonoBehaviour {

    private Transform playerTransform;
    private Quaternion originalWorldRotation;
    private Quaternion rotationRelativeToShip;

	// Use this for initialization
	void Start () {
        InitializeComponents();
        InitializeValues();
    }

    private void InitializeComponents() {
        playerTransform = GetComponentInParent<Player>().transform;
    }

    private void InitializeValues() {
        originalWorldRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update () {
        transform.rotation = originalWorldRotation;
    }
}
