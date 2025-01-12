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
        string brighten = "<b><color=#BF9453>";
        string end = "</b></color>";

        switch (GameManager.Instance.currentLevel)
        {
            case Level.Level1:
                return new List<(string, string, Vector2)>() { 

                    ("empty", "Hi! This is a <b>very important</b> tutorial explaining how to navigate this game, but if you aren't new to " +
                    "its mechanics, feel free to skip it by clicking the X button in the top right corner of this small window.", positions.center),

                    ("empty", "Alright, let's get started!", positions.center),

                    ("dispenser",$"This is a {brighten}dispenser{end}.\n\nIt makes the marbles roll one by one.", positions.middleRight),

                    ("both_dispensers", $"There are two dispensers,\none with {blue} marbles and one with {white} marbles.", positions.bottomRight),

                    ("dispenser_lamps", $"Each has a small lamp next to it.\n\nLamp {brighten}switched on{end} means this dispenser will start " +
                    $"{brighten}first{end}.\n\n<b>Pay attention to them.</b>", 
                    positions.bottomRight),

                    ("generated_board", $"The marbles will roll through {brighten}a path you create{end}.\n\nA part of it is already prepared " +
                    "- work with it by adding your own elements.", positions.bottomRight),

                    ("generated_board", $"You need to create {brighten}a full path all the way{end} to where the marbles " +
                    "need to get to!", positions.bottomRight),

                    ("breaking_marble", $"If you let them fall from too high, they will {brighten}break and disappear{end}.", positions.bottomRight),

                    ("block_button", $"Here you {brighten}choose your building blocks{end}", positions.middleLeft),

                    ("block_button", $"For starters you will use only one type: {brighten}the Pass{end}.\n\n" +
                    $"It passes the marbles further, like a pipe.", positions.middleLeft),

                    ("block_place", $"To {brighten}place it{end}, click this {brighten}Pass button{end} and click somewhere on the system " +
                    $"board - it will latch into place.", positions.bottomRight),

                    ("block_mirror", $"You can also {brighten}switch the side{end} it is turned towards.\n\nSimply left-click with your mouse " +
                    "on it once it is placed.", positions.bottomRight),

                    ("block_delete", $"To {brighten}delete{end} your Pass element from the board, right-click with your mouse on it.", 
                    positions.bottomRight),

                    ("output", "And this is were the marbles need to get to right now.", positions.topRight),

                    ("output_ramp", $"They will create a pattern in this little nook. We call it the {brighten}output{end} of the system.", positions.topRight),

                    ("left_wheels", $"When a marble rolls into one of the {brighten}output wheels{end}, it makes the wheel at " +
                    $"{brighten}the dispenser turn as well{end}.", positions.middleRight),

                    ("test_marble", $"You can also place {blue} {brighten}test marbles{end} on the system board almost wherever you want!\n\n" +
                    $"Just point to a place with your mouse cursor and {brighten}hit Q{end} on you keyboard.", positions.bottomRight),

                    ("test_marble_empty", $"Try it out now!", positions.bottomRight),

                    ("trashcan_button", $"If you want to {brighten}get rid{end} of your test marbles, click this {brighten}trashcan button{end}" +
                    " - it will delete them.", positions.middleRight),

                    ("restart_button", $"You can also {brighten}reload the level{end} to restart the board completely with this circle arrow here.", 
                    positions.middleRight),

                    ("upper_buttons", "At the top here you have a couple more buttons, their functions are pretty simple:",  positions.bottomRight),

                    ("pause_button", $"A {brighten}Pause Button{end} - makes the time pause, so everything will {brighten}freeze{end}.\n\n" +
                    "To unpause the time, hit this button again.", positions.bottomRight),

                    ("play_button", $"Next, a {brighten}Start Button{end} - click it when you {brighten}are done setting up the path{end}.\n\n", 
                    positions.bottomRight),

                    ("play_button", $"It will start the dispenser and make the system {brighten}check your solution{end}.", positions.bottomRight),

                    ("play_button", $"You {brighten}are unable to make any changes{end} to the path during that time.\n\n" +
                    "If you want to make changes, click this button again to stop the checking system. ", positions.bottomRight),

                    ("play_button", "This button also deletes your test marbles, similarly to the trashcan.", positions.bottomRight),

                    ("speed_button", $"And lastly, a {brighten}Speed Button{end} - by clicking on it, you can make the marbles " +
                    $"roll {brighten}faster {end}or {brighten}slower{end}.",  positions.bottomRight),

                    ("speed_button", $"The speed will {brighten}change with each click{end}.\n\nThe button will show " +
                    $"you how fast the system currently runs.",  positions.bottomRight),

                    ("side_menu", $"Other than these buttons, you also have {brighten}a side menu{end}.", positions.middleLeft),

                    ("instruction_button", $"First here is an {brighten}Instruction{end} button.\n\n" +
                    $"Click it to {brighten}view your task{end}, it will tell you what your current goal is.", positions.middleLeft),

                    ("settings_button", $"Next is a {brighten}Settings{end} button.\n\nYou can open it to change your {brighten}volume settings{end} " +
                    "anytime.", positions.middleLeft),

                    ("level_selection_button", $"Under that there is a {brighten}Level Selection{end} button.\n\nThis is for when you want to " +
                    $"go back to the menu with selection of the levels.", positions.middleLeft),

                    ("main_menu_button", $"You can also go to the main menu with this {brighten}Main Menu{end} button here.", positions.middleLeft),

                    ("tutorial_button", $"If you would like to {brighten}display a tutorial{end} again, you can open it here.", positions.middleLeft),

                    ("hint_button", $"In case you have any struggles with the level, there is a {brighten}Hint{end} button as well.", positions.middleLeft),

                    ("empty", $"Now the time has come for you to {brighten}try out all the buttons by yourself{end}.", positions.center),

                    ("empty", $"When you feel ready, try to {brighten}solve the task{end} given in the instruction.", positions.center),

                    ("play_button", $"There is no need to rush. Once you believe you solved the task, hit the {brighten}Start{end} button.", 
                    positions.center),

                    ("level_complete_text", $"The {brighten}checking system{end} will run and if your solution will give " +
                    $"{brighten}a correct result{end}, you will get {brighten}a message{end} right here.",  positions.bottomRight),

                    ("next_level_button", $"Here will also appear a {brighten}Next Level{end} button. Good luck!",  positions.bottomRight),

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
            case Level.Level23:
                return new List<(string, string, Vector2)>() {

                    ("empty", "Pay attention...", positions.center),

                    ("block_button", "No blocks here is not a bug!", positions.topLeft),

                    ("block_button", "This level does not require placing any.", positions.topLeft),
                };
            case Level.Level25:
                return new List<(string, string, Vector2)>() {

                    ("empty", "Pay attention...", positions.center),

                    ("generated_board", "No blocks being set up here is not a bug!", positions.topLeft),

                    ("empty", "It is completely up to you how the path will look like.", positions.topLeft)
                };
            case Level.Level27:
                return new List<(string, string, Vector2)>() {

                    ("empty", "Pay attention...", positions.center),

                    ("switch_sides_button", "In some tasks you can decide the side that releases the first marble.", positions.bottomRight),

                    ("dispenser_lamps", "Do you remember these lamps? They will show you the side you have currently chosen.", positions.bottomRight),

                    ("switch_sides_button", "If you want to pick a different side than the one which is chosen at the moment, just " +
                    "hit this button anytime before starting the system.", positions.bottomRight),

                    ("dispenser_lamps", $"{brighten}Pay attention to them!{end} You will often need to change the side!", positions.bottomRight),
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
