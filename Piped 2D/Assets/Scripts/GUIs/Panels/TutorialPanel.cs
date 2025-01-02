using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField]
    Image spotlightOverlay;

    [SerializeField]
    GameObject card;

    [SerializeField]
    Button nextCardButton;

    [SerializeField]
    Button previousCardButton;

    [SerializeField]
    TextMeshProUGUI cardContentText;

    [SerializeField]
    GameUI gameUIScript;

    bool isNextCard = false;
    bool isPreviousCard = false;
    bool isClosed = false;

    public bool? thisLevelHasTutorial;

    RectTransform cardTransform;

    (Vector2 center, Vector2 topLeft, Vector2 topRight, 
        Vector2 middleLeft, Vector2 middleRight, 
        Vector2 bottomLeft, Vector2 bottomRight) positions =
        (new Vector2(0, 0), new Vector2(-250, 85), new Vector2(250, 85),
        new Vector2(-250, 0), new Vector2(250, 0),
        new Vector2(-250, -85), new Vector2(250, -85));


    private void Start()
    {
        //GameManager.Instance.pauseGameSpeed();
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Button");
        foreach (GameObject buttonObject in buttons)
        {
            Button button = buttonObject.GetComponent<Button>();
            button.onClick.AddListener(() => AudioManager.Instance.playSfx("RegularButton"));
        }

        cardTransform = card.GetComponent<RectTransform>();
        if(getTutorialContent().Count > 0 )
        {
            thisLevelHasTutorial = true;
            StartCoroutine(setNextContent());
        }
        else
        {
            thisLevelHasTutorial = false;
            closeTutorial();
        }
    }

    public void openTutorial()
    {
        isClosed = false;
        gameObject.SetActive(true);
        StartCoroutine(setNextContent());
    }

    public void closeTutorial()
    {
        gameObject.SetActive(false);
        isClosed = true;
        gameUIScript.toggleInstruction();
        //GameManager.Instance.resetGameSpeed();
    }

    public void showNextCard()
    {
        isNextCard = true;
    }

    public void showPreviousCard()
    {
        isPreviousCard = true;
    }

    IEnumerator setNextContent()
    {
        int counter = 0;
        disablePreviousButton();

        while (true)
        {
            (string overlayName, string content, Vector2 position) tuple = getTutorialContent()[counter];

            cardContentText.text = tuple.content;
            spotlightOverlay.sprite = Resources.Load<Sprite>("Images/Tutorial/tutorial_" + tuple.overlayName);
            cardTransform.localPosition = tuple.position;

            while (isNextCard == false && isPreviousCard == false)
            {
                if (isClosed)
                {
                    isClosed = false;
                    yield break;
                }
                yield return null;
            }
            if (isNextCard)
            {
                isNextCard = false;
                counter++;
                if (counter == 1)
                {
                    enablePreviousButton();
                }
            }
            else { 
                isPreviousCard = false;
                counter--;
                if(counter == 0)
                {
                    disablePreviousButton();
                }
            }

            if(counter == getTutorialContent().Count)
            {
                break;
            }

        }
        closeTutorial();

    }

    void disablePreviousButton()
    {
        previousCardButton.interactable = false;
    }
    void enablePreviousButton()
    {
        previousCardButton.interactable = true;
    }

    public List<(string, string, Vector2)> getTutorialContent()
    {
        string blue = "<color=#4E8FF6><b>blue</b></color>";
        string white = "<color=#F8EBF6><b>white</b></color>";

        switch (GameManager.Instance.currentLevel)
        {
            case Level.Level1:
                return new List<(string, string, Vector2)>() { 

                    ("empty", "Hi! This is a <b>very important</b> tutorial explaining how to navigate this game, but if you aren't new to " +
                    "its mechanics, feel free to skip it by clicking the X button in the top right corner of this small window.", positions.center),

                    ("empty", "Alright, let's get started!", positions.center),

                    ("dispenser","This is a dispenser, it makes the marbles roll one by one.", positions.middleRight),

                    ("both_dispensers", $"There are two dispensers, one with {blue} marbles and one with {white} marbles.", positions.bottomRight),

                    ("dispenser_lamps", "Each has a small lamp next to it. Lamp switched on means this dispenser will start first. <b>Pay " +
                    "attention to them.</b>", 
                    positions.bottomRight),

                    ("generated_board", "The marbles will roll through a path you create. A part of it is already prepared " +
                    "- work with it by adding your own elements.", positions.bottomRight),

                    ("generated_board", "You need to create a full path <b>all the way</b> to where the marbles need to get to!\n\n" +
                    "If you let them fall from too high, they will break and disappear.", positions.bottomRight),

                    ("block_button", "Here you choose what type of element you want to use. \n\nFor starters we will use only " +
                    "one type: the <b>Pass</b>. It simply passes the marbles further.", positions.middleLeft),

                    ("block_button", "To place it, click this Pass button and left-click somewhere on the system board - it will latch into place." +
                    "\n\nTo delete it from the board, right-click with your mouse on the Pass element.", positions.middleLeft),

                    ("block_button", "You can also switch the side it is turned towards - simply left-click with your mouse " +
                    "on it once it is placed.", positions.middleLeft),

                    ("output", "This is were the marbles need to get right now.", positions.topRight),

                    ("output_ramp", "They will create a pattern in this little nook. We call it the output of the system.", positions.topRight),

                    ("left_wheels", "When a marble rolls into one of the output wheels, it makes the wheel at " +
                    "the dispenser turn as well.", positions.middleRight),

                    ("generated_board", $"You can also place {blue} test marbles on the system board almost wherever you want! Just point to a place " +
                    "with your mouse cursor and hit Q on you keyboard.", positions.bottomRight),

                    ("trashcan_button", "If you want to get rid of your test marbles, hit this trashcan button" +
                    " - it will delete them.\n\n", positions.middleLeft),

                    ("upper_buttons", "At the top here you have a couple of buttons, their functions are pretty simple.",  positions.bottomRight),

                    ("pause_button", "A <b>Pause Button</b> - makes the time pause, so all the marbles will freeze.\n\n" +
                    "To unpause the time, hit this button again.", positions.bottomRight),

                    ("play_button", "Next, a <b>Start Button</b> - click it when you are done setting up the path, it will roll the first marble from " +
                    "the dispenser and make the system check your solution.", positions.bottomRight),

                    ("play_button", "You <b>are unable to</b> make any changes to the path during that time, " +
                    "but you can stop and reset the system checking by clicking this button again. ", positions.bottomRight),

                    ("play_button", "This button also deletes your test marbles, similarly to the trashcan.", positions.bottomRight),

                    ("speed_button", "And lastly, a <b>Speed Button</b> - by clicking on it, you can make the marbles roll faster or slower. " +
                    "\n\nThe speed will change with each click and the button will change an icon to show how fast the system " +
                    "currently runs.",  positions.bottomRight),

                    ("side_menu", "Other than these buttons, you also have a side menu.", positions.middleLeft),

                    ("instruction_button", "First here is an <b>Instruction button</b>. " +
                    "Click it to view your task, it will tell you what your current goal is.", positions.middleLeft),

                    ("settings_button", "Next is a <b>Settings button</b>. You can open it to change your volume settings " +
                    "anytime.", positions.middleLeft),

                    ("level_selection_button", "Under that there is a <b>Level Selection button</b>. This is for when you want to go back to " +
                    "the menu with selection of the levels.", positions.middleLeft),

                    ("main_menu_button", "There is also a Main Menu button. Click it if you want to leave this level and go to " +
                    "the Main Menu.", positions.middleLeft),

                    ("empty", "Now the time has come for you to try all the buttons by yourself. When you feel ready, try to " +
                    "solve the task given in the instruction.", positions.center),

                    ("play_button", "There is no need to rush. Once you believe you solved the task, hit the Start button.", positions.center),

                    ("level_complete_text", "The checking system will run and if your solution will give a correct result, you will " +
                    "get a message right here. Good luck!",  positions.bottomRight),
                    };
            case Level.Level2:
                return new List<(string, string, Vector2)>() {

                    ("empty", "Pay attention...", positions.center),

                    ("both_dispensers", "Just because marbles are there, that does not mean they need to be dispensed.", positions.middleLeft)
                };
            case Level.Level4:
                return new List<(string, string, Vector2)>() {

                    ("empty", "Pay attention...", positions.center),

                    ("block_button", "Sometimes, there will be more parts than you need in the task.", positions.middleLeft),

                    ("dispenser_lamps", $"Also, this task starts with the {white} marble dispenser. Always pay attention to the " +
                     "<b>dispenser lamps</b>!", positions.bottomRight)
                };
            case Level.Level5:
                return new List<(string, string, Vector2)>() {

                    ("empty", "New element!", positions.center),

                    ("crossover_introduction", "A Crossover - it passes the marbles from left to right or right to left. " +
                    "Kind of like a bridge with a perpendicular road underneath.", positions.bottomRight),

                    ("crossover_introduction", "They have one small quirk though - you cannot place a test marble inside of them.", 
                    positions.bottomRight)
                };
            case Level.Level8:
                return new List<(string, string, Vector2)>() {

                    ("empty", "New element!", positions.center),

                    ("bit_introduction", "A <b>Bit</b> - it's ramp rotates when a marble runs through it (or when you click it).", 
                    positions.bottomRight),

                    ("bit_introduction", "Bit acts like a <b>light switch</b> - it can be switched <b>ON</b> or <b>OFF</b>.\n\n", positions.bottomRight),

                    ("bit_introduction", "This is a way it stores an information, like a <b>YES</b> (on) and <b>NO</b> (off).", positions.bottomRight),

                    ("empty", "Try it for yourself!", positions.center)
                };
            case Level.Level9:
                return new List<(string, string, Vector2)>() {

                    ("empty", "Pay attention...", positions.center),

                    ("dispenser", "Sometimes tasks will start a different amount of marbles.", positions.middleLeft)
                };
            case Level.Level12:
                return new List<(string, string, Vector2)>() {

                    ("empty", "New element!", positions.center),

                    ("stopper_introduction", "A Stopper - it stops the system by stopping the marble from falling further. " +
                    "No marble to trigger the output wheel means there will be no next marble releasing from the dispenser.", positions.topRight)
                };
            case Level.Level14:
                return new List<(string, string, Vector2)>() {

                    ("empty", "Pay attention...", positions.center),

                    ("empty", "You will see a new system behaviour on this level.\n\nThe task here has two situations:\nin case of that setup, " +
                    "do that, otherwise do that.", positions.center),

                    ("empty", "The system will have to check both of these situations, so it will play out two scenarios.", positions.center),

                    ("empty", "So do not worry when the marbles suddenly reset when system checks your solution! Just let it do " +
                    "its thing.", positions.center)
                };
            case Level.Level19:
                return new List<(string, string, Vector2)>() {

                    ("empty", "Pay attention...", positions.center),

                    ("empty", $"This task reads 'If ..., stop the {blue} marble. If ..., stop the {white} marble.'", positions.center),

                    ("empty", "Stopping the marbles does not mean 'do not let them fall through'. It means to put the final " +
                    "marble from your solution in a <b>Stopper</b>.\n\n" +
                    "The task would tell you directly to not let specific marbles fall through if that is the case.", positions.center)
                };
            case Level.Level21:
                return new List<(string, string, Vector2)>() {

                    ("empty", "New element!", positions.center),

                    ("register_introduction2", "A register - it is a group of Bits, which can be read as a number." +
                    "When you open the instruction, you will see numbers next to each Bit in the register.", positions.bottomRight),

                    ("register_introduction", "By adding the numbers found next to the Bits that are turned ON, you can read the " +
                    "number the whole register holds.\n\nExample:\n" +
                    "  <b>on</b> off off off                 off <b>on</b> off <b>on</b>\n" +
                    "  1    2    4    8  ==>  1         1    2    4    8  ==>  2 + 8 = 10", positions.bottomRight)
                };
            case Level.Level27:
                return new List<(string, string, Vector2)>() {

                    ("empty", "Pay attention...", positions.center),

                    ("switch_sides_button", "In some tasks you can decide the side that releases the first marble.", positions.bottomRight),

                    ("dispenser_lamps", "Do you remember these lamps? They will show you the side you have currently chosen.", positions.bottomRight),

                    ("switch_sides_button", "If you want to pick a different side than the one which is chosen at the moment, just " +
                    "hit this button anytime before starting the system.", positions.bottomRight),
                };
            case Level.Level28:
                return new List<(string, string, Vector2)>() {

                    ("empty", "New element!", positions.center),

                    ("gear_introduction", "A Gear - it will allow you to connect Bits together.\n\n" +
                    "Connected Bits will move together at the same time.", positions.middleLeft),

                    ("gear_introduction", "To create a connection, either add a Gear next to Bits you want to connect or " +
                    "add the Bits next to the Gear - the order doesn't matter.", positions.middleLeft),

                    ("gear_introduction", "The Gear element clicks on the system board in different places than the regular elements. " +
                    "This is because it needs to be right next to the Bits it will connect.\n\nThink of this like a checker board: " +
                    "regular elements go in black squares, Gears go in white ones.", positions.middleLeft),

                    ("gear_introduction", "One Gear connects at most 4 Bits, but you can add more Gears to the connection to create " +
                    "a big connection net!", positions.middleLeft),

                    ("empty", "Try creating your first Bit connection!", positions.center)
                };
            default:
                return new List<(string, string, Vector2)>() { };
        }
    }
}
