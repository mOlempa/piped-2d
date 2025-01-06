using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stopper : MonoBehaviour
{
    Block block;
    SpriteRenderer spriteRenderer;
    int cell = 0;


    private void Start()
    {
        cell = LevelManager.Instance.posToCellDictionary[gameObject.transform.position];
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Detect the ball entering and leaving the block for ball's ignoreFall control
    private void OnTriggerEnter2D(Collider2D other)
    {
        block.detectBallEnterBlock(other.gameObject, cell);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        block.detectBallExitBlock(other.gameObject, cell);
    }

    private void OnMouseOver()
    {
        // Block player interaction with the blocks once simulation has started
        // And do not interact with elements locked on the board
        if (GameManager.Instance.marblesAreRunning ||
            LevelManager.Instance.lockedCellCenters.Contains(gameObject.transform.position))
        {
            return;
        }

        // Mouse right-click to remove the object
        if (Input.GetMouseButtonDown(1))
        {
            deleteBlock();
        }

    }

    private void OnMouseEnter()
    {
        changeSpriteColor(block.highlightColor);
    }

    private void OnMouseExit()
    {
        changeSpriteColor(Color.white);
    }

    private void deleteBlock()
    {
        block.destroyBlock(gameObject);
    }


    private void changeSpriteColor(Color color)
    {
        spriteRenderer.material.color = color;
    }
}
