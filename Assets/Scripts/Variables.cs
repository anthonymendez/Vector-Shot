using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variables {
    // Used in all Translate methods. Use to fine tune movement speed.
    public static float speedDampener = 100f;
    
    // Track score of the player.
    public static int score = 0;

    // Track Keys obtained
    public static int keysObtained = 0;
    
    // Volume Control
    public static float musicVolume = 1;
    public static float sfxVolume = 1;

    public static void RestartScore() {
        score = 0;
        keysObtained = 0;
    }

}
