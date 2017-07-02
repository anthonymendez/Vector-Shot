using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variables {
    //Used in all Translate methods; Allows me to give fine tune movement speed
	public static float speedDampener = 100f;
    //There's a good reason I put this here...I just can't remember right now...
    public static Camera mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    //Track score of the player
    public static int Score = 0;
    //Track Keys obtained
    public static int keysObtained = 0;

    public static void restart() {
        Score = 0;
        keysObtained = 0;
    }

}
