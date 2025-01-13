using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
 * A class responsible for handling the Level Selection Menu UI.
 */
public class ChapterUI : MonoBehaviour
{
    /**
    * A private TextMeshProUGUI field for referencing the text of the chapter.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    TextMeshProUGUI chapterText;

    /**
    * A private Button field for referencing the Next button of the chapters.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    Button nextChapterButton;

    /**
    * A private Button field for referencing the Previous button of the chapters.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    Button previousChapterButton;

    /**
    * A private TextMeshProUGUI field for referencing the text of the level.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    TextMeshProUGUI levelText;

    /**
    * A private Button field for referencing the Level button.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    Button levelButton;

    /**
    * A private Button field for referencing the Next button of the levels.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    Button nextLevelButton;

    /**
    * A private Button field for referencing the Previous button of the levels.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    Button previousLevelButton;

    /**
    * A private GameObject field for referencing the settings panel.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    GameObject settingsPanel;

    /**
    * A private Image field for referencing the image of the board thumbnail.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField] 
    Image boardThumbnailImage = null;

    /**
    * A private List of Buttons to reference all the buttons in the UI.
    */
    List<Button> allButtons = new List<Button>();

    /**
     * A private Chapter field to set the current chapter.
     */
    Chapter currentChapter = Chapter.Chapter1;

    /**
     * A private int field to set the current level number.
     */
    public int currentLevel = 1;

    /**
     * A private List of integers field to get a set of current levels for the chapter.
     */
    List<int> currentChapterLevels = new List<int>();

    /**
     * A private enum for chapters.
     */
    enum Chapter { 
        Chapter1, Chapter2, Chapter3, Chapter4, Chapter5
    }

    /**
     * A private method that returns the list of level numbers for the chapter.
     * 
     * @return List of numbers of the levels.
     */
    List<int> getLevelsForChapter(Chapter chapter)
    {
        var levels = new List<int>();
        switch (chapter){
            case Chapter.Chapter1:
                for(int i = 1; i < 11; i++)
                {
                    levels.Add(i);
                }
                break;
            case Chapter.Chapter2:
                for (int i = 11; i < 21; i++)
                {
                    levels.Add(i);
                }
                break;
            case Chapter.Chapter3:
                for (int i = 21; i < 31; i++)
                {
                    levels.Add(i);
                }
                break;
            case Chapter.Chapter4:
                for (int i = 31; i < 41; i++)
                {
                    levels.Add(i);
                }
                break;
            case Chapter.Chapter5:
                for (int i = 41; i < 48; i++)
                {
                    levels.Add(i);
                }
                break;
            default: break;
        }
        return levels;
    }

    /**
    * A private void method called when the script instance is being loaded. It sets necessary variables.
    */
    private void Start()
    {
        // Activate the panel for adding listeners to the buttons
        settingsPanel.SetActive(true);

        updateLevelView();

        chapterText.text = "CHAPTER " + ((int)currentChapter + 1);

        if (currentChapter == Chapter.Chapter1)
        {
            previousChapterButton.interactable = false;
        }
        if (currentChapter == Chapter.Chapter5)
        {
            nextChapterButton.interactable = false;
        }

        currentChapterLevels = getLevelsForChapter(currentChapter);

        if (currentLevel == currentChapterLevels[0])
        {
            previousLevelButton.interactable = false;
        }
        if (currentLevel == currentChapterLevels[^1])
        {
            nextLevelButton.interactable = false;
        }

        // Make all buttons able to deselect after being clicked
        foreach (GameObject buttonObject in GameObject.FindGameObjectsWithTag("LevelSelectionButton"))
        {
            Button button = buttonObject.GetComponent<Button>();
            allButtons.Add(button);
            button.onClick.AddListener(() => deselectButton(button));
            button.onClick.AddListener(() => AudioManager.Instance.playSfx("LevelSelectionButton"));
        }
        foreach (GameObject buttonObject in GameObject.FindGameObjectsWithTag("Button"))
        {
            Button button = buttonObject.GetComponent<Button>();
            allButtons.Add(button);
            button.onClick.AddListener(() => deselectButton(button));
            button.onClick.AddListener(() => AudioManager.Instance.playSfx("RegularButton"));
        }

        // Deactivate the panel after all the button listeners were added
        settingsPanel.SetActive(false);
    }

    /**
     * A private void method that deselects the buttons.
     */
    void deselectButton(Button buttonPressed)
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    /**
     * A private void method called on destroying the object.
     */
    void OnDestroy()
    {
        foreach(Button button in allButtons)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    /**
     * A private void method that calls a coroutine that updates the view of the levels
     */
    void updateLevelView()
    {
        setLevelInManager();
        levelText.text = "LEVEL " + currentLevel;
        boardThumbnailImage.sprite = Resources.Load<Sprite>("Images/Board Thumbnails/thumbnail " + currentLevel);
        StartCoroutine(updateLevelViewCoroutine());
    }

    /**
     * A private coroutine that updates the view of the levels.
     */
    IEnumerator updateLevelViewCoroutine()
    {
        while (LevelManager.Instance.currentLevelStates.Count == 0)
        {
            print("Waiting for LevelManager...");
            yield return null;
        }
        if (LevelManager.Instance.currentLevelStates[(Level)(currentLevel - 1)])
        {
            changeButtonColor(levelButton, new Color(0.53f, 0.83f, 0.15f));
        }
        else
        {
            changeButtonColor(levelButton, Color.white);
        }
    }

    /**
     * A public void method that changes the color of the button.
     * 
     * @param button that will get its color changed.
     * @param color that will the button have.
     */
    public void changeButtonColor(Button button, Color color)
    {
        ColorBlock cb = button.colors;
        cb.normalColor = color;
        float highlight = 0.3f;
        float pressed = 0.4f;
        if(color.r + color.b + color.g < 0.25)
        {
            highlight *= -0.5f;
            pressed *= -1;
        }
        cb.highlightedColor = new Color(color.r - highlight, color.g - highlight, color.b - highlight);
        cb.selectedColor = cb.highlightedColor;
        cb.pressedColor = new Color(color.r - pressed, color.g - pressed, color.b - pressed);
        cb.disabledColor = new Color(cb.pressedColor.r, cb.pressedColor.g, cb.pressedColor.b, 0.5f);

        button.colors = cb;
    }

    /**
     * A public void method that sets the next chapter.
     */
    public void nextChapter()
    {
        currentChapter++;
        updateAll();
    }

    /**
     * A public void method that sets the previous chapter.
     */
    public void previousChapter()
    {
        currentChapter--;
        updateAll();
    }

    /**
     * A private void method that calls all update methods.
     */
    public void updateAll()
    {
        chapterText.text = "CHAPTER " + ((int)currentChapter + 1);

        updateChapterNavigationButtons();
        currentChapterLevels = getLevelsForChapter(currentChapter);
        currentLevel = currentChapterLevels[0];

        updateLevelNavigationButtons();
        updateLevelView();
    }

    /**
     * A public void method that sets the next level.
     */
    public void nextLevel()
    {
        currentLevel++;
        updateLevelView();
        updateLevelNavigationButtons();
    }

    /**
     * A public void method that sets the previous level.
     */
    public void previousLevel()
    {
        currentLevel--;
        updateLevelView();
        updateLevelNavigationButtons();
    }

    /**
     * A private void method that updates the navigation buttons of the levels.
     */
    void updateLevelNavigationButtons()
    {
        if (currentLevel == currentChapterLevels[1])
        {
            previousLevelButton.interactable = true;
        }
        if (currentLevel == currentChapterLevels[^1])
        {
            previousLevelButton.interactable = true;
            nextLevelButton.interactable = false;
        }
        if (currentLevel == currentChapterLevels[0])
        {
            previousLevelButton.interactable = false;
            nextLevelButton.interactable = true;
        }
        if (currentLevel == currentChapterLevels[^2])
        {
            nextLevelButton.interactable = true;
        }
    }

    /**
     * A private void method that updates the navigation buttons of the chapters.
     */
    void updateChapterNavigationButtons()
    {
        if (currentChapter == Chapter.Chapter2)
        {
            previousChapterButton.interactable = true;
        }
        if (currentChapter == Chapter.Chapter5)
        {
            nextChapterButton.interactable = false;
        }
        if (currentChapter == Chapter.Chapter1)
        {
            previousChapterButton.interactable = false;
        }
        if (currentChapter == Chapter.Chapter4)
        {
            nextChapterButton.interactable = true;
        }
    }

    /**
     * A private void method that sets the current level in the Game Manager
     */
    void setLevelInManager()
    {
        GameManager.Instance.currentLevel = (Level)currentLevel - 1;
    }

    /**
     * A public void method that plays the game music once the level was selected.
     */
    public void playGameMusic()
    {
        AudioManager.Instance.playGameMusic();
    }
}
