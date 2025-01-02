using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    GameObject settingsPanel;

    private void Start()
    {
        Screen.SetResolution(1920, 1080, true);

        // Activate the panel for adding listeners to the buttons
        settingsPanel.SetActive(true);

        foreach (GameObject buttonObject in GameObject.FindGameObjectsWithTag("Button"))
        {
            Button button = buttonObject.GetComponent<Button>();
            button.onClick.AddListener(() => deselectButton());
            button.onClick.AddListener(() => AudioManager.Instance.playSfx("RegularButton"));
        }

        // Deactivate the panel after all the button listeners were added
        settingsPanel.SetActive(false);

        AudioManager.Instance.playMenuMusic();
    }

    void deselectButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
