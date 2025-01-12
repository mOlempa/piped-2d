using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstructionPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI contentText;
    [SerializeField]
    private TextMeshProUGUI titleText;

    private Image overlay;

    private string overlaysPath = "Images/Instruction Overlays/overlay_";

    public bool instructionVisible = false;


    public void showInstruction()
    {
        (string title, string instruction) = getLevelInstruction();
        gameObject.SetActive(true);
        contentText.text = instruction;
        titleText.text = title;
        instructionVisible = true;
        overlay = gameObject.GetComponent<Image>();
        overlay.sprite = getOverlaySprite(GameManager.Instance.currentLevel);
    }

    public void hideInstruction()
    {
        gameObject.SetActive(false);
        instructionVisible = false;
    }


    public Sprite getOverlaySprite(Level level)
    {
        string overlayFile = overlaysPath + "level" + ((int)level + 1);

        if(Resources.Load<Sprite>(overlayFile) == null){
            return Resources.Load<Sprite>(overlaysPath + "empty");
        }
        else
        {
            return Resources.Load<Sprite>(overlayFile);
        }
    }


    public (string title, string instruction) getLevelInstruction()
    {
        string blue = "<color=#4E8FF6><b>blue</b></color>";
        string white = "<color=#F8EBF6><b>white</b></color>";
        string on = "<b><color=#BF9453>ON</b></color>";
        string off = "<b><color=#6D4839>OFF</b></color>";

        switch (GameManager.Instance.currentLevel)
        {
            case Level.Level1:
                return ("Task 1", $"Build a setup in a way that all {blue} marbles roll to the output.\n\n<b>Do not</b> let any {white} marbles roll through.\n\n" +
                    $"Output:\n{blue}\n{blue}\n{blue}\n{blue}\n{blue}\n...");
            case Level.Level2:
                return ("Task 2", $"Build a setup in a way that all {blue} marbles roll to the output.\n\n<b>Do not</b> let any {white} marbles roll through.\n\n" +
                    $"Output:\n{blue}\n{blue}\n{blue}\n{blue}\n{blue}\n...");
            case Level.Level3:
                return ("Task 3", $"Make only one {blue} marble roll to the output, and then all of the {white} ones.\n\n" +
                    $"Output:\n{blue}\n{white}\n{white}\n{white}\n{white}\n...");
            case Level.Level4:
                return ("Task 4", $"Make only one {white} marble roll to the output, and then all of the {blue} ones\n\n" +
                    $"Output:\n{white}\n{blue}\n{blue}\n{blue}\n{blue}\n...");
            case Level.Level5:
                return ("Task 5", $"In the output, make a pattern:\n\n{blue}\n{white}\n{blue}\n{white}\n{blue}\n{white}\n...");
            case Level.Level6:
                return ("Task 6", $"Make a pattern:\n\n{blue}\n{white}\n{blue}\n{white}\n{blue}\n{white}\n...");
            case Level.Level7:
                return ("Task 7", $"Make only {blue} marbles roll to the output.");
            case Level.Level8:
                return ("Task 8", $"Make a pattern:\n\n{blue}\n{white}\n{blue}\n{white}\n{blue}\n{white}\n...");
            case Level.Level9:
                return ("Task 9", $"Make a pattern:\n\n{blue}\n{blue}\n{white}\n{blue}\n{blue}\n{white}\n...");
            case Level.Level10:
                return ("Task 10", $"Make a pattern:\n\n{blue}\n{blue}\n{white}\n{white}\n{blue}\n{blue}\n{white}\n{white}\n...");
            case Level.Level11:
                return ("Task 11", "Make a path for marbles to switch on the bits 2 and 5.");
            case Level.Level12:
                return ("Task 12", $"Stop the {blue} marble.");
            case Level.Level13:
                return ("Task 13", $"Stop the {white} marble.");
            case Level.Level14:
                return ("Task 14", $"If the system starts with the <b>Bit A</b> switched {off}, stop the {blue} marble." +
                    $"\n\nOtherwise stop the {white} one.");
            case Level.Level15:
                return ("Task 15", $"If the system starts with the <b>Bit A</b> switched {off}, stop the {blue} marble." +
                    $"\n\nOtherwise stop the {white} one.");
            case Level.Level16:
                return ("Task 16", $"Let only 3 {blue} marbles get to the output.\n\nStop the 4th one in the <b>Stopper</b>.\n\nOutput:\n" +
                    $"{blue}\n{blue}\n{blue}");
            case Level.Level17:
                return ("Task 17", $"Make a pattern of <b>6</b> marbles:\n\n{blue}\n{blue}\n{blue}\n{white}\n{white}\n{white}");
            case Level.Level18:
                return ("Task 18", $"If Bit A <b>AND</b> Bit B are {on}, make the marble roll into the Stopper T.\n\n" +
                    "Otherwise, make it roll into Stopper F.");
            case Level.Level19:
                return ("Task 19", $"If Bit A <b>AND</b> Bit B are {on}, stop the {blue} marble.\n\nOtherwise, stop {white} marble.");
            case Level.Level20:
                return ("Task 20", $"If Bit A <b>OR</b> Bit B are {on}, stop the {blue} marble.\n\nOtherwise, stop {white} marble.");
            case Level.Level21:
                return ("Task 21", $"Use register A to count the amount of {blue} marbles that roll through.");
            case Level.Level22:
                return ("Task 22", $"Register A starts with a number 15.\n\nSubtract from it the amount of {blue} marbles that roll through\n\n" +
                    $"Example:\nRegister A = 15,\nthen 1 marble rolls,\nand then Register A = 14.");
            case Level.Level23:
                return ("Task 23", $"Let exactly 4 {blue} marbles get to the output.\n\nStop the 5th one in the <b>Stopper</b>.\n\nOutput:\n" +
                    $"{blue}\n{blue}\n{blue}\n{blue}");
            case Level.Level24:
                return ("Task 24", $"Let exactly 9 {blue} marbles get to the output.\n\nStop the 10th one in a " +
                    "<b>Stopper</b> (which you need to place).\n\nOutput:\n" +
                    $"{blue}\n{blue}\n{blue}\n{blue}\n{blue}\n{blue}\n{blue}\n{blue}\n{blue}");
            case Level.Level25:
                return ("Task 25", $"Make a pattern of <b>9</b> marbles:\n\n{blue}\n{blue}\n{blue}\n{blue}\n{blue}\n{blue}\n{white}\n{white}\n{white}");
            case Level.Level26:
                return ("Task 26", $"Make a pattern of <b>9</b> marbles:\n\n{blue}\n{blue}\n{blue}\n{blue}\n{white}\n{blue}\n{blue}\n{blue}\n{blue}");
            case Level.Level27:
                return ("Task 27", "Switch the direction of each of the 9 Bits already placed on the board no matter " +
                    "what direction they start in.");
            case Level.Level28:
                return ("Task 28", $"Release only {blue} marbles.\n\nOutput:\n" +
                    $"{blue}\n{blue}\n{blue}\n{blue}\n{blue}\n{blue}\n{blue}\n{blue}");
            case Level.Level29:
                return ("Task 29", $"Release a {blue} marble, then {white} marble, and then <b>all of the rest</b> of the {blue} marbles.\n\n" +
                    $"Output:\n{blue}\n{white}\n{blue}\n{blue}\n{blue}\n{blue}\n{blue}\n{blue}\n{blue}");
            case Level.Level30:
                return ("Task 30", $"Count {blue} marbles in register A.\n\nIf there is more than 7, make <b>Bit V</b> switch {on} <b>(it has to stay on)</b>, " +
                    "to signal an <b>overflow</b>.\n\n(All marbles are supposed to roll)");
            case Level.Level31:
                return ("Task 31", $"If Bit S is {off}, count all {blue} marbles in register A.\n\nOtherwise, count them in register B.");
            case Level.Level32:
                return ("Task 32", $"For <b>any</b> starting direction of the Bit A, make it switched {off} is a {blue} marble is released.\n\n" +
                    $"If a {white} marble is released, make it switch {on}.");
            case Level.Level33:
                return ("Task 33", $"Create a system where Bit B is in the same state ({on} or {off}) as the Bit A.\n\nDo this " +
                    "without changing the state of Bit A.\n\nAfter that stop the marble.");
            case Level.Level34:
                return ("Task 34", $"If Bit A <b>AND</b> Bit B are in opposite states, stop the {blue} marble.\n\n" +
                    $"Otherwise, stop the {white} marble.");
            case Level.Level35:
                return ("Task 35", $"Stop {blue} marble if register A is an even number.\n\n If it is an odd number, stop the {white} marble.");
            case Level.Level36:
                return ("Task 36", $"If Bit A and Bit B are in an {on} state, stop {blue} marble.\n\nOtherwise, stop {white} marble.");
            case Level.Level37:
                return ("Task 37", "<b>Add</b> number from\nregister A to number in register B and store the result in B.");
            case Level.Level38:
                return ("Task 38", "Make an output of marbles represent the Bit states in register A.\n\n" +
                    $"If Bit is {off}, make it a {white} marble. If Bit is {on}, make it a {blue} marble. Don't count the first marble.\n\n\n" +
                    $"Examples:\n\nRegister A: 0 0 0\nOutput: {blue} {white} {white} {white}\n\nRegister A: 0 <b>1 1</b>\nOutput: {blue} {white} {blue} {blue}");
            case Level.Level39:
                return ("Task 39", $"Make a pattern:\n\n{blue}\n{blue}\n{blue}\n{white}\n{blue}\n{blue}\n{blue}\n{white}\n...");
            case Level.Level40:
                return ("Task 40", $"<b>Multiply</b> the amount of {blue} marbles by 4 and save the result in register A.\n\n");
            case Level.Level41:
                return ("Task 47", "<b>Add</b> register A to register B and store the result in B.");
            case Level.Level42:
                return ("Task 42", $"If register A is bigger than register B, stop the {blue} marble.\n\nOtherwise, stop the {white} marble.");
            case Level.Level43:
                return ("Task 43", $"If Bits A <b>AND</b> B are in opposite states, stop the {blue} marble. Otherwise, stop the {white} marble.");
            case Level.Level44:
                return ("Task 44", "<b>Subtract</b> the register A from register B and store the result in B.\n\nStarting value of A has" +
                    "to be smaller or equal to B (e.g. if B = 5, then A cannot be bigger than 5).");
            case Level.Level45:
                return ("Task 45", $"If register A is <b>smaller or equal</b> to register B, stop the {blue} marble.\n\nOtherwise, stop the {white} marble.");
            case Level.Level46:
                return ("Task 46", "Switch each of the 5 Bits to the opposite no matter what state they start in.");
            case Level.Level47:
                return ("Task 41 - Hard Reset", "For any starting number in the register A, set the register to 0 with releasing only one marble.");
            default:
                Debug.LogWarning("This level does not have an instruction!");
                return ("ERROR", "No instruction found!");
        }
    }

}
