using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour {

    public Text TimeScore, GameScore, FinalScore;

    public GameObject WMUI;

    float placeHolder;

    float TS, GS, FS;

    void Awake() {
        //Calculate Timescore
        placeHolder = TimeUI.timeInSeconds;
        if (placeHolder >= 60) {
            TimeScore.text = "" + 60 + "";
            TS = 60;
        } else {
            TimeScore.text = (1000f / placeHolder).ToString();
            TS = 1000f / placeHolder;
        }
        //Set GameScore
        GameScore.text = Variables.score.ToString();
        GS = Variables.score;
        //Set FinalScore
        FS = TS + GS;
        FinalScore.text = FS.ToString();
    }

    public void Restart() {
        Variables.score = 0;
        Variables.keysObtained = 0;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        WMUI.SetActive(false);
    }

    public void Options() {

    }
}
