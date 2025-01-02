using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BitTriggers : MonoBehaviour
{
    public Sprite lampOnSprite;
    public Sprite lampOffSprite;
    public SpriteRenderer bitSpriteRenderer;
    public Block blockScript;

    public GameObject ballBarrier;
    public GameObject rampBlockingColliderLeft;
    public GameObject rampBlockingColliderRight;
    //public Ramp rampScript;

    int thisBitCell;


    private void Start()
    {
        if(!getParentBitObject().TryGetComponent<Block>(out blockScript)){
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
            //StartCoroutine(blockRamp(rampBlockingColliderLeft));
        }

        if (gameObject.CompareTag("RightTrigger"))
        {
            bitSpriteRenderer.sprite = lampOffSprite;
            GameManager.Instance.changeBitStateVar(thisBitCell, false);
            if (blockScript != null) { blockScript.isOn = false; }
            //StartCoroutine(blockRamp(rampBlockingColliderRight));
        }
        //rampScript.resetReferenceForConnectedRamps();
    }


    /*private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("BlueBall") || other.CompareTag("RedBall") || other.isTrigger)
        {
            return;
        }

        print("Trigger exit");

        Ramp rampScript = null;

        if (gameObject.CompareTag("LeftTrigger") || gameObject.CompareTag("RightTrigger"))
        {
            print(gameObject.tag);

            foreach(List<(int cell, GameObject, GameObject)> chain in GameManager.Instance.bitConnectionChains)
            {
                if(chain.Any(tuple => tuple.cell == thisBitCell))
                {
                    print("<color=green> Found connections to this bit");
                    foreach((int cell, GameObject, GameObject ramp) tuple in chain)
                    {
                        print("This bit cell: " + thisBitCell + ", cell to push: " + tuple.cell);
                        if(tuple.cell != thisBitCell) {
                            print("<color=green> Pushing the ramp at " + tuple.cell);
                            rampScript = tuple.ramp.GetComponent<Ramp>();
                            rampScript.pushRamp();
                        }
                    }
                }
            }
        }

    }*/

    // Blocking the ramp so it doesn't bounce back after hitting top of the bit
    /*IEnumerator blockRamp(GameObject blockingCollider)
    {
        //if (!rampScript.disableRampBlockers)
        //{
            blockingCollider.SetActive(true);
            yield return new WaitForSeconds(0.25f);
            blockingCollider.SetActive(false);
        //}

    }*/

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
