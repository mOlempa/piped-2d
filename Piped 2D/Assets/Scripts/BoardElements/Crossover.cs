using UnityEngine;

/**
 * A class controlling block specific behaviour of a Crossover type.
 */
public class Crossover : MonoBehaviour
{
    /**
     * A private void method that is called when the mouse cursor is over the object. It blocks spawning test marbles inside due to
     * its colliders being dependent on the side a marble enters the Crossover.
     */
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

    /**
     * A private void method that is called when the mouse cursor exits the object.
     */
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
