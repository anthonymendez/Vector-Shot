using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.left * 25f/ Variables.speedDampener);

        if(transform.position.x < -30f) {
            transform.position = new Vector3(Random.Range(130f,150f), Random.Range(-80f,140f),0f);
        }
	}
}
