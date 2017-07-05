using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptMenu : MonoBehaviour {

    public Button exitOptions;

    public Slider sfxVol, musVol;
    
    public Toggle floatyControls;

    AudioSource musAS;

    Player player;

    // Use this for initialization
    void Start () {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        //Options Menu Stuff
        
        musAS = GameObject.FindWithTag("Background").GetComponent<AudioSource>();
        musVol.onValueChanged.AddListener(this.UpdateMusicVolumeFromSlider);
        
        floatyControls.onValueChanged.AddListener(updateControllerScheme);

        exitOptions.onClick.AddListener(OptionsMenu);

        //Settings values already set
        musVol.value = musAS.volume;
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
        musAS.volume = volume;
    }

    public void UpdateSFXVolumeFromSlider(float volume) {
        //TODO
    }

    public void updateControllerScheme(bool isSpaceLike) {
        player.isSpaceLike = isSpaceLike;
        Variables.isSpaceLike = isSpaceLike;
    }
}
