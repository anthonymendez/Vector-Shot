using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour {

    [SerializeField] private Player playerPrefab;
    [SerializeField] private GameObject spawnPointsHolder;
    [SerializeField] private Transform playerHolder;
    [SerializeField] Color[] spawnableColors;

    private string[] joystickNames;

	// Use this for initialization
	void Start () {
        Transform[] spawnPoints = spawnPointsHolder.GetComponentsInChildren<Transform>();

        for (int i = 0; i < 4; i++) {
            if (!Helper.playerJoined[i])
                continue;

            Transform spawnPoint = spawnPoints[i + 1];

            Player spawnedPlayer = Instantiate(playerPrefab, transform);
            spawnedPlayer.transform.position = spawnPoint.position;
            spawnedPlayer.transform.parent = playerHolder;
            spawnedPlayer.SetColor(spawnableColors[i]);
            spawnedPlayer.SetPlayerNumber(i + 1);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
