using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeadMenu : MonoBehaviour {

    Button restart, options, exit, exitOptions;


    public OptMenu optMenu;

    void Start() {
        restart = GameObject.FindWithTag("Restart").GetComponent<Button>();
        restart.onClick.AddListener(RestartGame);

        options = GameObject.FindWithTag("Options").GetComponent<Button>();
        options.onClick.AddListener(OptionsMenu);

        exit = GameObject.FindWithTag("Exit").GetComponent<Button>();
        
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
