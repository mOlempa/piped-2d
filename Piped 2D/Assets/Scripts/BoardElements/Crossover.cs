using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossover : MonoBehaviour
{
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
        }

    }


    private void OnMouseExit()
    {
        if (GameManager.Instance.marblesAreRunning)
        {
            return;
        }

        // Block spawning test marbles in a crossover since its colliders depend on triggers placed on their entrances
        GameManager.Instance.blockSpawningTestBalls = false;
    }


}
