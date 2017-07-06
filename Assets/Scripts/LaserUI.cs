using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaserUI : MonoBehaviour {

    Text text;

    Player player;

	void Start () {
        text = GetComponent<Text>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
        text.text = string.Format("Laser: {0}",player.shotsAvailable);
	}
}
