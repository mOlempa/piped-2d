using UnityEngine;
using System;

/**
 * An enum for differentiating between marbles hitting each other and the blocks
 */
public enum HitMaterial
{
    Glass,
    Metal
}

/**
 * A class controlling the audio of the game
 */
public class AudioManager : MonoBehaviour
{
    /**
     * A public instance of the AudioManager singleton
     */
    public static AudioManager Instance;

    /**
     * A public array of Sound objects to be defined inside the Unity Editor
     */
    public Sound[] musicSounds, sxfSounds;

    /**
    * A private AudioClip array for referencing the sounds for marbles hitting each other
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    AudioClip[] marbleGlassHitSounds;

    /**
    * A private AudioClip array for referencing the sounds for marbles hitting other blocks
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    AudioClip[] marbleMetalHitSounds;

    /**
    * A private AudioClip array for referencing the sounds for rotating ramps of the bits.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    AudioClip[] bitRotateSounds;

    /**
    * A private AudioClip array for referencing the sounds for placing the blocks.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    AudioClip[] blockPlaceSounds;

    /**
    * Public AudioSources for referencing the sources of the music and sound effects inside the scene.
    */
    public AudioSource musicSource, sfxSource;

    /**
    * A private void method called when the script instance is being loaded.
    */
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

    /**
    * A private void method called when the script instance is being loaded. It sets necessary variables.
    */
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

    /**
    * A public void method that plays music from the referenced music source by the name.
    * 
    * @param name of the music defined in the Unity Editor.
    * @param loop defines whether the music should play on loop or not.
    */
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

    /**
    * A public void method that plays sound effects from the referenced sfx source by the name.
    * 
    * @param name of the sfx defined in the Unity Editor.
    * @param withRandomVolume defines whether the sfx should play with volume random between two values.
    */
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
            sfxSource.PlayOneShot(sfx.clip, UnityEngine.Random.Range(0.8f, 1f));
        }
        else
        {
            sfxSource.PlayOneShot(sfx.clip);

        }

    }

    /**
     * A public void method that plays the menu music.
     */
    public void playMenuMusic()
    {
        playMusic("MenuTheme", true);
    }
    /**
     * A public void method that plays the game music.
     */
    public void playGameMusic()
    {
        playMusic("GameTheme", true);
    }
    /**
     * A public void method that stops the music.
     */
    public void stopMusic()
    {
        musicSource.Stop();
    }

    /**
     * A public void method that mutes or unmutes the music (toggles the music).
     */
    public void toggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    /**
     * A public void method that mutes or unmutes the sound effects (toggles the sound effects).
     */
    public void toggleSfx()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    /**
     * A public void method that changes the volume of the music.
     * 
     * @param volume of the music to be changed to as a float value.
     */
    public void changeMusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetString("MusicVolume", volume.ToString());
    }

    /**
     * A public void method that changes the volume of the sound effects.
     * 
     * @param volume of the sound effects to be changed to as a float value.
     */
    public void changeSfxVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetString("SfxVolume", volume.ToString());
    }


    /**
     * A public void method that plays a random sound of the marble hitting something.
     * 
     * @param volumeMultiplier of the sound effects volume to be multiplied by based on the strength of the hit.
     * @param material that the marble is hitting, either HitMaterial.Glass or HitMaterial.Metal.
     */
    public void playRandomMarbleHitSfx(float volumeMultiplier, HitMaterial material)
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
            case HitMaterial.Glass:
                rand = UnityEngine.Random.Range(0, marbleGlassHitSounds.Length);
                audioSource.clip = marbleGlassHitSounds[rand];
                break;
            case HitMaterial.Metal:
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

    /**
     * A public void method that plays a random sound of the bit rotating.
     */
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

    /**
     * A public void method that plays a random sound of the block placing.
     */
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

    /**
     * A public void method that plays a sound of marble shattering.
     */
    public void playMarbleShatterSfx()
    {
        playSfx("MarbleShatter");
    }

    /**
     * A public void method that changes the pitch of the sound played, used when changing the speed of the game.
     * 
     * @param pitch for the audio to be changed to as a float value.
     */
    public void changeSfxPitch(float pitch)
    {
        sfxSource.pitch = pitch;
    }

}
