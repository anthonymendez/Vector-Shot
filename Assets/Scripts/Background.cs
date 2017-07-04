using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {
    
    private static Background instance = null;
    public static Background Instance {
        get { return instance; }
    }

    public AudioSource Music;

    // Use this for initialization
    void Awake () {

        if (instance != null && Instance != null) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
	}

    void Start() {
        Music = GetComponent<AudioSource>();
    }
}
