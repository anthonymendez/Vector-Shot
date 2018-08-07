using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper {

    // Thank you Mikhael-H http://answers.unity.com/answers/1332280/view.html
    public static bool Contains(this LayerMask mask, int layer) {
        return mask == (mask | (1 << layer));
    }

    public static string ToInputConverter(this string axesName, int playerNumber) {
        return string.Format("{0}_P{1}", axesName, playerNumber);
    }
}
