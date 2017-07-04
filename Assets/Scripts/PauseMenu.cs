using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public Button cont, options, exit;

    public OptMenu optMenu;

    Player player;

    void Start() {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        
        cont.onClick.AddListener(PauseOrUnpause);
        
        options.onClick.AddListener(OptionsMenu);
        

    }

    void OptionsMenu() {
        optMenu.OptionsMenu();
    }

    void PauseOrUnpause() {
        player.isPaused = true;
        player.TrackGamePause();
    }
    
}
