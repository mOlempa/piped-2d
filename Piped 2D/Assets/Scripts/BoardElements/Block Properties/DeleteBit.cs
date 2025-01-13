using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
* A class that handles deleting the Bit block from the board.
*/
public class DeleteBit : MonoBehaviour
{
    /**
     * A private UpdateBitConnection field for referencing the script that updates the connections in all Bit connection nets.
     */
    UpdateBitConnection connection;

    /**
     * A private int field that stores the board cell number the Bit block is in.
     */
    int cell = 0;

    /**
    * A private void method called when the script instance is being loaded. It sets necessary variables.
    */
    private void Start()
    {
        connection = gameObject.GetComponent<UpdateBitConnection>();
        cell = LevelManager.Instance.posToCellDictionary[gameObject.transform.position];
    }

    /**
     * A private void method that is called when the mouse cursor is over the object. It handles deletion of the Bit.
     */
    private void OnMouseOver()
    {
        // Block player interaction with the blocks once simulation has started
        if (GameManager.Instance.marblesAreRunning ||
            LevelManager.Instance.lockedCellCenters.Contains(gameObject.transform.position))
        {
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            GameManager.Instance.deleteBitFromLists(cell);
            deleteBitFromConnectionChain();

            // Bit has its own deletion mechanism
            GameManager.Instance.availableRegularCellCenters.Add(gameObject.transform.position);
            Destroy(gameObject);
            Debug.Log("<color=grey>Block deleted");
            GameManager.Instance.increaseCounter(GameManager.Instance.getObjTypeByTag(gameObject.tag));
            AudioManager.Instance.playRandomBlockPlaceSfx();
        }

    }

    /**
     * A private void method that handles deleting the Bit object from the connection net stores in the Game Manager.
     */
    private void deleteBitFromConnectionChain()
    {
        foreach (List<(int cell, GameObject bit, GameObject ramp)> chain in GameManager.Instance.bitConnectionChains)
        {
            if (chain.Any(chain => chain.cell == cell))
            {
                // Remove the whole chain for reevaluation (in case deleting the Bit split it into two)
                GameManager.Instance.bitConnectionChains.Remove(chain);

                // Find any Gears that were connected to this Bit and reevaluate connection chains for those Gears
                connection.updateBitConnections(cell);

                //chain.RemoveAll(chain => chain.cell == cell);

                break;
            }
        }

        print("-------All bit connections after deleting bit:-------");
        foreach (List<(int cell, GameObject bit, GameObject ramp)> chain in GameManager.Instance.bitConnectionChains)
        {
            print("Connection chain:");
            foreach ((int cell, GameObject bit, GameObject ramp) tuple in chain)
            {
                print(tuple.cell);
            }
        }
    }

    
}
