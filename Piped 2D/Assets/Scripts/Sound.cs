using UnityEngine;

/**
 * A public class for assigning the sound clips inside the Unity Editor together with a custom name.
 */
[System.Serializable]
public class Sound
{
    /**
     * A public string field for the name of the sound clip.
     */
    public string name;

    /**
    * A public AudioClip field for referencing the sound clip.
    */
    public AudioClip clip;
}
