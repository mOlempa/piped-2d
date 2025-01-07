using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HintPanel : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI hint;

    [SerializeField]
    TextMeshProUGUI solutionButtonText;

    [SerializeField]
    GameObject solution;

    [SerializeField]
    Image solutionImage;

    private void Start()
    {
        solution.SetActive(false);
        hint.gameObject.SetActive(true);
        solutionButtonText.text = "Show Solution";
    }

    private void OnEnable()
    {
        hint.text = getHint();
    }

    public void closeHint()
    {
        gameObject.SetActive(false);
    }

    public void showSolution()
    {
        hint.gameObject.SetActive(false);
        solution.SetActive(true);
        try
        {
            Sprite sprite = Resources.Load<Sprite>("Images/Solutions/solution_" + (int)GameManager.Instance.currentLevel);
            solutionImage.sprite = sprite;
        }
        catch(Exception ex)
        {
            Debug.LogException(ex);
        }
        solutionButtonText.text = "Hide Solution";
    }
    public void hideSolution()
    {
        solution.SetActive(false);
        hint.gameObject.SetActive(true);
        solutionButtonText.text = "Show Solution";
    }

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


    private string getHint()
    {
        string blue = "<color=#4E8FF6><b>blue</b></color>";
        string white = "<color=#F8EBF6><b>white</b></color>";
        string on = "<b><color=#BF9453>ON</b></color>";


        switch (GameManager.Instance.currentLevel)
        {
            case Level.Level14:
                return $"To make the {white} marble roll into the <b>Stopper</b>, you have to first let the {blue} marble roll to the output.\n\n" +
                    "Also, system does a new thing on this level (more on that in tutorial).";
            case Level.Level15:
                return "Not all elements need to be used... Try out a solution without the Crossovers.";
            case Level.Level18:
                return $"The {white} marbles do not need to be dispensed.\n\n" +
                    "Also, some Bits do not need to be checked. If case of AND, it is enough that one of the Bits is off.";
            case Level.Level20:
                return $"In case of OR, it is enough that one Bit is {on}. Do not roll white marbles in this case.";
            case Level.Level23:
                return "Use the <b>Bit Register</b> to count 4 marbles.\n\nSet it up in a way that 5th marble ends up in the <b>Stopper</b>";
            case Level.Level28:
                return "Place a Bit next to the one placed already and connect them with a Gear.";
            case Level.Level31:
                return "Place two new Bits next to <b>Bit A</b> and connect them with a <b>Gear</b>.\n\n" +
                    "Observe how it resets to the starting state with each marble.";
            default:
                return "No hint here.";
        }
    }
}
