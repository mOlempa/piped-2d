using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitBlockTrigger : MonoBehaviour
{
    Block block = new Block();
    int cell = 0;
    private void Start()
    {
        cell = LevelManager.Instance.posToCellDictionary[gameObject.transform.parent.position];
    }


    // Detect the ball entering and leaving the Bit for ball's ignoreFall control
    private void OnTriggerEnter2D(Collider2D other)
    {
        block.detectBallEnterBlock(other.gameObject, cell);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        block.detectBallExitBlock(other.gameObject, cell);
    }
}
