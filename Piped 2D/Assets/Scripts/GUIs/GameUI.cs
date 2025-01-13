using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Color = UnityEngine.Color;

/**
 * A class responsible for handling the Game UI.
 */
public class GameUI : MonoBehaviour
{
    /**
    * A private InstructionPanel field for referencing the script of the instruction panel.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    InstructionPanel instructionPanelScript;

    /**
    * A private GameObject field for referencing the settings panel.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    GameObject settingsPanel;

    /**
    * A private GameObject field for referencing the tutorial panel.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    GameObject tutorialPanel;

    /**
    * A private GameObject field for referencing the hint panel.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    GameObject hintPanel;

    [Header("Menu UI")]
    /**
    * A private Button field for referencing the Instruction button.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    Button instructionButton;

    /**
    * A private Button field for referencing the Main Menu button.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    Button menuButton;

    /**
    * A private Button field for referencing the Tutorial button.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    Button tutorialButton;

    /**
    * A private TextMeshProUGUI field for referencing the text of the level.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    TextMeshProUGUI levelText;

    [Header("UI")]
    /**
    * A private Button field for referencing the Switch Sides button.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    Button switchStartingSideButton;

    /**
    * A private Button field for referencing the Speed button.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    Button toggleGameSpeedUpButton;

    /**
    * A private Button field for referencing the simulation button.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    Button toggleSystemButton;

    /**
    * A private Button field for referencing the Pause button.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    Button togglePauseButton;

    /**
    * A private Button field for referencing the Delete Test Marbles button.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    Button deleteTestBallsButton;

    /**
    * A private Button field for referencing the Next Level button.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    Button nextLevelButton;

   
    /**
     * A public bool field that informs whether the game is paused or not.
     */
    public bool gamePause = false;

    /**
     * A private bool field that informs whether the simulation toggle is blocked or not.
     */
    bool gameToggleBlocked = false;

    [Header("Block Buttons")]
    /**
    * A public GameObject field for referencing the Pass button object.
    */
    public GameObject passButtonObject;

    /**
    * A public GameObject field for referencing the Crossover button object.
    */
    public GameObject crossoverButtonObject;

    /**
    * A public GameObject field for referencing the Bit button object.
    */
    public GameObject bitButtonObject;

    /**
    * A public GameObject field for referencing the Stopper button object.
    */
    public GameObject stopperButtonObject;

    /**
    * A public GameObject field for referencing the Gear button object.
    */
    public GameObject gearButtonObject;

    /**
    * A public Button field for referencing the Pass button.
    */
    private Button passButton;

    /**
    * A public TextMeshProUGUI field for referencing the Pass text.
    */
    private TextMeshProUGUI passCounterText;

    /**
    * A public Button field for referencing the Crossover button.
    */
    private Button crossoverButton;

    /**
    * A public TextMeshProUGUI field for referencing the Crossover text.
    */
    private TextMeshProUGUI crossoverCounterText;

    /**
    * A public Button field for referencing the Bit button.
    */
    private Button bitButton;

    /**
    * A public TextMeshProUGUI field for referencing the Bit text.
    */
    private TextMeshProUGUI bitCounterText;

    /**
    * A public Button field for referencing the Stopper button.
    */
    private Button stopperButton;

    /**
    * A public TextMeshProUGUI field for referencing the Stopper text.
    */
    private TextMeshProUGUI stopperCounterText;

    /**
    * A public Button field for referencing the Gear button.
    */
    private Button gearButton;

    /**
    * A public TextMeshProUGUI field for referencing the Gear text.
    */
    private TextMeshProUGUI gearCounterText;


    /**
    * A public string field for storing the path to the icons.
    */
    private string iconsPath = "Images/Icons/";

    /**
    * A private void method called when the script instance is being loaded. It sets necessary variables.
    */
    private void Start()
    {
        settingsPanel.SetActive(true);
        hintPanel.SetActive(true);

        passButton = passButtonObject.GetComponent<Button>();
        passCounterText = passButtonObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        crossoverButton = crossoverButtonObject.GetComponent<Button>();
        crossoverCounterText = crossoverButtonObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        bitButton = bitButtonObject.GetComponent<Button>();
        bitCounterText = bitButtonObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        stopperButton = stopperButtonObject.GetComponent<Button>();
        stopperCounterText = stopperButtonObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        gearButton = gearButtonObject.GetComponent<Button>();
        gearCounterText = gearButtonObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        GameManager.Instance.resetGameSpeed();
        toggleGameSpeedUpButton.image.sprite = Resources.Load<Sprite>(iconsPath + "clock_icon");

        tutorialPanel.SetActive(true);
        instructionPanelScript.gameObject.SetActive(true);

        // Make all buttons able to deselect after being clicked
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Button");
        foreach(GameObject buttonObject in buttons)
        {
            Button button = buttonObject.GetComponent<Button>();
            button.onClick.AddListener(() => deselectButton());
            button.onClick.AddListener(() => AudioManager.Instance.playSfx("RegularButton"));
        }

        instructionPanelScript.hideInstruction();
        settingsPanel.SetActive(false);
        hintPanel.SetActive(false);

        // Change number of the level in the UI, because it 41 and 47 have switched places in the plot
        switch (GameManager.Instance.currentLevel)
        {
            case Level.Level41:
                levelText.text = "Level 47";
                break;
            case Level.Level47:
                levelText.text = "Level 41";
                break;
            default:
                levelText.text = "Level " + ((int)GameManager.Instance.currentLevel + 1);
                break;
        }
        StartCoroutine(setTutorialButton());
        StartCoroutine(blockGameToggle());
    }

    /**
    * A private coroutine that enables the Tutorial button if the level has a tutorial.
    */
    IEnumerator setTutorialButton()
    {
        while (tutorialPanel.GetComponent<TutorialPanel>().thisLevelHasTutorial == null)
        {
            yield return null;
        }
        if ((bool)tutorialPanel.GetComponent<TutorialPanel>().thisLevelHasTutorial)
        {
            tutorialButton.gameObject.SetActive(true);
        }
        else
        {
            tutorialButton.gameObject.SetActive(false);
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
     * A private void method that deselects the buttons.
     */
    void deselectButtons()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    /**
     * A private void method that hides all the buttons.
     */
    private void hideAllButtons()
    {
        passButton.gameObject.SetActive(false);
        crossoverButton.gameObject.SetActive(false);
        bitButton.gameObject.SetActive(false);
        stopperButton.gameObject.SetActive(false);
        gearButton.gameObject.SetActive(false);

        switchStartingSideButton.gameObject.SetActive(false);

        showNextLevelButton(false);
    }

    /**
     * A public void method that sets the buttons available for the level.
     */
    public void setAvailableButtons()
    {
        passButton = passButtonObject.GetComponent<Button>();
        passCounterText = passButtonObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        crossoverButton = crossoverButtonObject.GetComponent<Button>();
        crossoverCounterText = crossoverButtonObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        bitButton = bitButtonObject.GetComponent<Button>();
        bitCounterText = bitButtonObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        stopperButton = stopperButtonObject.GetComponent<Button>();
        stopperCounterText = stopperButtonObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        gearButton = gearButtonObject.GetComponent<Button>();
        gearCounterText = gearButtonObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        hideAllButtons();

        if(LevelManager.Instance.getLevelStartingSide() == Ball.Neither)
        {
            switchStartingSideButton.gameObject.SetActive(true);
        }
         
        foreach (KeyValuePair<BlockType, int> entry in LevelManager.Instance.getLevelBlocks())
        {
            switch(entry.Key)
            {
                case BlockType.Pass:
                    passButton.gameObject.SetActive(true);
                    passCounterText.SetText(entry.Value.ToString());
                    break;
                case BlockType.Crossover:
                    crossoverButton.gameObject.SetActive(true);
                    crossoverCounterText.SetText(entry.Value.ToString());
                    break;
                case BlockType.Bit:
                    bitButton.gameObject.SetActive(true);
                    bitCounterText.SetText(entry.Value.ToString());
                    break;
                case BlockType.Stopper:
                    stopperButton.gameObject.SetActive(true);
                    stopperCounterText.SetText(entry.Value.ToString());
                    break;
                case BlockType.Gear:
                    gearButton.gameObject.SetActive(true);
                    gearCounterText.SetText(entry.Value.ToString());
                    break;
            }
        }
    }

    /**
     * A public void method that updates the buttons available for the level.
     */
    public void updateAvailableBlockButtons()
    {
        foreach(KeyValuePair<BlockType, int> entry in GameManager.Instance.availableBlocksCopy)
        {
            switch (entry.Key)
            {
                case BlockType.Pass:
                    passCounterText.SetText(entry.Value.ToString());
                    break;
                case BlockType.Crossover:
                    crossoverCounterText.SetText(entry.Value.ToString());
                    break;
                case BlockType.Bit:
                    bitCounterText.SetText(entry.Value.ToString());
                    break;
                case BlockType.Stopper:
                    stopperCounterText.SetText(entry.Value.ToString());
                    break;
                case BlockType.Gear:
                    gearCounterText.SetText(entry.Value.ToString());
                    break;
            }
        }
    }

    /**
     * A public void method that toggles the instruction.
     */
    public void toggleInstruction()
    {
        if (instructionPanelScript.instructionVisible)
        {
            instructionPanelScript.hideInstruction();
        }
        else
        {
            //(string title, string instruction) tuple = LevelManager.Instance.getLevelInstruction();
            instructionPanelScript.showInstruction();
        }

    }

    /**
     * A public void method that shows the Next Level button
     */
    public void showNextLevelButton(bool show)
    {
        nextLevelButton.gameObject.SetActive(show);
        if(GameManager.Instance.currentLevel == Enum.GetValues(typeof(Level)).Cast<Level>().Last())
        {
            nextLevelButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Finish Game";
        }
    }

    /**
     * A public void method that cycles through the speed of the game.
     */
    public void cycleSpeed()
    {
        // If game was at regular speed, speed it up
        if (GameManager.Instance.currentGameSpeed == GameSpeed.Regular)
        {
            GameManager.Instance.speedUpGame();
            updateSpeedButon();
            //toggleGameSpeedUpButton.image.sprite = Resources.Load<Sprite>(iconsPath + "speed_up_icon");
        }
        else
        {
            // If game was sped up, make it slower than regular
            if(GameManager.Instance.currentGameSpeed == GameSpeed.Fast)
            {
                GameManager.Instance.slowDownGame();
                updateSpeedButon();
                //toggleGameSpeedUpButton.image.sprite = Resources.Load<Sprite>(iconsPath + "clock_snail_icon");
            }
            // If game was slower than regular, make it regular speed
            else
            {
                GameManager.Instance.resetGameSpeed();
                updateSpeedButon();
                //toggleGameSpeedUpButton.image.sprite = Resources.Load<Sprite>(iconsPath + "clock_icon");
            }
        }
        // Deselection with button listeners for some reason doesn't work if sprite is changing
        deselectButtons();
    }

    /**
     * A public void method that updates the Speed button.
     */
    public void updateSpeedButon()
    {
        if (GameManager.Instance.currentGameSpeed == GameSpeed.Regular)
        {
            toggleGameSpeedUpButton.image.sprite = Resources.Load<Sprite>(iconsPath + "clock_icon");
        }
        else
        {
            if (GameManager.Instance.currentGameSpeed == GameSpeed.Fast)
            {
                toggleGameSpeedUpButton.image.sprite = Resources.Load<Sprite>(iconsPath + "speed_up_icon");
            }
            else
            {
                toggleGameSpeedUpButton.image.sprite = Resources.Load<Sprite>(iconsPath + "clock_snail_icon");
            }
        }
    }

    /**
     * A public void method that toggles pause of the game.
     */
    public void toggleGamePause()
    {
        if (gamePause)
        {
            gamePause = false;
            if (GameManager.Instance.currentGameSpeed == GameSpeed.Regular)
            {
                GameManager.Instance.resetGameSpeed();
            }
            else
            {
                if (GameManager.Instance.currentGameSpeed == GameSpeed.Fast)
                {
                    GameManager.Instance.speedUpGame();
                }
                else
                {
                    GameManager.Instance.slowDownGame();
                }
            }
            togglePauseButton.image.sprite = Resources.Load<Sprite>(iconsPath + "pause_icon");

        }
        else
        {
            gamePause = true;
            GameManager.Instance.pauseGameSpeed();
            togglePauseButton.image.sprite = Resources.Load<Sprite>(iconsPath + "play_icon");
        }
    }

    /**
     * A public void method that toggles the simulation
     */
    public void toggleGame()
    {
        if (gameToggleBlocked)
        {
            return;
        }
        StartCoroutine(blockGameToggle());
        if (GameManager.Instance.marblesAreRunning)
        {
            GameManager.Instance.resetSimulation();
            toggleSystemButton.image.sprite = Resources.Load<Sprite>(iconsPath + "cog_play_icon");
        }
        else
        {
            GameManager.Instance.runSimulation();
            toggleSystemButton.image.sprite = Resources.Load<Sprite>(iconsPath + "cog_square_icon");
        }
    }

    /**
     * A private coroutine that blocks the player from toggling the simulation for a short while.
     */
    IEnumerator blockGameToggle()
    {
        //print("<color=green>Coroutine started");
        gameToggleBlocked = true;
        toggleSystemButton.interactable = false;
        while (GameManager.Instance.marblesAreSpawning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1);
        gameToggleBlocked = false;
        toggleSystemButton.interactable = true;
        //print("<color=red>Coroutine finished");
    }

    /**
     * A public void method that calls a method from Game Manager to delete the test marbles.
     */
    public void deleteTestBalls()
    {
        GameManager.Instance.deleteTestBalls();
    }

    /**
     * A public void method that updates the colors for the buttons.
     */
    public void updateButtonColorBlock(Button button)
    {
        Color color = Color.white;
        ColorBlock cb = button.colors;
        cb.normalColor = color;
        float highlight = 0.2f;
        float pressed = 0.3f;
        if (color.r + color.b + color.g < 0.25)
        {
            highlight *= -0.5f;
            pressed *= -1;
        }
        cb.highlightedColor = new Color(color.r - highlight, color.g - highlight, color.b - highlight + 0.2f);
        cb.selectedColor = cb.normalColor;
        cb.pressedColor = new Color(color.r - pressed, color.g - pressed, color.b - pressed);
        cb.disabledColor = new Color(cb.pressedColor.r, cb.pressedColor.g, cb.pressedColor.b, 0.5f);

        button.colors = cb;
    }

    /**
     * A public void method that calls a method from Game Manager that changes the levels.
     */
    public void loadNextLevel()
    {
        GameManager.Instance.LoadNextLevel();
    }

    /**
     * A public void method that calls a method from Game Manager that reloads the level.
     */
    public void reloadLevel()
    {
        GameManager.Instance.ReloadLevel();
    }

    /**
     * A public void method that calls a method from Game Manager that moves the player to Level Selection Menu.
     */
    public void loadLevelSelection()
    {
        GameManager.Instance.LoadLevelSelection();
    }

    /**
     * A public void method that calls a method from Game Manager that moves the player to Main Menu.
     */
    public void loadMainMenu()
    {
        GameManager.Instance.LoadMainMenu();
    }

    /**
     * A public void method that opens a tutorial.
     */
    public void openTutorial()
    {
        tutorialPanel.GetComponent<TutorialPanel>().openTutorial();
    }

}
