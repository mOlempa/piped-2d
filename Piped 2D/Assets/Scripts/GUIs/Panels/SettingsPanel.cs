using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * A class controlling the settings panel in the UI within the Level scene, MainMenu scene and LevelSelection scene
 */
public class SettingsPanel : MonoBehaviour
{
    /**
      * A private Slider field for referencing the sliders for sound effects and music volume adjustment.
      * Serialized in order to access it from the Unity editor.
      */
    [SerializeField]
    Slider musicSlider, sfxSlider;

    /**
      * A private Button field for referencing the buttons for muting and unmuting music and sound effects.
      * Serialized in order to access it from the Unity editor.
      */
    [SerializeField]
    Button musicButton, sfxButton;

    [SerializeField]
    TMP_Dropdown languageDropDown;

    /**
      * A private string field to store the path to the icons for the buttons in the assets folder.
      */
    string iconsPath = "Images/Icons/";

    public void closeSettings()
    {
        gameObject.SetActive(false);
    }

    /*public void changeLanguage()
    {
        Language lan;
        switch (languageDropDown.value)
        {
            case 0:
                lan = Language.English; break;
            case 1:
                lan = Language.Polski; break;
            default:
                lan = Language.English; break;
        }
        
        GameManager.Instance.currentLanguage = lan;
    }*/

    /**
     * A private void member function called at the start of the scene with the settings panel.
     * It sets the slider values.
     */
    private void Start()
    {
        musicSlider.value = AudioManager.Instance.musicSource.volume;
        sfxSlider.value = AudioManager.Instance.sfxSource.volume;
    }

    /**
    * A private void member function called on enabling the settings panel.
    * It sets the slider values.
    */
    private void OnEnable()
    {
        musicSlider.value = AudioManager.Instance.musicSource.volume;
        sfxSlider.value = AudioManager.Instance.sfxSource.volume;
    }

    /**
    * A public void member function toggling the music, assigned to the music button in the settings panel.
    */
    public void toggleMusic()
    {
        AudioManager.Instance.toggleMusic();
        if (AudioManager.Instance.musicSource.mute)
        {
            musicButton.image.sprite = Resources.Load<Sprite>(iconsPath + "music_off_icon");
        }
        else
        {
            musicButton.image.sprite = Resources.Load<Sprite>(iconsPath + "music_on_icon");
        }
    }

    /**
    * A public void member function toggling the sound effects, assigned to the sfx button in the settings panel.
    */
    public void toggleSfx()
    {
        AudioManager.Instance.toggleSfx();
        if (AudioManager.Instance.sfxSource.mute)
        {
            sfxButton.image.sprite = Resources.Load<Sprite>(iconsPath + "sound_off_icon");
        }
        else
        {
            sfxButton.image.sprite = Resources.Load<Sprite>(iconsPath + "sound_on_icon");
        }
    }

    /**
    * A public void member function adjusting the music volume, called on every change of the music volume slider.
    */
    public void changeMusicVolume()
    {
        AudioManager.Instance.changeMusicVolume(musicSlider.value);
    }

    /**
   * A public void member function adjusting the sound effects volume, called on every change of the sfx volume slider.
   */
    public void changeSfxVolume()
    {
        AudioManager.Instance.changeSfxVolume(sfxSlider.value);
    }
}
