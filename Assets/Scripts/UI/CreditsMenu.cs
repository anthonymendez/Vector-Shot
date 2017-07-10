using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsMenu : MonoBehaviour {

    public Text title, song, soundEffects;

    public Button exit;

	// Use this for initialization
	void Start () {
        exit.onClick.AddListener(Close);

        gameObject.SetActive(false);
	}

	void Close () {
        gameObject.SetActive(false);
	}
}
