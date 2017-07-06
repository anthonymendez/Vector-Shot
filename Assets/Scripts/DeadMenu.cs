using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeadMenu : MonoBehaviour {

    public Button restart, options, exit, exitOptions;


    public OptMenu optMenu;

    void Start() {
        restart.onClick.AddListener(RestartGame);
        
        options.onClick.AddListener(OptionsMenu);
    }

    //Restart Method
    void RestartGame() {
        Variables.score = 0;
        Variables.keysObtained = 0;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OptionsMenu() {
        optMenu.gameObject.SetActive(true);
    }

}
