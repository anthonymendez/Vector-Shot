﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public Button cont, options, exit;

    public OptMenu optMenu;

    public CreditsMenu credMenu;

    Player player;

    void Start() {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

//        cont.onClick.AddListener(Pause);

        options.onClick.AddListener(OptionsMenu);

        exit.onClick.AddListener(Credits);

        gameObject.SetActive(false);
    }

    void OptionsMenu() {
        optMenu.gameObject.SetActive(true);
    }
    /*
    public void Pause() {
        player.Pause();
    }*/

    void Credits() {
        credMenu.gameObject.SetActive(true);
    }
    
}
