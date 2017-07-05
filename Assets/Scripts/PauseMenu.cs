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

        cont.onClick.AddListener(Pause);

        options.onClick.AddListener(OptionsMenu);

        gameObject.SetActive(false);
    }

    void OptionsMenu() {
        optMenu.OptionsMenu();
    }

    public void Pause() {
        player.Pause();
    }
    
}
