using UnityEngine;

/**
 * A class responsible for block behaviour of type Pass.
 */
public class Pass : MonoBehaviour
{
    /**
     * A private void method called when the mouse cursor is over the object.
     */
    private void OnMouseOver()
    {
        // Block player interaction with the blocks once simulation has started
        // And do not interact with elements locked on the board
        if (GameManager.Instance.marblesAreRunning ||
            LevelManager.Instance.lockedCellCenters.Contains(gameObject.transform.position))
        {
            return;
        }

        // Mouse left-click to mirror the object
        if (Input.GetMouseButtonDown(0))
        {
            mirrorBlock();
        }

    }

    /**
     * A private void for mirroring the Pass object.
     */
    private void mirrorBlock()
    {
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * -1,
            gameObject.transform.localScale.y,
            gameObject.transform.localScale.z);

        AudioManager.Instance.playSfx("BlockMirror");
    }

    
}
