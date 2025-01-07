using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeleteBit : MonoBehaviour
{
    UpdateBitConnection connection;
    int cell = 0;

    private void Start()
    {
        connection = gameObject.GetComponent<UpdateBitConnection>();
        cell = LevelManager.Instance.posToCellDictionary[gameObject.transform.position];
    }


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
