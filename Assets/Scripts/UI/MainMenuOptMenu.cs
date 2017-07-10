using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuOptMenu : MonoBehaviour {

    public Button exitOptions;

    public Slider sfxVol, musVol;
    
    public Toggle floatyControls;

    AudioSource musAS;

    // Use this for initialization
    void Start () {

        //Options Menu Stuff
        musVol.onValueChanged.AddListener(this.UpdateMusicVolumeFromSlider);

        sfxVol.onValueChanged.AddListener(this.UpdateSFXVolumeFromSlider);

        floatyControls.onValueChanged.AddListener(updateControllerScheme);

        exitOptions.onClick.AddListener(OptionsMenu);

        //Settings values already set
        floatyControls.isOn = Variables.isSpaceLike;

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

    public void updateControllerScheme(bool isSpaceLike) {
        Variables.isSpaceLike = isSpaceLike;
    }
}
