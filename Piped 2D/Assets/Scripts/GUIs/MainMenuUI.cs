using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
 * 
 */
public class MainMenuUI : MonoBehaviour
{
    /**
    * A private GameObject field for referencing the settings panel.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    GameObject settingsPanel;

    /**
    * A private GameObject field for referencing the credits panel.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    GameObject creditsPanel;

    /**
    * A private void method called when the script instance is being loaded. It sets necessary variables.
    */
    private void Start()
    {
        Screen.SetResolution(1920, 1080, true);

        // Activate the panels for adding listeners to the buttons
        settingsPanel.SetActive(true);
        creditsPanel.SetActive(true);

        foreach (GameObject buttonObject in GameObject.FindGameObjectsWithTag("Button"))
        {
            Button button = buttonObject.GetComponent<Button>();
            button.onClick.AddListener(() => deselectButton());
            button.onClick.AddListener(() => AudioManager.Instance.playSfx("RegularButton"));
        }

        // Deactivate the panels after all the button listeners were added
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);

        if (!AudioManager.Instance.musicSource.isPlaying)
        {
            AudioManager.Instance.playMenuMusic();

        }
        else
        {
            print("Music is playing already");
        }
    }

    /**
     * A private void method that deselects the buttons.
     */
    void deselectButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    /**
     * A public void method that quits the program.
     */
    public void quitGame()
    {
        Application.Quit();
    }
}
