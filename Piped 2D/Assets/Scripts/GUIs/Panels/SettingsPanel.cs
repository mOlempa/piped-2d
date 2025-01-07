using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField]
    Slider musicSlider, sfxSlider;

    [SerializeField]
    Button musicButton, sfxButton;

    string iconsPath = "Images/Icons/";

    public void closeSettings()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        musicSlider.value = AudioManager.Instance.musicSource.volume;
        sfxSlider.value = AudioManager.Instance.sfxSource.volume;
    }

    private void OnEnable()
    {
        musicSlider.value = AudioManager.Instance.musicSource.volume;
        sfxSlider.value = AudioManager.Instance.sfxSource.volume;
    }

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

    public void changeMusicVolume()
    {
        AudioManager.Instance.changeMusicVolume(musicSlider.value);
    }

    public void changeSfxVolume()
    {
        AudioManager.Instance.changeSfxVolume(sfxSlider.value);
    }
}
