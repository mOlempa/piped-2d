using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Color = UnityEngine.Color;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    InstructionPanel instructionPanelScript;

    [SerializeField]
    GameObject settingsPanel;

    [SerializeField]
    GameObject tutorialPanel;

    [SerializeField]
    GameObject hintPanel;

    [Header("Menu UI")]
    [SerializeField]
    Button instructionButton;

    [SerializeField]
    Button menuButton;

    [SerializeField]
    Button tutorialButton;

    [SerializeField]
    TextMeshProUGUI levelText;

    [Header("UI")]
    [SerializeField]
    Button switchStartingSideButton;

    [SerializeField]
    Button toggleGameSpeedUpButton;

    [SerializeField]
    Button toggleSystemButton;

    [SerializeField]
    Button togglePauseButton;

    [SerializeField]
    Button deleteTestBallsButton;

    [SerializeField]
    Button nextLevelButton;

   

    //bool gameSpeedUp = false;
    public bool gamePause = false;

    bool gameToggleBlocked = false;

    [Header("Block Buttons")]
    public GameObject passButtonObject;
    public GameObject crossoverButtonObject;
    public GameObject bitButtonObject;
    public GameObject stopperButtonObject;
    public GameObject gearButtonObject;

    private Button passButton;
    private TextMeshProUGUI passCounterText;
    private Button crossoverButton;
    private TextMeshProUGUI crossoverCounterText;
    private Button bitButton;
    private TextMeshProUGUI bitCounterText;
    private Button stopperButton;
    private TextMeshProUGUI stopperCounterText;
    private Button gearButton;
    private TextMeshProUGUI gearCounterText;


    private string iconsPath = "Images/Icons/";
    //Color yellowishTint = new Color(0.2f, 0.2f, 0.1f);

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

        /*updateButtonColorBlock(switchStartingSideButton);
        updateButtonColorBlock(toggleGameSpeedUpButton);
        updateButtonColorBlock(toggleSystemButton);
        updateButtonColorBlock(togglePauseButton);
        updateButtonColorBlock(deleteTestBallsButton);*/

        tutorialPanel.SetActive(true);
        instructionPanelScript.gameObject.SetActive(true);

        // Make all buttons able to deselect after being clicked
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Button");
        foreach(GameObject buttonObject in buttons)
        {
            Button button = buttonObject.GetComponent<Button>();
            button.onClick.AddListener(() => deselectButton());
            button.onClick.AddListener(() => AudioManager.Instance.playSfx("RegularButton"));
            //updateButtonColorBlock(button);
        }

        instructionPanelScript.hideInstruction();
        settingsPanel.SetActive(false);
        hintPanel.SetActive(false);

        levelText.text = "Level "+ ((int)GameManager.Instance.currentLevel + 1);
        StartCoroutine(setTutorialButton());
        StartCoroutine(blockGameToggle());
    }

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

    void deselectButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
    void deselectButtons()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }


    private void hideAllButtons()
    {
        passButton.gameObject.SetActive(false);
        crossoverButton.gameObject.SetActive(false);
        bitButton.gameObject.SetActive(false);
        stopperButton.gameObject.SetActive(false);
        gearButton.gameObject.SetActive(false);

        switchStartingSideButton.gameObject.SetActive(false);

        showNextLevelButton(false);

        //nextLevelButton.gameObject.SetActive(false);
    }

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

    private TextMeshProUGUI getCounterTextByBlockType(BlockType blockType)
    {
        switch (blockType)
        {
            case BlockType.Pass:
                return passCounterText;
            case BlockType.Crossover:
                return crossoverCounterText;
            case BlockType.Stopper:
                return stopperCounterText;
            case BlockType.Bit:
                return bitCounterText;
            case BlockType.Gear:
                return gearCounterText;
            default:
                Debug.LogError("ERROR: getCounterTextByBlockType - Wrong block type! Returning null");
                return null;
        }
    }


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

    public void showNextLevelButton(bool show)
    {
        nextLevelButton.gameObject.SetActive(show);
        if(GameManager.Instance.currentLevel == Enum.GetValues(typeof(Level)).Cast<Level>().Last())
        {
            nextLevelButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Finish Game";
        }
    }

    // Cycle the speed of the game with UI button
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

    // Accessed by a button, redirects to GameManager function since button didn't work well with GameManager singleton --> kept returning 0
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

    public void deleteTestBalls()
    {
        GameManager.Instance.deleteTestBalls();
    }

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

    public void toggleSuperSpeed()
    {
        GameManager.Instance.superSpeedUpGame();
    }

    public void loadNextLevel()
    {
        GameManager.Instance.LoadNextLevel();
    }
    public void reloadLevel()
    {
        GameManager.Instance.ReloadLevel();
    }
    public void loadLevelSelection()
    {
        GameManager.Instance.LoadLevelSelection();
    }
    public void loadMainMenu()
    {
        GameManager.Instance.LoadMainMenu();
    }

    public void openTutorial()
    {
        tutorialPanel.GetComponent<TutorialPanel>().openTutorial();
        //tutorialPanel.SetActive(true);
    }

}
