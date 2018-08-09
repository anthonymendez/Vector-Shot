using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public Button play, options, credits, exit;

    [SerializeField] Text title;
    [SerializeField] GameObject optionsMenu, creditsMenu;
    
	// Use this for initialization
	void Start () {
        play.onClick.AddListener(Play);
        options.onClick.AddListener(OptionsMenu);
        credits.onClick.AddListener(Credits);
        exit.onClick.AddListener(Exit);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void Play() {
        SceneManager.LoadScene(SceneManager.GetSceneByName("TestArena_1").buildIndex);
    }

    void OptionsMenu() {
        optionsMenu.SetActive(true);
    }

    void Credits() {
        creditsMenu.SetActive(true);
    }

    void Exit() {

    }
}
