using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossover : MonoBehaviour
{
    Block block = new Block();
    SpriteRenderer[] spriteRenderers;

    int cell = 0;

    private void Start()
    {
        cell = LevelManager.Instance.posToCellDictionary[gameObject.transform.position];
        spriteRenderers = gameObject.transform.GetComponentsInChildren<SpriteRenderer>(false);
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
        if (GameManager.Instance.marblesAreRunning)
        {
            return;
        }

        // Block spawning test marbles in a crossover since its colliders depend on triggers placed on their entrances
        GameManager.Instance.blockSpawningTestBalls = true;

        // Do not let to interact with a block locked on the board
        if (LevelManager.Instance.lockedCellCenters.Contains(gameObject.transform.position))
        {
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            GameManager.Instance.blockSpawningTestBalls = false;
            block.destroyBlock(gameObject);
        }

    }

    private void OnMouseEnter()
    {
        changeSpriteColor(block.highlightColor);
    }

    private void OnMouseExit()
    {
        if (GameManager.Instance.marblesAreRunning)
        {
            return;
        }


        // Block spawning test marbles in a crossover since its colliders depend on triggers placed on their entrances
        GameManager.Instance.blockSpawningTestBalls = false;

        changeSpriteColor(Color.white);

        
    }




    private void changeSpriteColor(Color color)
    {
        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.material.color = color;
        }
        
    }
}
