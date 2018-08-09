using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour {

    private Text[] playerScores;

    void Start() {
        playerScores = transform.GetComponentsInChildren<Text>();
    }

	// Update is called once per frame
	void Update () {
		for (int i = 0; i < playerScores.Length; i++) {
            Text playerScore = playerScores[i];
            playerScore.text = Helper.playerScores[i].ToString();
        }
	}
}
