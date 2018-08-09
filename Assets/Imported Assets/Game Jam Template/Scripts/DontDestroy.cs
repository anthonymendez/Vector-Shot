using UnityEngine;
using System.Collections;

public class DontDestroy : MonoBehaviour {

    bool isFirst = false;

    void Awake() {
        DontDestroy[] musicPlayers = FindObjectsOfType<DontDestroy>();

        if (musicPlayers.Length == 1)
            isFirst = true;

        foreach (DontDestroy musicPlayer in musicPlayers) {
            if (!musicPlayer.isFirst) {
                Destroy(musicPlayer.gameObject);
            }
        }
    }

	void Start()
	{
		//Causes UI object not to be destroyed when loading a new scene. If you want it to be destroyed, destroy it manually via script.
		DontDestroyOnLoad(this.gameObject);
	}

	

}
