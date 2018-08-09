using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LaserOrigin { Enemy, Player }

public static class Helper {

    public static bool[] playerJoined = { false, false, false, false };
    public static int[] playerScores = { 0, 0, 0, 0 };

    // Thank you Mikhael-H http://answers.unity.com/answers/1332280/view.html
    public static bool Contains(this LayerMask mask, int layer) {
        return mask == (mask | (1 << layer));
    }

    public static string ToInputConverter(this string axesName, int playerNumber) {
        return string.Format("{0}_P{1}", axesName, playerNumber);
    }

    public static bool IsLaserFromPlayer(this LaserOrigin laserOrigin) {
        return laserOrigin.Equals(LaserOrigin.Player);
    }

    public static bool IsLaserFromEnemy(this LaserOrigin laserOrigin) {
        return laserOrigin.Equals(LaserOrigin.Enemy);
    }
}
