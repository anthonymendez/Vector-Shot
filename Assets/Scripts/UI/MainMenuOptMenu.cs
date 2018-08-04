using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuOptMenu : MonoBehaviour {

    [SerializeField] Button exitOptions;
    [SerializeField] Slider sfxVol, musVol;

    AudioSource musAS;

    // Use this for initialization
    void Start () {

        //Options Menu Stuff
        musVol.onValueChanged.AddListener(this.UpdateMusicVolumeFromSlider);

        sfxVol.onValueChanged.AddListener(this.UpdateSFXVolumeFromSlider);

        exitOptions.onClick.AddListener(OptionsMenu);

        gameObject.SetActive(false);
    }

    public void OptionsMenu() {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void UpdateMusicVolumeFromSlider(float volume) {
        Variables.musicVolume = volume;
    }

    public void UpdateSFXVolumeFromSlider(float volume) {
        Variables.sfxVolume = volume;
    }
}
