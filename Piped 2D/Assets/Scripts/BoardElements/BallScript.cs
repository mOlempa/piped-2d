using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  class handling the behaviour of the marbles.
 */
public class BallScript : MonoBehaviour
{
    /**
     * A private Material field for referencing the material the marble needs to change to when it gets destroyed.
     * Serialized in order to access it from the Unity editor.
     */
    [SerializeField]
    UnityEngine.Material darkBlueMaterial;

    /**
     * A private ParticleSystem field for referencing the particle system that releases coloured smoke particles after the marble gets destroyed.
     * Serialized in order to access it from the Unity editor.
     */
    [SerializeField]
    ParticleSystem smoke;

    /**
     * A private RigidBody field for referencing the rigidbody of the marble
     */
    Rigidbody2D rb;

    /**
     * A private Vector2 field for storing the direction the marble is currently moving in.
     */
    Vector2 dir = Vector2.zero;

    /**
     * A public int field that defines the strength the marble jumps with when the player clicks it.
     */
    public int strength = 10;

    /**
     * A public Dictionary that keeps track of whether the marble is inside any blocks or not.
     */
    public Dictionary<int, bool> isInBlocks = new Dictionary<int, bool>();

    /**
     * A public List that keeps track of which blocks the marble is currently inside.
     */
    public List<int> blocksThatBallIsIn = new List<int>();

    /**
     * A public bool field that decides whether the marble needs to get destroyed on collision or not.
     */
    public bool ignoreFall = false;

    /**
     * A public bool field that helps with decision whether the marble needs to get 
     * destroyed on collision or not - it does not when the marble is at the very bottom of the 
     * board right after leaving a path from blocks.
     */
    public bool isAtOutput = false;

    /**
     * A public bool deciding whether the marble can play a glass hit sound effect.
     */
    public bool canPlayGlassSfx = true;

    /**
     * A public bool deciding whether the marble can play a metal hit sound effect.
     */
    public bool canPlayMetalSfx = true;

    /**
     * A private bool deciding whether the marble gets destroyed on collision or not.
     */
    bool destroyOnHitGround = false;

    /**
     * A private bool stating whether the coroutine destroying the marble has started.
     */
    bool destroyCoroutineStarted = false;

    /**
     * A private bool deciding whether the marble will get respawned after getting destroyed.
     */
    bool willRespawn = false;

    /**
    * A private void method called when the script instance is being loaded. It sets necessary variables.
    */
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /**
     * A private void method called every frame.
     */
    private void Update()
    {
        // Stop the ball from going too fast
        if (rb.velocity.magnitude > 7)
        {
            rb.velocity = rb.velocity.normalized * 7;
        }

        // If script detects the ball falling for too long and it's not at the very bottom of the board,
        // destroy the ball once it hits the ground
        if (rb.velocity.magnitude > 6 && !ignoreFall && !isAtOutput)
        {
            destroyOnHitGround = true;

            // Change the tag so that the triggers don't recognize this ball as a regular one
            gameObject.tag = "BrokenBall";
        }

    }

    /**
     * A private void method called on collision with another object.
     */
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Destroy the ball if it was falling
        if (destroyOnHitGround)
        {
            if (destroyCoroutineStarted)
            {
                return;
            }
            MeshRenderer mr = gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
            mr.material = darkBlueMaterial;
            if (!AudioManager.Instance.sfxSource.mute)
            {
                AudioManager.Instance.playMarbleShatterSfx();
            }
            StartCoroutine(destroyCoroutine());
        }

        // Do not proceed to playing sound effects if they were muted from the menu
        if (AudioManager.Instance.sfxSource.mute)
        {
            return;
        }

        float volume = other.relativeVelocity.magnitude;
        if (volume < 2)
        {
            return;
        }
        volume = volume / 30;

        // Block out sounds from colliding with dispenser
        if (other.gameObject.CompareTag("Dispenser"))
        {
            return;
        }

        HitMaterial otherMaterial = HitMaterial.Metal;
        if (other.collider.CompareTag("BlueBall") || other.collider.CompareTag("RedBall"))
        {
            // Make only one of two collided marbles play the suffix
            if (!canPlayGlassSfx)
            {
                return;
            }
            otherMaterial = HitMaterial.Glass;
            other.gameObject.GetComponent<BallScript>().canPlayGlassSfx = false;
        }
        else
        {
            if (!canPlayMetalSfx)
            {
                return;
            }
            StartCoroutine(soundWaitAfterCollision(0.3f));

            // Make the hit sound louder if it causes the marble to get destroyed
            if (destroyOnHitGround)
            {
                volume = volume * 2f;
            }
            else
            {
                volume = volume * 0.7f;
            }
        }


        AudioManager.Instance.playRandomMarbleHitSfx(volume, otherMaterial);
    }

    /**
     * A private coroutine that handles special effects before destroying the marble and then destroying it.
     */
    IEnumerator destroyCoroutine()
    {
        destroyCoroutineStarted = true;
        smoke.Play();
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    /**
     * A private coroutine that does not allow multiple marble hit sounds to play at almost the same time.
     */
    IEnumerator soundWaitAfterCollision(float sec)
    {
        canPlayMetalSfx = false;
        yield return new WaitForSeconds(sec);
        canPlayMetalSfx = true;
    }

    /**
     * A private void method called when the mouse cursor is over the object.
     * Is allows the player to nudge the ball in case it gets unexpectedly stuck.
     */
    private void OnMouseOver()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        dir = new Vector2(mouseWorldPos.x - transform.position.x, mouseWorldPos.y - transform.position.y);

        // If the ball is clicked while not being in motion
        if (Input.GetMouseButtonDown(0))
        {
            if (rb.velocity.magnitude == 0)
            {
                rb.AddForce(-dir.normalized * strength, ForceMode2D.Impulse);
            }
        }
    }

    /**
     * A public void method that handles the events after marble enters a block.
     * 
     * @param cell number of the block the marble is entering.
     */
    public void handleBlockEnter(int cell)
    {
        // Add the ball to the list for respawning after restarting the simulation
        if (GameManager.Instance.marblesAreRunning)
        {
            // But do it only after the first block it enters, not with every single one
            if (!willRespawn)
            {
                willRespawn = true;
            }
        }
        // Save the block cell number the ball just entered
        if (!blocksThatBallIsIn.Contains(cell))
        {
            blocksThatBallIsIn.Add(cell);
        }
        ignoreFall = true;
    }

    /**
     * A public void method that handles the events after marble exits a block.
     * 
     * @param cell number of the block the marble is entering.
     */
    public void handleBlockExit(int cell)
    {
        if (blocksThatBallIsIn.Contains(cell))
        {
            // Remove the block cell number the ball just exited from the list
            blocksThatBallIsIn.Remove(cell);

            // If the ball is no longer in a block nor it is at the bottom of the board, allow it to get destroyed on impact
            if (blocksThatBallIsIn.Count == 0)
            {
                if (!isAtOutput)
                {
                    ignoreFall = false;
                }
            }
        }
    }



}
