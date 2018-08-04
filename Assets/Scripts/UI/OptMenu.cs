using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptMenu : MonoBehaviour {

    [SerializeField] Button exitOptions;
    [SerializeField] Slider sfxVol, musVol;
    [SerializeField] Toggle lightingOn;
    [SerializeField] GameObject lighting;

    AudioSource musAS;
    
    // Use this for initialization
    void Start () {

        //Options Menu Stuff
        musAS = GameObject.FindWithTag("Background").GetComponent<AudioSource>();

        musVol.onValueChanged.AddListener(this.UpdateMusicVolumeFromSlider);
        lightingOn.onValueChanged.AddListener(Lights);
        exitOptions.onClick.AddListener(OptionsMenu);

        //Settings values already set
        musVol.value = Variables.musicVolume;
        musAS.volume = Variables.musicVolume;

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
        
    }

    void Lights(bool isOn) {
        lighting.SetActive(isOn);
    }
}
