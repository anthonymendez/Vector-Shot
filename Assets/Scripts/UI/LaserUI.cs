using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaserUI : MonoBehaviour {

    public Text rToReload;

    Text text;

    Player player;

	void Start () {
        text = GetComponent<Text>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
        text.text = string.Format("Lasers {0}",player.shotsAvailable);
        if(player.shotsAvailable == 0) {
            rToReload.gameObject.SetActive(true);
        } else {
            rToReload.gameObject.SetActive(false);
        }
	}
}
