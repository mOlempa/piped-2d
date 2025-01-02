using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Hint : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI hint;

    private void OnEnable()
    {
        hint.text = getHint();
    }

    public void closeHint()
    {
        gameObject.SetActive(false);
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
            default:
                return "No hint here.";
        }
    }
}
