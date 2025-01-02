using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChapterUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI chapterText;

    [SerializeField]
    Button nextChapterButton;

    [SerializeField]
    Button previousChapterButton;

    [SerializeField]
    TextMeshProUGUI levelText;

    [SerializeField]
    Button levelButton;

    [SerializeField]
    Button nextLevelButton;

    [SerializeField]
    Button previousLevelButton;

    [SerializeField]
    GameObject settingsPanel;


    [SerializeField] 
    Image boardThumbnailImage = null;

    List<Button> allButtons = new List<Button>();

    /*private void Awake()
    {
        if (image != null)
        {
            image.sprite = Resources.Load<Sprite>("Images/test");
        }
    }*/

    Chapter currentChapter = Chapter.Chapter1;
    public int currentLevel = 1;

    List<int> currentChapterLevels = new List<int>();

    enum Chapter { 
        Chapter1, Chapter2, Chapter3, Chapter4, Chapter5
    }


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

    void deselectButton(Button buttonPressed)
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    void OnDestroy()
    {
        foreach(Button button in allButtons)
        {
            button.onClick.RemoveAllListeners();
        }
    }


    void updateLevelView()
    {
        setLevelInManager();
        levelText.text = "LEVEL " + currentLevel;
        boardThumbnailImage.sprite = Resources.Load<Sprite>("Images/Board Thumbnails/thumbnail " + currentLevel);

        /*if (LevelManager.Instance.currentLevelStates[currentLevel])
        {
            levelText.text = "Level " + (int)currentLevel;
        }*/
        StartCoroutine(updateLevelViewCoroutine());
    }

    IEnumerator updateLevelViewCoroutine()
    {
        while (LevelManager.Instance.currentLevelStates.Count == 0)
        {
            print("Waiting for LevelManager...");
            yield return null;
        }
        //print("<color=green>LevelManager done");
        if (LevelManager.Instance.currentLevelStates[(Level)(currentLevel - 1)])
        {
            //levelText.color = Color.green;
            /*ColorBlock cb = levelButton.colors;
            cb.normalColor = Color.green;
            levelButton.colors = cb;*/
            changeButtonColor(levelButton, new Color(0.53f, 0.83f, 0.15f));
        }
        else
        {
            //levelText.color = Color.white;
            /*ColorBlock cb = levelButton.colors;
            cb.normalColor = Color.white;
            levelButton.colors = cb;*/
            changeButtonColor(levelButton, Color.white);
        }
    }

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
    public void nextChapter()
    {
        currentChapter++;
        updateAll();
    }

    public void previousChapter()
    {
        currentChapter--;
        updateAll();
    }

    public void updateAll()
    {
        chapterText.text = "CHAPTER " + ((int)currentChapter + 1);

        updateChapterNavigationButtons();
        currentChapterLevels = getLevelsForChapter(currentChapter);
        currentLevel = currentChapterLevels[0];

        updateLevelNavigationButtons();
        updateLevelView();
    }

    public void nextLevel()
    {
        currentLevel++;
        updateLevelView();
        updateLevelNavigationButtons();
    }

    public void previousLevel()
    {
        currentLevel--;
        updateLevelView();
        updateLevelNavigationButtons();
    }

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

    void setLevelInManager()
    {
        GameManager.Instance.currentLevel = (Level)currentLevel - 1;
    }
}
