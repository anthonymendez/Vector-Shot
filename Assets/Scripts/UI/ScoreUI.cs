using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour {

    Text text;

	void Start () {
        text = GetComponent<Text>();	
	}
	
	// Update is called once per frame
	void Update () {
        text.text = string.Format(" Score {0}",Variables.score);
	}
}
