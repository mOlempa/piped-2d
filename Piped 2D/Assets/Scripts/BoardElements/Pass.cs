using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pass : MonoBehaviour
{
    /*Block block = new Block();
    SpriteRenderer spriteRenderer;
    int cell = 0;
*/

    /*private void Start()
    {
        cell = LevelManager.Instance.posToCellDictionary[gameObject.transform.position];
        spriteRenderer = GetComponent<SpriteRenderer>();
    }*/

    /*// Detect the ball entering and leaving the block for ball's ignoreFall control
    private void OnTriggerEnter2D(Collider2D other)
    {
        block.detectBallEnterBlock(other.gameObject, cell);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        block.detectBallExitBlock(other.gameObject, cell);
    }*/

    private void OnMouseOver()
    {
        // Block player interaction with the blocks once simulation has started
        // And do not interact with elements locked on the board
        if (GameManager.Instance.marblesAreRunning ||
            LevelManager.Instance.lockedCellCenters.Contains(gameObject.transform.position))
        {
            return;
        }

        /*// Mouse right-click to remove the object
        if (Input.GetMouseButtonDown(1))
        {
            block.destroyBlock(gameObject);
        }*/

        // Mouse left-click to mirror the object
        if (Input.GetMouseButtonDown(0))
        {
            mirrorBlock();
        }

    }

    /*private void OnMouseEnter()
    {
        changeSpriteColor(block.highlightColor);
    }

    private void OnMouseExit()
    {
        changeSpriteColor(Color.white);
    }*/

    private void mirrorBlock()
    {
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * -1,
            gameObject.transform.localScale.y,
            gameObject.transform.localScale.z);

        AudioManager.Instance.playSfx("BlockMirror");
    }

    


    /*private void changeSpriteColor(Color color)
    {
        spriteRenderer.material.color = color;
    }*/
}
