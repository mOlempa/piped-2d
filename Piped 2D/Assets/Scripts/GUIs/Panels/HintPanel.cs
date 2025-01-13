using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * A class controlling the hint panel in the UI within the Level scene.
 */
public class HintPanel : MonoBehaviour
{
    /**
     * A private TextMeshProUGUI field for referencing the hint text.
     * Serialized in order to access it from the Unity editor.
     */
    [SerializeField]
    TextMeshProUGUI hint;

    /**
     * A private TextMeshProUGUI field for referencing the text on the button in the hint panel.
     * Serialized in order to access it from the Unity editor.
     */
    [SerializeField]
    TextMeshProUGUI solutionButtonText;

    /**
     * A private GameObject field for referencing the object with an image component containing solution to the level.
     * Serialized in order to access it from the Unity editor.
     */
    [SerializeField]
    GameObject solution;

    /**
     * A private Image field for referencing the image of the solution to the level in the hint panel.
     * Serialized in order to access it from the Unity editor.
     */
    [SerializeField]
    Image solutionImage;

    /**
     * A private void member function called at the start of the scene with the hint panel.
     * It sets all variables to a state they should have at the beginning.
     */
    private void Start()
    {
        solution.SetActive(false);
        hint.gameObject.SetActive(true);
        solutionButtonText.text = "Show Solution";
    }

    /**
     * A private void member function that gets called on enabling the hint panel.
     */
    private void OnEnable()
    {
        hint.text = getHint();
    }

    /**A public void member function that closes the hint panel
     * 
     */
    public void closeHint()
    {
        gameObject.SetActive(false);
    }

    /**
     * A public void member function that shows the solution within the hint panel.
     */
    public void showSolution()
    {
        hint.gameObject.SetActive(false);
        solution.SetActive(true);
        try
        {
            Sprite sprite = Resources.Load<Sprite>("Images/Solutions/Level" + ((int)GameManager.Instance.currentLevel + 1) + "_1000_690");
            solutionImage.sprite = sprite;
        }
        catch(Exception ex)
        {
            Debug.LogException(ex);
        }
        solutionButtonText.text = "Hide Solution";
    }

    /**
     * A public void member function that hides the solution within the hint panel.
     */
    public void hideSolution()
    {
        solution.SetActive(false);
        hint.gameObject.SetActive(true);
        solutionButtonText.text = "Show Solution";
    }

    /**
     * A public void member function that toggles the solution within the hint panel. 
     * It is assigned to the button at the bottom of the panel.
     */
    public void toggleSolution()
    {
        if(solution.activeSelf)
        {
            hideSolution();
        }
        else
        {
            showSolution();
        }
    }

    /**
     * A private member function that returns hint text based on the current level set.
     * @return hint text as string.
     */
    private string getHint()
    {
        string blue = "<color=#4E8FF6><b>blue</b></color>";
        string white = "<color=#F8EBF6><b>white</b></color>";
        string on = "<b><color=#BF9453>ON</b></color>";
        string off = "<b><color=#6D4839>OFF</b></color>";

        switch (GameManager.Instance.currentLevel)
        {
            case Level.Level1:
                return "Create a path the marble can roll through all the way to the ground.";
            case Level.Level2:
                return "Create a path the marble can roll through all the way to the ground on the left side.";
            case Level.Level3:
                return $"Create a path the {blue} marble can roll through all the way to the ground on the <b>right</b> side.";
            case Level.Level4:
                return $"Create a path the {white} marble can roll through all the way to the ground on the <b>left</b> side.";
            case Level.Level5:
                return $"Create a path for the {white} marbles the same way it is created for the {blue} ones.";
            case Level.Level6:
                return $"Use Crossovers for the marbles to go in zigzag.";
            case Level.Level7:
                return $"Not all Crossover have to be used. Find the ones that will work with the amount of Pass pipes that you have now" +
                    $" (yes, it is possible).";
            case Level.Level8:
                return $"Make the first {blue} marble roll to the <b>right</b> side.\n\n" +
                    $"Remember: <b>YOU</b> set the Bit's direction here!";
            case Level.Level9:
                return $"Use the Bit to make first {blue} marble roll on the <b>left</b> side, and second through the crossover onto the " +
                    $"<b>right</b> side, so it releases a {white} marble.";
            case Level.Level10:
                return $"Use the logic of Bit usage from task 9 on both {blue} and {white} marbles.";
            case Level.Level11:
                return $"Use the <b>upper Bit</b> to let first marble roll through the Bit 2, and the second marble roll through the Bit 5.\n\n" +
                    $"Remember: <b>YOU</b> set the upper Bit's direction here!";
            case Level.Level12:
                return $"Fill the path to the Stopper.";
            case Level.Level13:
                return $"Use the <b>Bit</b> to direct the {blue} marble <b>around the Stopper</bi> and onto the left side.";
            case Level.Level14:
                return $"To make the {white} marble roll into the <b>Stopper</b>, you have to first let the {blue} marble roll to the output.\n\n" +
                    "Also, system does a new thing on this level (more on that in the tutorial).";
            case Level.Level15:
                return $"Let first {blue} marble run through Bit A and from there make paths to both of the sides that depend on " +
                    $"the state of Bit A.\n\nAlso, not all elements need to be used.";
            case Level.Level16:
                return $"Create a path down on the side of the Bits.";
            case Level.Level17:
                return $"Use the Bits to direct first two {blue} marbles to the <b>left</b> side, and the third marble to the <b>right</b> to " +
                    $"release {white} marbles next.";
            case Level.Level18:
                return $"The {white} marbles do not need to be dispensed.\n\n" +
                    $"Also, some Bits do not need to be checked. If case of AND, if even one Bit is {off}, the marble should go in the F Stopper.";
            case Level.Level19:
                return $"Some Bits do not need to be checked. If case of AND, if even one Bit is {off}, the {blue} marble should roll to the " +
                    $"<b>right</b> side and release a {white} marble, which should go in the Stopper.";
            case Level.Level20:
                return $"In case of OR, it is enough that one Bit is {on}. Do not roll white marbles in this case.";
            case Level.Level21:
                return $"Use the Pass elements to finish the path. Notice how the marbles get counted by opening the instruction " +
                    $"and <b>looking at the numbers next to the register</b>.";
            case Level.Level22:
                return $"Use the Pass elements to finish the path. Notice how the marbles get counted by opening the instruction " +
                    $"and <b>looking at the numbers next to the register</b>.";
            case Level.Level23:
                return "Use the <b>Bit Register</b> to count 4 marbles.\n\nSet it up in a way that 5th marble ends up in the <b>Stopper</b>";
            case Level.Level24:
                return "Use the <b>Bit Register</b> to count 9 marbles.\n\nSet it up in a way that 10th marble ends up in the <b>Stopper you place</b>";
            case Level.Level25:
                return $"Use the <b>Bits you have available</b> to set up two counters, one that counts 6 {blue} marbles and one that " +
                    $"counts 3 {white} marbles.";
            case Level.Level26:
                return $"Use the <b>Bits you have available</b> to set up a counter that counts 3 {blue} marbles and lets the fourth one " +
                    $"roll onto the right side to release {white} marble.\n\nThis {white} marble needs to trigger {blue} again, and the second " +
                    $"{white} needs to roll in the <b>Stopper</b>";
            case Level.Level27:
                return $"Set up a Bit that will alternate the route for {blue} marbles - let the first marble roll through each of the <b>Bits</b> in the " +
                    $"first column, then the second in the second column and make it trigger {white}, which will roll through the last <b>Bit</b> and into " +
                    $"the <b>Stopper</b> you place.";
            case Level.Level28:
                return "Place a Bit next to the one placed already and connect them with a Gear.";
            case Level.Level29:
                return "Add a Gear and a Bit to the Bit on the board and make the first {blue} marble roll onto the <b>right</b> side, which triggers " +
                    "the {white} marble. Make it roll onto the <b>left</b> side, triggering {blue}, and use the Bits resetting via Gear connection " +
                    "to let all the rest of {blue} roll.";
            case Level.Level30:
                return "Place a new Bit next to <b>Bit V</b> and connect them with a <b>Gear</b>.\n\n" +
                    "Observe how it resets to the starting state with each marble.";
            case Level.Level31:
                return "Place two new Bits next to <b>Bit A</b> and connect them with a <b>Gear</b>.\n\n" +
                    "Observe how it resets to the starting state with each marble.";
            case Level.Level32:
                return $"When the marble released is {blue}, make the Bits reset if they are {off} or roll marble through just one if they are {on}.\n" +
                    $"Do the opposite when it is a {white} marble.";
            case Level.Level33:
                return $"Setup a Bit under {blue} marble dispenser in a way, that the first marble released will check the <b>Bit A</b>. " +
                    $"If it is {off}, lead the marble to the left side, if {on} - to the right. The next released marble should go to Bit B and " +
                    $"possibly change it. Make the change depend on the color of this second marble.";
            case Level.Level34:
                return "Make Bit B connect with a Bit added by you. Use this new Bit to check the state of Bit B and lead the marble accordingly.";
            case Level.Level35:
                return "Make the result depend on the state of the first Bit in the register, as the number can be uneven <b>only</b> when this Bit " +
                    $"is {on} and even when it is {off}.";
            case Level.Level36:
                return $"The first marble needs to check the state of either A or B Bit. It is enough that one of them is {off} to roll the {white} " +
                    $"marble into the <b>Stopper</b>. Add a Bit and the Crossovers to direct the marbles accordingly.";
            case Level.Level37:
                return $"Roll the marbles through registers like counters - each {blue} marble adds a 1 to A and releases a {white} marble, " +
                    $"which adds a 1 to B. Once register A zeroes out, roll the marble in the Stopper.";
            case Level.Level38:
                return "Use Bits at the top of the board to count out the 4 marbles. Put fifth in a Stopper.";
            case Level.Level39:
                return $"Use Bits to count out 4 {blue} marbles, and lead the fourth one to the right side.";
            case Level.Level40:
                return "Use only bottom half of the register to count the marbles.";
            case Level.Level41:
                return $"Use first marble to check the first Bit of register A - if it is {off}, lead the marble to the bottom. If {on}, " +
                    $"lead the marble through Bits of A so that it adds 1 to the number in register A. Make the second marble check second Bit of B " +
                    $"and do the same as with first marble, but this time add a 2.";
            case Level.Level42:
                return "Roll the marbles through registers as if they were counters. The one that finishes counting first (gets zeroed-out) " +
                    "is the one that rolls the ball into the Stopper.\n(Remember that zeroing the register works both with adding and subtracting.)";
            case Level.Level43:
                return "Use Gears to help you check the state without the necessity of leading the marble through the exact Bit.";
            case Level.Level44:
                return $"Roll the marbles through registers as if they were counters. Each {blue} marble rolling through A needs to release " +
                    $"a {white} marble that rolls through B, both subtracting 1 from their register. Once A is 0, stop the {blue} marble in " +
                    $"a Stopper.";
            case Level.Level45:
                return $"Roll the marbles through registers as if they were counters. Roll {blue} marbles through A and B interchangeably, " +
                    $"and if A zeroes out first, lead the {blue} marble into a Stopper. If B is equal to 0 first, release the {white} marble " +
                    $"and roll it into a Stopper.";
            case Level.Level46:
                return "Lead marbles through each Bit, and once all Bits are turned to opposite, stop the marble in a Stopper.";
            case Level.Level47:
                return $"Use Gears to switch {off} each Bit if it is {on}, or reset the state of the Bit the marble " +
                    $"just run through if it was {off}.";
            default:
                return "No hint here.";
        }
    }
}
