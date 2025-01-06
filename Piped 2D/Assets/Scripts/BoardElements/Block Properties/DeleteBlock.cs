using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            destroyBlock(gameObject);
        }
    }

    

    public void destroyBlock(GameObject gameObject)
    {
        GameManager.Instance.availableRegularCellCenters.Add(gameObject.transform.position);
        Object.Destroy(gameObject);
        Debug.Log("<color=grey>Block deleted");
        GameManager.Instance.increaseCounter(GameManager.Instance.getObjTypeByTag(gameObject.tag));
        AudioManager.Instance.playRandomBlockPlaceSfx();
    }

    


    


}
