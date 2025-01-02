using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Ramp : MonoBehaviour
{
    //public float angularVelocity;
    Color highlightColor = new Color(0.8f, 0.6f, 1);

    Rigidbody2D rb;

    public GameObject bitTop;
    public bool isRotating = false;
    public bool stopRotation = false;
    public int direction = 1;
    public bool isOn = true;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        //angularVelocity = GameManager.Instance.rampAngularVelocity;
    }

    
    private void OnMouseOver()
    {
        changeSpriteColor(highlightColor);
        if (Input.GetMouseButtonDown(0))
        {
            // Don't allow interaction with the ramp if player is "holding" a block to spawn
            if (GameManager.Instance.objToSpawnIsActive)
            {
                return;
            }
            onClick();
        }

    }
    private void OnMouseExit()
    {
        changeSpriteColor(Color.white);
    }

    private void changeSpriteColor(Color color)
    {
        gameObject.GetComponent<SpriteRenderer>().material.color = color;
    }

    public void onClick()
    {
        // Block player interaction with the ramps once simulation has started
        if (GameManager.Instance.marblesAreRunning)
        {
            return;
        }
        rotateRamp();
        rotateConnectedRamps();

    }

    public void rotateRamp()
    {
        StartCoroutine(rotateCoroutine());
    }

    IEnumerator rotateCoroutine()
    {
        if (isRotating)
        {
            yield break;
        }

        AudioManager.Instance.playRandomBitRotateSfx();

        isRotating = true;
        while (stopRotation == false)
        {
            yield return new WaitForSeconds(0.025f);
            rb.MoveRotation(rb.rotation + 2 * direction);
        }

        isOn = (direction == 1) ? false : true;

        stopRotation = false;
        isRotating = false;
        direction *= -1;
    }

    public void stopRotating()
    {
        stopRotation = true;
    }





    // Pass through other blocks
    private void OnCollisionEnter2D(Collision2D other)
    {
        GameObject otherObject = other.GetContact(0).collider.gameObject;

        if(other.GetContact(0).collider == transform.parent.GetComponent<Collider2D>())
        {
            stopRotating();
            return;
        }

        // If the other collider is not the top of the current Bit and it is not a ball or a ramp blocker
        if (other.collider != transform.parent.GetComponent<Collider2D>())
        {
            if (otherObject.CompareTag("RedBall") || otherObject.CompareTag("BlueBall") ||
                otherObject.CompareTag("RightBlocker") || otherObject.CompareTag("LeftBlocker"))
            {
                return;
            }
            else
            {
                Physics2D.IgnoreCollision(other.GetContact(0).collider, GetComponent<Collider2D>());
            }

        }
    }

    // When ball enters the ramp
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BlueBall") || other.gameObject.CompareTag("RedBall"))
        {
            rotateRamp();
            rotateConnectedRamps();
        }
    }

    public void resetRamp()
    {
        if (!isOn)
        {
            return;
        }
        //print("Resetting ramp at " + LevelManager.Instance.posToCellDictionary[gameObject.transform.parent.parent.transform.position]);
        direction = 1;
        rotateRamp();
        rotateConnectedRamps(true);
    }

    public void setRamp()
    {
        if (isOn)
        {
            return;
        }
        direction = -1;
        rotateRamp();
        rotateConnectedRamps();
    }

    private GameObject findBitParentObject()
    {
        return transform.parent.parent.gameObject;
    }



    void rotateConnectedRamps(bool isReset = false)
    {
        var thisCell = LevelManager.Instance.posToCellDictionary[findBitParentObject().transform.position];

        // For each bit connection saved in Game Manager
        foreach (List<(int cell, GameObject bit, GameObject ramp)> chain in GameManager.Instance.bitConnectionChains.Distinct())
        {
            if (chain.Any(tuple => tuple.cell == thisCell))
            {
                foreach ((int cell, GameObject bit, GameObject ramp) tuple in chain)
                {
                    //If it is not the cell of the current bit
                    if (tuple.cell != thisCell)
                    {
                        Ramp rampScript = tuple.ramp.GetComponent<Ramp>();

                        // If the main ramp receives reset, do not make other ramps just rotate in opposite direction to current, but reset
                        if (!(isReset && rampScript.isOn == false))
                        {
                            rampScript.rotateRamp();
                        }
                    }
                }
                break;
            }
        }

    }

}
