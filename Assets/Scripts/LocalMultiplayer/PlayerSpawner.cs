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
        joystickNames = Input.GetJoystickNames();

        Transform[] spawnPoints = spawnPointsHolder.GetComponentsInChildren<Transform>();
        
        // i is equal to 1 because the first transform is the holder itself
        for (int i = 1; i < spawnPoints.Length && i < joystickNames.Length + 1; i++) {
            Transform spawnPoint = spawnPoints[i];

            Player spawnedPlayer = Instantiate(playerPrefab, transform);
            spawnedPlayer.transform.position = spawnPoint.position;
            spawnedPlayer.transform.parent = playerHolder;
            spawnedPlayer.SetColor(spawnableColors[i-1]);
            spawnedPlayer.SetPlayerNumber(i);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
