using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    GameObject settingsPanel;

    [SerializeField]
    GameObject creditsPanel;

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
