using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
     * A private enum of dialog persons.
     */
public enum Person
{
    You,
    Cynem,
    Narrator,
    Empty
}

/**
 * A class storing all information about the contents of the plot and handling the plot display.
 */
public class Plot : MonoBehaviour
{
    
    /**
     * A private string defining the color of the text of the main character.
     */
    string you_color = "<color=#9299EA>";

    /**
     * A private string defining the color of the text of the second character.
     */
    string cyn_color = "<color=#AECD68>";

    string enDialogPath = "Assets/Resources/lang-en/PlotDialogues/";
    string plDialogPath = "Assets/Resources/lang-pl/PlotDialogues/";

    /**
    * A private TextMeshProUGUI field for referencing the plot text on the screen.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    TextMeshProUGUI content;

    /**
    * A private TextMeshProUGUI field for referencing the header of the plot.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    TextMeshProUGUI header;

    /**
     * A private bool field for defining whether the next plot content should be displayed.
     */
    bool doShowNext = false;

    /**
    * A private void method called when the script instance is being loaded. It sets necessary variables.
    */
    private void Start()
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Button");
        foreach (GameObject buttonObject in buttons)
        {
            Button button = buttonObject.GetComponent<Button>();
            button.onClick.AddListener(() => AudioManager.Instance.playSfx("RegularButton"));
        }

        var plot = readPlotFile();

        if (plot.Count > 0)
        {
            StartCoroutine(setNextContent(plot));
        }
        else
        {
            closePlot();
        }
    }

    /**
     * A private void method called every frame.
     */
    void Update()
    {
        if (Input.anyKeyDown)
        {
             doShowNext = true;
        }
    }

    /**
     * A public void method for closing the plot scene
     */
    public void closePlot()
    {
        GameManager.Instance.LoadLoadingScreen();
    }

    /**
     * A public void method for showing the next plot content.
     */
    public void showNextContent()
    {
        doShowNext = true;
    }

    /**
     * A coroutine responsible for the display of the plot contents.
     */
    IEnumerator setNextContent(List<(Person person, string content)> plot)
    {
        foreach ((Person person, string content) tuple in plot)
        {
            //print(tuple.content);
            content.text = tuple.content;
            if(tuple.person == Person.You)
            {
                content.text = you_color + content.text;
                header.text = $"{you_color}You";
            }
            if (tuple.person == Person.Cynem)
            {
                content.text = cyn_color + content.text;
                header.text = $"{cyn_color}Cynem";
            }
            if (tuple.person == Person.Narrator || tuple.person == Person.Empty)
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


    List<(Person person, string content)> readPlotFile()
    {
        List<(Person person, string content)> plot = new List<(Person person, string content)>();
        string line;
        string path;
        if(GameManager.Instance.currentLanguage == Language.Polski)
        {
            path = plDialogPath;
        }
        else
        {
            path = enDialogPath;
        }
        StreamReader sr = new StreamReader(path + "Dialogue_Level_" + ((int)GameManager.Instance.currentLevel + 1) + ".txt");
        line = sr.ReadLine();

        string content = "";
        Person person = Person.Empty;

        while (line != null)
        {
            print(line);

            // Ignore empty lines
            if (line.Length != 0)
            {
                if (line.First() == '#')
                {
                    // If it is not the first line of the file
                    if (content.Length != 0)
                    {
                        plot.Add((person, content));
                        content = "";
                    }
                    person = getPersonByString(line.Remove(0, 1));
                }
                else
                {
                    content += line;
                    formatContent(ref content);
                }
            }

            //Read the next line
            line = sr.ReadLine();
        }
        plot.Add((person, content));
        sr.Close();
        return plot;
    }

    Person getPersonByString(string str)
    {
        print("Converting " + str + " to ");
        switch(str){
            case "Narrator":
                return Person.Narrator;
            case "You":
                return Person.You;
            case "Cynem":
                return Person.Cynem;
            case "Empty":
                return Person.Empty;
            default:
                return Person.Empty;
        }
    }

    void formatContent(ref string content)
    {
        string yellow = "<b><color=#BF9453>";
        string yellow_end = "</b></color>";

        content = content.Replace("{yellow}", yellow);
        content = content.Replace("{yellow_end}", yellow_end);
    }


    /**
     * A private List storing the plot contents for the levels.
     */
    List<(Person, string)> getPlotContent()
    {
        string yellow = "<b><color=#BF9453>";
        string yellow_end = "</b></color>";
        switch (GameManager.Instance.currentLevel)
        {
            case Level.Level1:
                return new List<(Person, string)>() {

                    (Person.Narrator, $"A catastrophe happened:\nthe city’s supply underground system of {yellow}thought forms{yellow_end}\n- magical marbles powering " +
                    "the town and providing essential energy for protection -\nhas gone down."),

                    (Person.Narrator, "There is only one still alive engineer in the city\nwho specializes in these systems.\n" +
                    "Unfortunately, he is not there at the moment of this disaster."),

                    (Person.Narrator, $"He is your professor at the town's academy,\nand you are {yellow}the only one{yellow_end} who he entrusted with access\n" +
                    $"to the maps and instructions for this system.\n\n" +
                    "You decide to take the matters into your own hands\n and head to the underground system yourself."),
                };

            case Level.Level4:
                return new List<(Person, string)>() { 

                (Person.Empty, $"<color=#AECD68>Hey, may I accompany you?"),

                (Person.You, "GAH! Cynem! You scared me! How did you find me down here?"),

                (Person.Cynem, "After all the sirens went on, I kind of figured it must have been the system…"),

                (Person.Cynem, "The council doesn’t even want to disclose the situation to the public,\nI guess to avoid the panic...\n" +
                "They probably want to reach the old man first."),

                (Person.Cynem, "Is he out of town?\n" +
                "I thought I would encounter him right here and, you know,\ncheck on the situation and stuff…"),

                (Person.You, "Yeah, the professor's gone for a couple of weeks since the end of the finals."),

                (Person.Cynem, "Huh..."),

                (Person.You, "Now, can I go back to the repairs?\n" +
                "Someone needs to get the system back up on its feet."),

                (Person.Cynem, "Sure sure… But how do you know what to do exactly?\nDid he leave you some notes in case of such emergencies?"),

                (Person.You, "Yeah, something like that…"),

                (Person.Cynem, "Well, can I help?"),

                (Person.You, "Sure, see the map?\n\nYou can go down these stairs to the level 5 and turn right,\n" +
                $"there should be the next system board."),

                (Person.You, "See if anything is broken, it should work like it says right here."),

                (Person.You, "I'll work on this board in the meantime."),

                (Person.Cynem, "Got it!"),

                };
            case Level.Level5:
                return new List<(Person, string)>() {

                (Person.You, "…where’s Cynem? I told him to go to level 5,\nbut both the system board parts and Cynem are missing here."),

                (Person.You, "Well, gotta get to it myself I guess.")

                };
            case Level.Level8:
                return new List<(Person, string)>() {

                (Person.You, "Cynem! Finally found you! I thought you would help me do level 4?"),

                (Person.Cynem, "Well, wasn't it fixed? I swear I fixed a board."),

                (Person.You, "And then went down to the level 8?"),

                (Person.Cynem, "We are at 8 now? Yeah, kind of got lost..."),

                (Person.Cynem, "But look, this one has a flippy thing!"),

                (Person.You, "Oh, that's a switch! Professor showed me these before, they are quite interesting actually."),

                };
            case Level.Level9:
                return new List<(Person, string)>() {

                (Person.Cynem, "It can turn on and off, huh... It is interesting, as you said."),

                (Person.You, "Right? But that is not all!\n\nI mean, it is when you have just one switch alone,\nbut when you combine it with other elements,\n" +
                "it can even save numbers and make equations!"),

                (Person.You, "And it does it all without magic, just plain simple flipping back and forth!"),

                (Person.Cynem, "This thing? You must be joking."),

                (Person.You, "For real! Why would I lie?"),

                (Person.Cynem, "Fine, if you say so... I will 'believe' in this completely non-magical idea of yours."),

                (Person.Narrator, "--You threw him an angry look, but gave up on--\n--further convincing and went downstairs to the next board.--\n" +
                "--Cynem followed.--"),

                };
            case Level.Level13:
                return new List<(Person, string)>() {

                (Person.Cynem, "You look kind of mad."),

                (Person.You, "Me? Why?"),

                (Person.Cynem, "You have not spoken to me since\nwe talked about those non-magical Bits of yours."),

                (Person.You, "Well, because... I'm frustrated you don't want to believe me."),

                (Person.Cynem, "It just seems too unrealistic to me."),

                (Person.You, "Would you believe if you saw it by yourself?"),

                (Person.Cynem, "Maybe... But it would have to be with my own very eyes.\nNo theory, just actual Bits flipping and" +
                "\ncounting right in front of me."),

                (Person.You, "Okay, wait, I think it's somewhere...\nOh, here! Look at the maps!"),

                (Person.You, "Do you see the setups after level 20? I believe that is it!"),

                (Person.You, "You can take these instructions\nand go down these corridors to check it out by yourself."),

                (Person.Cynem, "Are you sure?"),

                (Person.You, "Of course! I will fix the ones on the way there,\nand you can go check if I was lying."),

                (Person.Cynem, "Alright, see you there then."),

                };
            case Level.Level19:
                return new List<(Person, string)>() {

                (Person.You, "I wonder how Cynem is doing..."),

                (Person.You, "I'm almost at the levels I sent him to."),

                (Person.You, "What face he must have made when he realized\nhow awesome these switches are? Hahah"),

                };
            case Level.Level21:
                return new List<(Person, string)>() {

                (Person.Narrator, "--You walk down the corridor--\n--From far away, you see Cynem hunched over the papers--\n" +
                "--underneath the board. He seems to be very occupied by something.--"),

                (Person.You, "Cynem! Hey! So how was it?"),

                (Person.Cynem, "Ah, hey."),

                (Person.You, "Did you see the counting?"),

                (Person.Cynem, "Counting? Aah, yeah, I did, it was pretty cool."),

                (Person.You, "And that's all? Just pretty cool?"),

                (Person.Cynem, "I mean, very cool, truly awesome!\nHow were your boards? Didn't have much to take care of?"),

                (Person.You, "What do you mean? I've fixed\nquite a couple since I last saw you."),

                (Person.Cynem, "Huh, time flies quickly I guess."),

                };
            case Level.Level23:
                return new List<(Person, string)>() {

                (Person.Narrator, "--While you are fixing the system boards, Cynem seems to--\n--stick around, but is not making any conversation.--\n" +
                "--Or rather that sticking around you specifically,--\n--he is studying boards, both the ones you fixed and the ones further down.--"),

                };
            case Level.Level24:
                return new List<(Person, string)>() {

                (Person.You, "The Bit registers are as interesting as I imaged."),

                (Person.You, "Professor called them binary counters\nif I recall correctly."),

                (Person.You, "Bi-nary, because it's a pair of two states. On and off.\n\nBi-nary like bi-cycle, two wheels."),

                (Person.Narrator, "--Cynem was far enough down the corridor--\n--to not hear this explanation.--\n\n" +
                "--So far you could just barely see him sitting on the floor,--\n--working on something.--"),

                (Person.You, "Maybe the Bits have inspired him\nto find some one big solution for this whole disaster?"),

                };
            case Level.Level33:
                return new List<(Person, string)>() {

                (Person.Cynem, "Hey"),

                (Person.You, "Oh hi, haven't spoken to you in a while.\nWhat have you been working on so hard?"),

                (Person.Cynem, "Ah, just, you know...\ntrying to help you solve these."),

                (Person.You, "And how did it go?\nDid you come up with something?"),

                (Person.Cynem, "No, not really. So, here,\nhave the rest of your papers back."),

                (Person.You, "Oh, okay. Thank you for trying though,\ndidn't think you would be this involved."),

                (Person.Cynem, "Sure."),

                };
            case Level.Level41:
                return new List<(Person, string)>() {

                (Person.You, "Hmm... The notes say to temporarily skip\nboards on level 41 and go to 47 instead..."),

                (Person.Cynem, "Really?"),

                (Person.You, "Yeah... and it says to afterwards go back\nto 42 and proceed as normal. " +
                "Kind of unusual...\nmaybe it matters for the system?"),

                (Person.Cynem, "Let's go then."),

                (Person.You, "Yeah, alright. I'm just kind of weirded out\nwhy they decided to create this system like that."),

                (Person.Cynem, "Let's not worry about it now,\nthere must have been a cause."),

                };
            case Level.Level42:
                return new List<(Person, string)>() {

                (Person.You, "And now back the stairs to level 42..."),

                (Person.You, "Oh my legs will hurt so much\nafter all that walking..."),

                (Person.You, "Not to even mention going back up all the way. Oh man."),

                };
            case Level.Level44:
                return new List<(Person, string)>() {

                (Person.You, "Did you hear that rumbling?"),

                (Person.Cynem, "Rumbling? No, maybe you're hungry?"),

                (Person.You, "Very funny... Although I am a little bit hungry,\nbut also quite concerned."),

                };
            case Level.Level46:
                return new List<(Person, string)>() {

                (Person.You, "Okay, now you can't tell me you don't hear it.\nThe whole underground is shaking."),

                (Person.Cynem, "Maybe you're right..."),

                (Person.Narrator, "--You both hear the rumbling getting closer--\n\n--Suddenly, the top of the board you just fixed " +
                "explodes with--\n--a multitude of blue and white marbles. Cynem looks at you with fear in his eyes.--"),

                (Person.Cynem, "That is the system overload?"),

                (Person.You, "What overload?"),

                (Person.Cynem, "The system... there were too many marbles delivered\nto system's vulnerable modules."),

                (Person.You, "How do you know that?"),

                (Person.Cynem, "Uh..."),

                (Person.Cynem, "..."),

                (Person.You, "Okay, that's not important right now.\nDo you know how to fix it?"),

                (Person.Cynem, "A hard reset I think. But I do not know\nwhere we could possibly do that from."),

                (Person.You, "There is not much left to check, I believe\nwe went through all the boards and there was\nnothing like that."),

                (Person.You, "Let's check the last one left."),

                };
            case Level.Level47:
                if (!GameManager.Instance.finishedGame)
                {
                    return new List<(Person, string)>() {

                    (Person.You, "Why did it not stop?"),

                    (Person.Narrator, "--Now the rumbling is joined with multiple hissings inside the walls.--"),

                    (Person.You, "It should have worked!\nThe system sounds like it's going to explode any minute!"),

                    (Person.Narrator, "--Cynem hides his head in shaky hands--"),

                    (Person.Cynem, "I'm sorry... I'm so sorry... I-I didn't mean to..."),

                    (Person.You, "What is it?"),

                    (Person.Cynem, "I messed with the notes."),

                    (Person.Cynem, "They told me to."),

                    (Person.Cynem, "I didn't know it would... Oh God..."),

                    (Person.You, "Calm down, there MUST be\nsomething we can do."),

                    (Person.Cynem, "Its all over..."),

                    (Person.You, "God, THINK! We don't have time!"),

                    (Person.Narrator, "--Suddenly, Cynem lifts his head.--"),

                    (Person.Cynem, "...we skipped it."),

                    (Person.You, "Huh?"),

                    (Person.Cynem, "41"),

                    (Person.You, "41!"),

                    (Person.You, "WE SKIPPED ONE LEVEL!"),

                    (Person.Narrator, "--You both rush up the stairs,--\n--stumbling over the steps as the ground shakes stronger.--"),

                    };
                }
                else
                {
                    return new List<(Person, string)>() {

                    (Person.Narrator, "--The system slowly started to cool down.--\n\n" +
                    "--For a while you both stood there in silence before sliding onto the ground,--\n--exhausted from the emotions.--"),

                    (Person.Narrator, "--Cynem started explaining through tears:--\n\n" +
                    "--he told you how he accidentally got--\n--involved with this group, this mysterious group.--\n\n" +
                    "--They wanted to get the old man caught in his own contraption.--\n"),

                    (Person.Narrator, "--They didn't expect you to get there before anyone else.--\n" +
                    "--Cynem insisted they let him go in and investigate the situation.--\n" +
                    "--He didn't want you to get killed.--\n"),

                    (Person.Narrator, "--He was told to either keep the system destroyed,--\n--or they destroy you with it.--\n\n" +
                    "--So he sabotaged your work.--\n" +
                    "--He studied the notes, and slightly changed a couple instructions.--\n"),

                    (Person.Narrator, "--He wanted well, he says.--\n" +
                    "--But he doesn't ask for forgiveness.--\n\n" +
                    "--That part depends on you.--"),

                    (Person.Narrator, "--After resurfacing, you get jumped by the soldiers.--\n" +
                    "--After all, the rumbling caused quite a commotion, there's no saying--\n" +
                    "--how much damage the explosion could have caused to the town and its inhabitants.--"),

                    (Person.Narrator, "--You and Cynem get to speak to the council,--\n" +
                    "--explain everything that just occurred.--\n"),

                    (Person.Narrator, "--They hear you out and end up deciding that,--\n" +
                    "--while they admire your courage, you should still--\n" +
                    "--take responsibility for doing the repairs without any qualifications.--\n\n"+
                    "--That is why you will help in restoring the towns' parts\n--"+
                    "--that were damaged from the system failure.--"),

                    (Person.Narrator, "--A couple days go by, and before you know it--\n" +
                    "--everything is back to normal. Professor gets back immediately--\n" +
                    "--after being notified what happened and manages to fix the system enough--\n" +
                    "--for the city to have its magic supply back again.--"),

                    (Person.Narrator, "--However, the mysterious organization has not been found yet.--\n" +
                    "--So be careful,--\n" +
                    "--you never know where they could be lurking in the shadows.--\n"),

                    };
                }
            default:
                return new List<(Person, string)>() { };
        }
    }
}
