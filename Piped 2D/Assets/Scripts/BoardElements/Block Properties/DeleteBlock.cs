using UnityEngine;

public class DeleteBlock : MonoBehaviour
{
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
            destroyBlock();
        }
    }

    public void destroyBlock()
    {
        // Delete the block
        Destroy(gameObject);
        // Add the cell of the deleted block to the available cells
        GameManager.Instance.availableRegularCellCenters.Add(gameObject.transform.position);
        // Increase counter of the type of the block deleted
        GameManager.Instance.increaseCounter(GameManager.Instance.getObjTypeByTag(gameObject.tag));
        // Play a random sound effect from place/delete sfx array
        AudioManager.Instance.playRandomBlockPlaceSfx();
    }
}
