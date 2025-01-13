using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * A class controlling the behavior of the ramp of a Bit.
 */
public class Ramp : MonoBehaviour
{
    /**
     * A private RigidBody field for referencing the rigid body of the ramp.
     */
    Rigidbody2D rb;

    /**
     * A public GameObject field for referencing the top part of the Bit.
     */
    public GameObject bitTop;

    /**
     * A public bool field for checking if the ramp is currently rotating.
     */
    public bool isRotating = false;

    /**
     * A public bool field to request stop of the rotation.
     */
    public bool stopRotation = false;

    /**
     * A public bool field for setting direction of the rotation.
     */
    public int direction = 1;

    /**
     * A public bool field for storing the state of the Bit parent of the ramp.
     */
    public bool isOn = true;

    /**
    * A private void method called when the script instance is being loaded. It sets necessary variables.
    */
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    /**
     * A private void method that is called when the mouse cursor is detected over the ramp. 
     * It handles user interaction (clicking on the ramp).
     */
    private void OnMouseOver()
    {
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

    /**
     * A public void method that rotates the ramp on click.
     */
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

    /**
     * A public void method that starts the ramp rotation coroutine.
     */
    public void rotateRamp()
    {
        StartCoroutine(rotateCoroutine());
    }

    /**
     * A private coroutine that rotates the ramp.
     */
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

    /**
     * A public void method that requests stopping the rotation of the ramp.
     */
    public void stopRotating()
    {
        stopRotation = true;
    }

    /**
     * A private void method that is called when the ramp collides with another object.
     */
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
                // Pass through other blocks
                Physics2D.IgnoreCollision(other.GetContact(0).collider, GetComponent<Collider2D>());
            }

        }
    }

    /**
     * A private void method that is called when an object enters the ramp trigger. It handles detection of marbles rolling onto the ramp.
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BlueBall") || other.gameObject.CompareTag("RedBall"))
        {
            rotateRamp();
            rotateConnectedRamps();
        }
    }

    /**
     * A public void method that resets the ramp (rotates to off state)
     */
    public void resetRamp()
    {
        if (!isOn)
        {
            return;
        }
        direction = 1;
        rotateRamp();
        rotateConnectedRamps(true);
    }

    /**
     * A public void method that sets the ramp (rotates to on state)
     */
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

    /**
     * A private GameObject method that returns the Bit parent of the ramp.
     * 
     * @return GameObject Bit parent
     */
    private GameObject findBitParentObject()
    {
        return transform.parent.parent.gameObject;
    }

    /**
     * A private void method that rotates the ramps in the same Bit connection net.
     * 
     * @param optional bool isReset that if true, instead of rotating the ramps connected it will reset them.
     */
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
