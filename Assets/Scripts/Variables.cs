using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variables {
    //Used in all Translate methods; Allows me to give fine tune movement speed
    public static float speedDampener = 100f;
    //There's a good reason I put this here...I just can't remember right now...
    public static Camera mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    //Track score of the player
    public static int score = 0;
    //Track Keys obtained
    public static int keysObtained = 0;
    //Control Schemes
    public static bool isSpaceLike = false;
    //Music and SFX Volume
    public static float musicVolume = 1;
    public static float sfxVolume = 1;

    public static void Restart() {
        score = 0;
        keysObtained = 0;
    }

}
