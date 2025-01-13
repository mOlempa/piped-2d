using UnityEngine;

/**
 * A class controlling the lamps next ot the dispensers.
 */
public class DispenserLamp : MonoBehaviour
{
    /**
    * A private Sprite for referencing the sprite for the lamp when it is on.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    Sprite lampOn;

    /**
    * A private Sprite for referencing the sprite for the lamp when it is off.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    Sprite lampOff;

    /**
    * A private bool field for storing the on/off state of the lamp.
    */
    bool lampIsOn;

    /**
    * A private SpriteRenderer field for referencing the sprite renderer of the lamp.
    */
    SpriteRenderer lampImage;

    /**
    * A private Ball field for storing the information about which side the lamp is on - left or right.
    */
    Ball thisSide;

    /**
     * A public bool field that informs whether the lamp was changed during the simulation or by the player
     */
    bool changedLampFromSolutionCase = false;

    /**
    * A private void method called when the script instance is being loaded. It sets necessary variables.
    */
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

    /**
    * A private void method called every frame.
    */
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

    /**
     * A public void method toggling the lamp on/off state.
     */
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

    /**
     * A public void method toggling the lamp off.
     */
    void turnLampOff()
    {
        lampIsOn = false;
        lampImage.sprite = lampOff;
    }

    /**
     * A public void method toggling the lamp on.
     */
    void turnLampOn()
    {
        lampIsOn = true;
        lampImage.sprite = lampOn;
    }

}
