﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeysUI : MonoBehaviour {

    Text text;

	void Start () {
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        text.text = string.Format(" Keys {0}/8", Variables.keysObtained);
	}
}
