using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DispenserLamp : MonoBehaviour
{
    [SerializeField]
    Sprite lampOn;

    [SerializeField]
    Sprite lampOff;

    bool lampIsOn;
    SpriteRenderer lampImage;
    Ball thisSide;

    bool changedLampFromSolutionCase = false;


    private void Start()
    {
        lampImage = GetComponent<SpriteRenderer>();
        Ball startingSide = LevelManager.Instance.getLevelStartingSide();
        if(startingSide == Ball.Neither)
        {
            startingSide = Ball.Red;
        }

        thisSide = (gameObject.CompareTag("LeftLamp")) ? Ball.Blue : Ball.Red;
        

        if(thisSide == startingSide)
        {
            turnLampOn();
        }
        else
        {
            turnLampOff();
        }
    }

    private void Update()
    {
        if (!GameManager.Instance.marblesAreRunning)
        {
            if (changedLampFromSolutionCase)
            {
                changedLampFromSolutionCase = false;
            }
            return;
        }
        if (changedLampFromSolutionCase)
        {
            return;
        }
        if (!GameManager.Instance.turnLeftWheels || !GameManager.Instance.turnRightWheels)
        {
            return;
        }
        if (GameManager.Instance.currentSolutionCase.startingSideCase != Ball.Neither)
        {
            if (GameManager.Instance.currentSolutionCase.startingSideCase == thisSide)
            {
                turnLampOn();
                print("Changing lamp on side " + thisSide + " to ON");
            }
            else
            {
                turnLampOff();
                print("Changing lamp on side " + thisSide + " to OFF");
            }
            changedLampFromSolutionCase = true;
        }
        
    }

    public void toggleLamp()
    {
        if (lampIsOn)
        {
            turnLampOff();
        }
        else
        {
            turnLampOn();
            GameManager.Instance.changeStartingSide(thisSide);
        }
    }

    void turnLampOff()
    {
        lampIsOn = false;
        lampImage.sprite = lampOff;
    }

    void turnLampOn()
    {
        lampIsOn = true;
        lampImage.sprite = lampOn;
    }

}
