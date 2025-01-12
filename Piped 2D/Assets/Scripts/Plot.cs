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

                (Dialog.Cynem, "After all the sirens went on, I kind of figured it must have been the system…"),

                (Dialog.Cynem, "The council doesn’t even want to disclose the situation to the public,\nI guess to avoid the panic...\n" +
                "They probably want to reach the old man first."),

                (Dialog.Cynem, "Is he out of town?\n" +
                "I thought I would encounter him right here and, you know,\ncheck on the situation and stuff…"),

                (Dialog.You, "Yeah, the professor's gone for a couple of weeks since the end of the finals."),

                (Dialog.Cynem, "Huh..."),

                (Dialog.You, "Now, can I go back to the repairs?\n" +
                "Someone needs to get the system back up on its feet."),

                (Dialog.Cynem, "Sure sure… But how do you know what to do exactly?\nDid he leave you some notes in case of such emergencies?"),

                (Dialog.You, "Yeah, something like that…"),

                (Dialog.Cynem, "Well, can I help?"),

                (Dialog.You, "Sure, see the map?\n\nYou can go down these stairs to the level 5 and turn right,\n" +
                $"there should be the next system board."),

                (Dialog.You, "See if anything is broken, it should work like it says right here."),

                (Dialog.You, "I'll work on this board in the meantime."),

                (Dialog.Cynem, "Got it!"),

                };
            case Level.Level5:
                return new List<(Dialog, string)>() {

                (Dialog.You, "…where’s Cynem? I told him to go to level 5,\nbut both the system board parts and Cynem are missing here."),

                (Dialog.You, "Well, gotta get to it myself I guess.")

                };
            case Level.Level8:
                return new List<(Dialog, string)>() {

                (Dialog.You, "Cynem! Finally found you! I thought you would help me do level 4?"),

                (Dialog.Cynem, "Well, wasn't it fixed? I swear I fixed a board."),

                (Dialog.You, "And then went down to the level 8?"),

                (Dialog.Cynem, "We are at 8 now? Yeah, kind of got lost..."),

                (Dialog.Cynem, "But look, this one has a flippy thing!"),

                (Dialog.You, "Oh, that's a switch! Professor showed me these before, they are quite interesting actually."),

                };
            case Level.Level9:
                return new List<(Dialog, string)>() {

                (Dialog.Cynem, "It can turn on and off, huh... It is interesting, as you said."),

                (Dialog.You, "Right? But that is not all!\n\nI mean, it is when you have just one switch alone,\nbut when you combine it with other elements,\n" +
                "it can even save numbers and make equations!"),

                (Dialog.You, "And it does it all without magic, just plain simple flipping back and forth!"),

                (Dialog.Cynem, "This thing? You must be joking."),

                (Dialog.You, "For real! Why would I lie?"),

                (Dialog.Cynem, "Fine, if you say so... I will 'believe' in this completely non-magical idea of yours."),

                (Dialog.Narrator, "--You threw him an angry look, but gave up on--\n--further convincing and went downstairs to the next board.--\n" +
                "--Cynem followed.--"),

                };
            case Level.Level13:
                return new List<(Dialog, string)>() {

                (Dialog.Cynem, "You look kind of mad."),

                (Dialog.You, "Me? Why?"),

                (Dialog.Cynem, "You have not spoken to me since\nwe talked about those non-magical Bits of yours."),

                (Dialog.You, "Well, because... I'm frustrated you don't want to believe me."),

                (Dialog.Cynem, "It just seems too unrealistic to me."),

                (Dialog.You, "Would you believe if you saw it by yourself?"),

                (Dialog.Cynem, "Maybe... But it would have to be with my own very eyes.\nNo theory, just actual Bits flipping and" +
                "\ncounting right in front of me."),

                (Dialog.You, "Okay, wait, I think it's somewhere...\nOh, here! Look at the maps!"),

                (Dialog.You, "Do you see the setups after level 20? I believe that is it!"),

                (Dialog.You, "You can take these instructions\nand go down these corridors to check it out by yourself."),

                (Dialog.Cynem, "Are you sure?"),

                (Dialog.You, "Of course! I will fix the ones on the way there,\nand you can go check if I was lying."),

                (Dialog.Cynem, "Alright, see you there then."),

                };
            case Level.Level19:
                return new List<(Dialog, string)>() {

                (Dialog.You, "I wonder how Cynem is doing..."),

                (Dialog.You, "I'm almost at the levels I sent him to."),

                (Dialog.You, "What face he must have made when he realized\nhow awesome these switches are? Hahah"),

                };
            case Level.Level21:
                return new List<(Dialog, string)>() {

                (Dialog.Narrator, "--You walk down the corridor--\n--From far away, you see Cynem hunched over the papers--\n" +
                "--underneath the board. He seems to be very occupied by something.--"),

                (Dialog.You, "Cynem! Hey! So how was it?"),

                (Dialog.Cynem, "Ah, hey."),

                (Dialog.You, "Did you see the counting?"),

                (Dialog.Cynem, "Counting? Aah, yeah, I did, it was pretty cool."),

                (Dialog.You, "And that's all? Just pretty cool?"),

                (Dialog.Cynem, "I mean, very cool, truly awesome!\nHow were your boards? Didn't have much to take care of?"),

                (Dialog.You, "What do you mean? I've fixed\nquite a couple since I last saw you."),

                (Dialog.Cynem, "Huh, time flies quickly I guess."),

                };
            case Level.Level23:
                return new List<(Dialog, string)>() {

                (Dialog.Narrator, "--While you are fixing the system boards, Cynem seems to--\n--stick around, but is not making any conversation.--\n" +
                "--Or rather that sticking around you specifically,--\n--he is studying boards, both the ones you fixed and the ones further down.--"),

                };
            case Level.Level24:
                return new List<(Dialog, string)>() {

                (Dialog.You, "The Bit registers are as interesting as I imaged."),

                (Dialog.You, "Professor called them binary counters\nif I recall correctly."),

                (Dialog.You, "Bi-nary, because it's a pair of two states. On and off.\n\nBi-nary like bi-cycle, two wheels."),

                (Dialog.Narrator, "--Cynem was far enough down the corridor--\n--to not hear this explanation.--\n\n" +
                "--So far you could just barely see him sitting on the floor,--\n--working on something.--"),

                (Dialog.You, "Maybe the Bits have inspired him\nto find some one big solution for this whole disaster?"),

                };
            case Level.Level33:
                return new List<(Dialog, string)>() {

                (Dialog.Cynem, "Hey"),

                (Dialog.You, "Oh hi, haven't spoken to you in a while.\nWhat have you been working on so hard?"),

                (Dialog.Cynem, "Ah, just, you know...\ntrying to help you solve these."),

                (Dialog.You, "And how did it go?\nDid you come up with something?"),

                (Dialog.Cynem, "No, not really. So, here,\nhave the rest of your papers back."),

                (Dialog.You, "Oh, okay. Thank you for trying though,\ndidn't think you would be this involved."),

                (Dialog.Cynem, "Sure."),

                };
            case Level.Level41:
                return new List<(Dialog, string)>() {

                (Dialog.You, "Hmm... The notes say to temporarily skip\nboards on level 41 and go to 47 instead..."),

                (Dialog.Cynem, "Really?"),

                (Dialog.You, "Yeah... and it says to afterwards go back\nto 42 and proceed as normal. " +
                "Kind of unusual...\nmaybe it matters for the system?"),

                (Dialog.Cynem, "Let's go then."),

                (Dialog.You, "Yeah, alright. I'm just kind of weirded out\nwhy they decided to create this system like that."),

                (Dialog.Cynem, "Let's not worry about it now,\nthere must have been a cause."),

                };
            case Level.Level42:
                return new List<(Dialog, string)>() {

                (Dialog.You, "And now back the stairs to level 42..."),

                (Dialog.You, "Oh my legs will hurt so much\nafter all that walking..."),

                (Dialog.You, "Not to even mention going back up all the way. Oh man."),

                };
            case Level.Level44:
                return new List<(Dialog, string)>() {

                (Dialog.You, "Did you hear that rumbling?"),

                (Dialog.Cynem, "Rumbling? No, maybe you're hungry?"),

                (Dialog.You, "Very funny... Although I am a little bit hungry,\nbut also quite concerned."),

                };
            case Level.Level46:
                return new List<(Dialog, string)>() {

                (Dialog.You, "Okay, now you can't tell me you don't hear it.\nThe whole underground is shaking."),

                (Dialog.Cynem, "Maybe you're right..."),

                (Dialog.Narrator, "--You both hear the rumbling getting closer--\n\n--Suddenly, the top of the board you just fixed " +
                "explodes with--\n--a multitude of blue and white marbles. Cynem looks at you with fear in his eyes.--"),

                (Dialog.Cynem, "That is the system overload?"),

                (Dialog.You, "What overload?"),

                (Dialog.Cynem, "The system... there were too many marbles delivered\nto system's vulnerable modules."),

                (Dialog.You, "How do you know that?"),

                (Dialog.Cynem, "Uh..."),

                (Dialog.Cynem, "..."),

                (Dialog.You, "Okay, that's not important right now.\nDo you know how to fix it?"),

                (Dialog.Cynem, "A hard reset I think. But I do not know\nwhere we could possibly do that from."),

                (Dialog.You, "There is not much left to check, I believe\nwe went through all the boards and there was\nnothing like that."),

                (Dialog.You, "Let's check the last one left."),

                };
            case Level.Level47:
                if (!GameManager.Instance.finishedGame)
                {
                    return new List<(Dialog, string)>() {

                    (Dialog.You, "Why did it not stop?"),

                    (Dialog.Narrator, "--Now the rumbling is joined with multiple hissings inside the walls.--"),

                    (Dialog.You, "It should have worked!\nThe system sounds like it's going to explode any minute!"),

                    (Dialog.Narrator, "--Cynem hides his head in shaky hands--"),

                    (Dialog.Cynem, "I'm sorry... I'm so sorry... I-I didn't mean to..."),

                    (Dialog.You, "What is it?"),

                    (Dialog.Cynem, "I messed with the notes."),

                    (Dialog.Cynem, "They told me to."),

                    (Dialog.Cynem, "I didn't know it would... Oh God..."),

                    (Dialog.You, "Calm down, there MUST be\nsomething we can do."),

                    (Dialog.Cynem, "Its all over..."),

                    (Dialog.You, "God, THINK! We don't have time!"),

                    (Dialog.Narrator, "--Suddenly, Cynem lifts his head.--"),

                    (Dialog.Cynem, "...we skipped it."),

                    (Dialog.You, "Huh?"),

                    (Dialog.Cynem, "41"),

                    (Dialog.You, "41!"),

                    (Dialog.You, "WE SKIPPED ONE LEVEL!"),

                    (Dialog.Narrator, "--You both rush up the stairs,--\n--stumbling over the steps as the ground shakes stronger.--"),

                    };
                }
                else
                {
                    return new List<(Dialog, string)>() {

                    (Dialog.Narrator, "--The system slowly started to cool down.--\n\n" +
                    "--For a while you both stood there in silence before sliding onto the ground,--\n--exhausted from the emotions.--"),

                    (Dialog.Narrator, "--Cynem started explaining through tears:--\n\n" +
                    "--he told you how he accidentally got--\n--involved with this group, this mysterious group.--\n\n" +
                    "--They wanted to get the old man caught in his own contraption.--\n"),

                    (Dialog.Narrator, "--They didn't expect you to get there before anyone else.--\n" +
                    "--Cynem insisted they let him go in and investigate the situation.--\n" +
                    "--He didn't want you to get killed.--\n"),

                    (Dialog.Narrator, "--He was told to either keep the system destroyed,--\n--or they destroy you with it.--\n\n" +
                    "--So he sabotaged your work.--\n" +
                    "--He studied the notes, and slightly changed a couple instructions.--\n"),

                    (Dialog.Narrator, "--He wanted well, he says.--\n" +
                    "--But he doesn't ask for forgiveness.--\n\n" +
                    "--That part depends on you.--"),

                    (Dialog.Narrator, "--After resurfacing, you get jumped by the soldiers.--\n" +
                    "--After all, the rumbling caused quite a commotion, there's no saying--\n" +
                    "--how much damage the explosion could have caused to the town and its inhabitants.--"),

                    (Dialog.Narrator, "--You and Cynem get to speak to the council,--\n" +
                    "--explain everything that just occurred.--\n"),

                    (Dialog.Narrator, "--They hear you out and end up deciding that,--\n" +
                    "--while they admire your courage, you should still--\n" +
                    "--take responsibility for doing the repairs without any qualifications.--\n\n"+
                    "--That is why you will help in restoring the towns' parts\n--"+
                    "--that were damaged from the system failure.--"),

                    (Dialog.Narrator, "--A couple days go by, and before you know it--\n" +
                    "--everything is back to normal. Professor gets back immediately--\n" +
                    "--after being notified what happened and manages to fix the system enough--\n" +
                    "--for the city to have its magic supply back again.--"),

                    (Dialog.Narrator, "--However, the mysterious organization has not been found yet.--\n" +
                    "--So be careful,--\n" +
                    "--you never know where they could be lurking in the shadows.--\n"),

                    };
                }
            default:
                return new List<(Dialog, string)>() { };
        }
    }
}
