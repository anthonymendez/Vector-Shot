using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelect : MonoBehaviour {

    [SerializeField] GameObject[] PlayerSelectMenus;
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < PlayerSelectMenus.Length; i++) {
            if(Input.GetButtonDown(string.Format("Start_P{0}", i+1))) {
                SetPlayerActive(true, i);
            } else if (Input.GetButtonDown(string.Format("Select_P{0}", i+1))) {
                SetPlayerActive(false, i);
            }
        }
    }

    void SetPlayerActive(bool isActive, int i) {
        Transform currentMenu = PlayerSelectMenus[i].transform;
        currentMenu.GetChild(0).gameObject.SetActive(!isActive);
        currentMenu.GetChild(1).gameObject.SetActive(isActive);
        Helper.playerJoined[i] = isActive;
    }

    public void Play() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
