using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BitTriggers : MonoBehaviour
{
    [SerializeField]
    Sprite lampOnSprite;

    [SerializeField]
    Sprite lampOffSprite;

    [SerializeField]
    SpriteRenderer bitSpriteRenderer;

    [SerializeField]
    Bit blockScript;

    int thisBitCell;


    private void Start()
    {
        if(!getParentBitObject().TryGetComponent<Bit>(out blockScript)){
            print("<color=red>Couldn't assign Block script to bit triggers");
        }
        thisBitCell = LevelManager.Instance.posToCellDictionary[getParentBitObject().transform.position];
    }

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
