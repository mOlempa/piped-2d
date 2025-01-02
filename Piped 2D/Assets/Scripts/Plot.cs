using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Plot : MonoBehaviour
{

    [SerializeField]
    TextMeshProUGUI text;

    bool doShowNext = false;


    private void Start()
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Button");
        foreach (GameObject buttonObject in buttons)
        {
            Button button = buttonObject.GetComponent<Button>();
            button.onClick.AddListener(() => AudioManager.Instance.playSfx("RegularButton"));
        }

        if (getPlotContent().Count > 0)
        {
            StartCoroutine(setNextContent());
        }
        else
        {
            closePlot();
        }
    }

    // Detect any key pressed
    void Update()
    {
        if (Input.anyKeyDown)
        {
             doShowNext = true;
        }
    }


    public void closePlot()
    {
        GameManager.Instance.LoadLoadingScreen();
    }

    public void showNextContent()
    {
        doShowNext = true;
    }

    IEnumerator setNextContent()
    {
        foreach (string content in getPlotContent())
        {
            text.text = content;

            while (doShowNext == false)
            {
                yield return null;
            }
            doShowNext = false;
        }
        closePlot();
    }


    List<string> getPlotContent()
    {
        switch (GameManager.Instance.currentLevel)
        {
            case Level.Level1:
                return new List<string>() { "Plot text 1", "More text" };
            case Level.Level2:
                return new List<string>() { "Plot text 2" };
            default:
                return new List<string>() { };
        }
    }
}
