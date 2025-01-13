using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * A class handling the Credits scene.
 */
public class Credits : MonoBehaviour
{
    /**
     * A private TextMeshProUGUI field for referencing the text of the credits.
     * Serialized in order to access it from the Unity editor.
     */
    [SerializeField]
    TextMeshProUGUI content;

    /**
    * A private void method called when the script instance is being loaded. It sets necessary variables.
    */
    private void Start()
    {
        content.text = "<b>Game inspired by: </b>\n" +
            "<color=#BF9453>Turing Tumble by Upper Story</color>" +
            "\n\n" +
            "<b>Sound effects: </b>\n" +
            "freesound_community on Pixaba\n" +
            "DavidDumaisAudio on Pixabay" +
            "\n\n" +
            "<b>Music:</b>\n" +
            "Gameplay: freesound_community on Pixabay\n" +
            "Menu: AmbientAUDIOVISION on Pixabay" +
            "\n\n" +
            "<b>Fonts:</b>\n" +
            "OldStyle by HPLHS Prop Fonts,\n" +
            "Angot by Tesfaye Dessalegn";
    }

    /**
    * A private void method called every frame.
    */
    void Update()
    {
        // Detect key press
        if (Input.anyKeyDown)
        {
            GameManager.Instance.LoadMainMenu();
        }
    }
}
