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


    public void setAndShowInstruction(string title, string instruction)
    {
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
}
