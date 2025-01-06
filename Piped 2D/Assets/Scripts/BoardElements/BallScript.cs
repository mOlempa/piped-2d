using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BallScript : MonoBehaviour
{
    [SerializeField]
    UnityEngine.Material darkBlueMaterial;

    Rigidbody2D rb;
    Vector2 dir = Vector2.zero;
    public int strength = 10;
    public Dictionary<int, bool> isInBlocks = new Dictionary<int, bool>();
    public List<int> blocksThatBallIsIn = new List<int>();
    public bool ignoreFall = false;
    public bool isAtOutput = false;

    public bool canPlayGlassSfx = true;
    public bool canPlayMetalSfx = true;

    bool destroyOnHitGround = false;
    bool destroyCoroutineStarted = false;

    bool willRespawn = false;

    Ball ballType;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ballType = gameObject.CompareTag("BlueBall") ? Ball.Blue : Ball.Red;
    }

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

    IEnumerator destroyCoroutine()
    {
        destroyCoroutineStarted = true;
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    IEnumerator soundWaitAfterCollision(float sec)
    {
        canPlayMetalSfx = false;
        yield return new WaitForSeconds(sec);
        canPlayMetalSfx = true;
    }


    // Allow the player to nudge the ball in case it gets unexpectedly stuck
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

        // When right-clicked on the ball, if it's a test ball, delete it
        if (Input.GetMouseButtonDown(1))
        {
            if (GameManager.Instance.testBallObjects.Contains(gameObject))
            {
                Destroy(gameObject);
            }
        }
    }

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
