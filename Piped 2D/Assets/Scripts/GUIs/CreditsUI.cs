using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI content;

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

    // Detect any key pressed
    void Update()
    {
        if (Input.anyKeyDown)
        {
            GameManager.Instance.LoadMainMenu();
        }
    }
}
