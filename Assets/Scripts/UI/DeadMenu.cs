using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeadMenu : MonoBehaviour {

    public GameObject DMUI;

    //Restart Method
    public void RestartGame() {
        Variables.score = 0;
        Variables.keysObtained = 0;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        gameObject.SetActive(false);
    }

    public void ToMainMenu() {
        Variables.score = 0;
        Variables.keysObtained = 0;
        SceneManager.LoadScene(0);
        GetComponent<StartOptions>().changeScenes = false;
        gameObject.SetActive(false);
    }

}
