using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pass : MonoBehaviour
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

        // Mouse left-click to mirror the object
        if (Input.GetMouseButtonDown(0))
        {
            mirrorBlock();
        }

    }


    private void mirrorBlock()
    {
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * -1,
            gameObject.transform.localScale.y,
            gameObject.transform.localScale.z);

        AudioManager.Instance.playSfx("BlockMirror");
    }

    
}
