using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Plot : MonoBehaviour
{

    enum Dialog
    {
        You,
        Cynem,
        Narrator,
        Empty
    }

    string you_color = "<color=#9299EA>";
    string cyn_color = "<color=#AECD68>";

    [SerializeField]
    TextMeshProUGUI content;

    [SerializeField]
    TextMeshProUGUI header;

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
        foreach ((Dialog person, string content) tuple in getPlotContent())
        {
            content.text = tuple.content;
            if(tuple.person == Dialog.You)
            {
                content.text = you_color + content.text;
                header.text = $"{you_color}You";
            }
            if (tuple.person == Dialog.Cynem)
            {
                content.text = cyn_color + content.text;
                header.text = $"{cyn_color}Cynem";
            }
            if (tuple.person == Dialog.Narrator || tuple.person == Dialog.Empty)
            {
                header.text = "";
            }

            while (doShowNext == false)
            {
                yield return null;
            }
            doShowNext = false;
        }
        closePlot();
    }


    List<(Dialog, string)> getPlotContent()
    {
        string yellow = "<b><color=#BF9453>";
        string yellow_end = "</b></color>";
        switch (GameManager.Instance.currentLevel)
        {
            case Level.Level1:
                return new List<(Dialog, string)>() {

                    (Dialog.Narrator, $"A catastrophe happened:\nthe city’s supply underground system of {yellow}thought forms{yellow_end}\n- magical marbles powering " +
                    "the town and providing essential energy for protection -\nhas gone down."),

                    (Dialog.Narrator, "There is only one still alive engineer in the city\nwho specializes in these systems.\n" +
                    "Unfortunately, he is not there at the moment of this disaster."),

                    (Dialog.Narrator, $"He is your professor at the town's academy,\nand you are {yellow}the only one{yellow_end} who he entrusted with access\n" +
                    $"to the maps and instructions for this system.\n\n" +
                    "You decide to take the matters into your own hands\n and head to the underground system yourself."),
                };

            case Level.Level4:
                return new List<(Dialog, string)>() { 

                (Dialog.Empty, $"<color=#AECD68>Hey, may I accompany you?"),

                (Dialog.You, "GAH! Cynem! You scared me! How did you find me down here?"),

                (Dialog.Cynem, "After all the sirens went on, I kind of figured it must be the system…"),

                (Dialog.Cynem, "The council doesn’t even want to disclose the situation to the public,\nI guess to avoid the panic...\n" +
                "They probably want to reach the old man first."),

                (Dialog.Cynem, "Is he out of town?\n" +
                "I thought I would encounter him right here and, you know,\ncheck on the situation and stuff…"),

                (Dialog.You, "Yeah, the professor's gone for a couple of weeks since the end of the finals."),

                (Dialog.Cynem, "Huh..."),

                (Dialog.Cynem, "..."),

                (Dialog.You, "Now, can I go back to the repairs?\n" +
                "Someone needs to get the system back up on its feet."),

                (Dialog.Cynem, "Sure sure… But how do you know what to do exactly?\nDid he leave you some notes in case of such emergencies?"),

                (Dialog.You, "Yeah, something like that…"),

                (Dialog.Cynem, "...well, can I help?"),

                (Dialog.You, "Sure, see the map?\n\nYou can go down these stairs to the level 5 and turn right,\n" +
                $"there should be the next system board."),

                (Dialog.You, "See if anything is broken, it should work like it says right here."),

                (Dialog.You, "I'll work on this board in the meantime."),

                (Dialog.Cynem, "Got it."),

                };
            default:
                return new List<(Dialog, string)>() { };
        }
    }
}
