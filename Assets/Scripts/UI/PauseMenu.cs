using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    [SerializeField] Button cont, options, exit;
    [SerializeField] OptMenu optMenu;
    [SerializeField] CreditsMenu credMenu;

    void Start() {
        options.onClick.AddListener(OptionsMenu);
        exit.onClick.AddListener(Credits);

        gameObject.SetActive(false);
    }

    void OptionsMenu() {
        optMenu.gameObject.SetActive(true);
    }

    void Credits() {
        credMenu.gameObject.SetActive(true);
    }
    
}
