using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEngine.Rendering.DebugUI;
using Unity.VisualScripting;

public enum Material
{
    Glass,
    Metal
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sxfSounds;

    [SerializeField]
    AudioClip[] marbleGlassHitSounds;

    [SerializeField]
    AudioClip[] marbleMetalHitSounds;

    [SerializeField]
    AudioClip[] bitRotateSounds;

    [SerializeField]
    AudioClip[] blockPlaceSounds;

    public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        string musicVolumeStr = PlayerPrefs.GetString("MusicVolume", "");
        float musicVolume;
        string sfxVolumeStr = PlayerPrefs.GetString("SfxVolume", "");
        float sfxVolume;

        if (musicVolumeStr != "")
        {
            if (float.TryParse(musicVolumeStr, out musicVolume))
            {
                musicSource.volume = musicVolume;
            }
            else
            {
                Debug.Log("Could not parse the saved music volume string, returning to default setting.");
            }
        }

        if (sfxVolumeStr != "")
        {
            if (float.TryParse(sfxVolumeStr, out sfxVolume))
            {
                sfxSource.volume = sfxVolume;
            }
            else
            {
                Debug.Log("Could not parse the saved sound effects volume string, returning to default setting.");
            }
        }
    }


    public void playMusic(string name, bool loop)
    {
        Sound music = Array.Find(musicSounds, m => m.name == name);

        if(music == null)
        {
            Debug.LogError("Sound with name " + name + " not found");
            return;
        }

        musicSource.clip = music.clip;
        musicSource.Play();
        musicSource.loop = loop;
    }

    public void playSfx(string name, bool withRandomVolume = false)
    {
        Sound sfx = Array.Find(sxfSounds, s => s.name == name);

        if (sfx.clip == null)
        {
            Debug.LogError("Sound with name " + name + " not found");
            return;
        }

        sfxSource.clip = sfx.clip;

        if (withRandomVolume)
        {
            //sfxSource.volume = sfxSource.volume * UnityEngine.Random.Range(0.5f, 1f);
            sfxSource.PlayOneShot(sfx.clip, UnityEngine.Random.Range(0.8f, 1f));
        }
        else
        {
            sfxSource.PlayOneShot(sfx.clip);

        }

    }

    public void playMenuMusic()
    {
        playMusic("MenuTheme", true);
    }
    public void stopMusic()
    {
        musicSource.Stop();
    }


    public void toggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }
    public void toggleSfx()
    {
        sfxSource.mute = !sfxSource.mute;
    }
    public void changeMusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetString("MusicVolume", volume.ToString());
    }
    public void changeSfxVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetString("SfxVolume", volume.ToString());
    }

    public void playRandomMarbleHitSfx(float volumeMultiplier, Material material)
    {
        if (marbleGlassHitSounds.Length < 1 || marbleMetalHitSounds.Length < 1)
        {
            Debug.Log("<color=red>No marble hit sounds found in one of the arrays!");
            return;
        }

        AudioSource audioSource = Instantiate(sfxSource, Vector3.zero, Quaternion.identity);

        int rand;
        switch (material)
        {
            case Material.Glass:
                rand = UnityEngine.Random.Range(0, marbleGlassHitSounds.Length);
                audioSource.clip = marbleGlassHitSounds[rand];
                break;
            case Material.Metal:
                rand = UnityEngine.Random.Range(0, marbleMetalHitSounds.Length);
                audioSource.clip = marbleMetalHitSounds[rand];
                break;
        }
        audioSource.volume = volumeMultiplier * audioSource.volume;
        audioSource.pitch = sfxSource.pitch;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }


    public void playRandomBitRotateSfx()
    {
        if (bitRotateSounds.Length < 1)
        {
            Debug.Log("<color=red>No bit rotate sounds found in the array!");
            return;
        }

        AudioSource audioSource = Instantiate(sfxSource, Vector3.zero, Quaternion.identity);

        int rand;
        rand = UnityEngine.Random.Range(0, bitRotateSounds.Length);
        audioSource.clip = bitRotateSounds[rand];
        audioSource.volume = sfxSource.volume * 0.5f;
        audioSource.pitch = sfxSource.pitch;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    public void playRandomBlockPlaceSfx()
    {
        if (bitRotateSounds.Length < 1)
        {
            Debug.Log("<color=red>No bit rotate sounds found in the array!");
            return;
        }

        AudioSource audioSource = Instantiate(sfxSource, Vector3.zero, Quaternion.identity);

        int rand;
        rand = UnityEngine.Random.Range(0, blockPlaceSounds.Length);
        audioSource.clip = blockPlaceSounds[rand];
        audioSource.volume = audioSource.volume * 0.2f;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }


    public void playMarbleShatterSfx()
    {
        playSfx("MarbleShatter");
    }

    public void changeSfxPitch(float pitch)
    {
        sfxSource.pitch = pitch;
    }

}
