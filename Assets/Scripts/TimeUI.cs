using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeUI : MonoBehaviour {

    public float timeInSeconds;

    Text text;
    Cam mainCamera;
	// Use this for initialization
	void Start () {
		text = GetComponent<Text>();
        timeInSeconds = 0;
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Cam>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!mainCamera.isDead()) {
            timeInSeconds += Time.deltaTime;
            text.text = string.Format("{0:0.00} Time", timeInSeconds);
        }
    }
}
