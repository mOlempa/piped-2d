using UnityEngine;

/**
 * A class controlling the behaviour of the bit triggers setup for the Bit's ramp.
 */
public class BitTriggers : MonoBehaviour
{
    /**
    * A private Sprite field for referencing the sprite for the lamp in an on state.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    Sprite lampOnSprite;

    /**
    * A private Sprite field for referencing the sprite for the lamp in an off state.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    Sprite lampOffSprite;

    /**
    * A private SpriteRenderer field for referencing the sprite renderer of the Bit to change the sprite on trigger.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    SpriteRenderer bitSpriteRenderer;

    /**
    * A private Bit field for referencing the script of the Bit.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    Bit blockScript;

    /**
     * A private int field for storing the board cell number the bit is currently in.
     */
    int thisBitCell;

    /**
    * A private void method called when the script instance is being loaded. It sets necessary variables.
    */
    private void Start()
    {
        if(!getParentBitObject().TryGetComponent<Bit>(out blockScript)){
            print("<color=red>Couldn't assign Block script to bit triggers");
        }
        thisBitCell = LevelManager.Instance.posToCellDictionary[getParentBitObject().transform.position];
    }

    /**
    * A private void method called when an object enters the trigger. It detects the ramp rotating.
    */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BlueBall") || other.CompareTag("RedBall") || other.isTrigger)
        {
            return;
        }

        AudioManager.Instance.playSfx("BitSwitch", true);

        if (gameObject.CompareTag("LeftTrigger"))
        {
            bitSpriteRenderer.sprite = lampOnSprite;
            GameManager.Instance.changeBitStateVar(thisBitCell, true);
            if(blockScript != null) { blockScript.isOn = true; }
        }

        if (gameObject.CompareTag("RightTrigger"))
        {
            bitSpriteRenderer.sprite = lampOffSprite;
            GameManager.Instance.changeBitStateVar(thisBitCell, false);
            if (blockScript != null) { blockScript.isOn = false; }
        }
    }

    /**
    * A private GameObject method that returns the parent Bit object of the triggers.
    * 
    * @return GameObject parent Bit object.
    */
    GameObject getParentBitObject()
    {
        GameObject parent = transform.parent.gameObject;
        if(parent != null)
        {
            return parent;
        }
        else
        {
            Debug.LogError("ERROR: Something went wrong - no parent found in this Bit object!");
            return null;
        }
    }

}
