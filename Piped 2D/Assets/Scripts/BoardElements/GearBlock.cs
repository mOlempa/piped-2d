using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GearBlock : MonoBehaviour
{
    bool isRotating = false;
    Rigidbody2D rb;

    public List<int> neighbourBitCells = new List<int>();
    List<(int pos, GameObject bit, GameObject ramp)> thisConnection = new List<(int pos, GameObject bit, GameObject ramp)>();
    Ramp referenceRampScript;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        detectAndConnectNeighbourBits(false);
        GameManager.Instance.allGears.Add(gameObject);
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 1);
    }

    // Rotate the gear together with the ramps from the connection
    private void Update()
    {
        if (thisConnection.Count > 0)
        {
            if (referenceRampScript.isRotating)
            {
                StartCoroutine(rotateCoroutine());
            }
        }
    }

    IEnumerator rotateCoroutine()
    {
        if (isRotating)
        {
            yield break;
        }

        isRotating = true;
        // While the ramp keeps rotating
        while (referenceRampScript.isRotating == true)
        {
            // If the bit with ramp is deleted mid rotation, stop rotating
            if (thisConnection.Count == 0)
            {
                isRotating = false;
                yield break;
            }
            yield return new WaitForSeconds(0.025f);
            rb.MoveRotation(rb.rotation + 1 * referenceRampScript.direction);
        }
        isRotating = false;
    }

    public void detectAndConnectNeighbourBits(bool calledByNewBit, int ignoredBitCell = 0)
    {
        print("Detecting neighbour bits");
        Collider2D[] colliders;
        colliders = Physics2D.OverlapCircleAll(gameObject.transform.position, 1f);
        thisConnection.Clear();

        if (colliders.Length > 1)
        {
            foreach (var collider in colliders)
            {
                if (collider.gameObject.CompareTag("Bit"))
                {
                    // Add the Bit's cell number to the temporary list
                    int cell = LevelManager.Instance.posToCellDictionary[collider.gameObject.transform.position];

                    // If this is not a Bit to ignore (which would be in case of reevaluating connection after deleting Bit)
                    if (ignoredBitCell != cell)
                    {
                        GameObject ramp = getRamp(collider.gameObject);

                        neighbourBitCells.Add(cell);
                        //resetRamp(ramp);
                        thisConnection.Add((cell, collider.gameObject, ramp));
                    }
                }
            }
        }

        // Check if any of the connected bits (cells) have a connection somewhere else. If so, add this current connection to it as a chain.
        bool theCellsHaveConnectionsAlready = false;

        foreach (List<(int cell, GameObject bit, GameObject ramp)> chain in GameManager.Instance.bitConnectionChains)
        {
            foreach (int cell in neighbourBitCells)
            {
                // If a chain was found
                if (chain.Any(tuple => tuple.cell == cell))
                {
                    chain.AddRange(thisConnection);

                    // Reset all ramps in this chain including current new connection
                    foreach ((int cell, GameObject bit, GameObject ramp) tuple in chain)
                    {
                        resetRamp(tuple.ramp);
                    }
                    theCellsHaveConnectionsAlready = true;
                    break;
                }
            }
            if (theCellsHaveConnectionsAlready) { break; }
        }

        // If there was no chain with any element of this connection found in it, add this connection as a new chain and reset the ramps
        if (!theCellsHaveConnectionsAlready)
        {
            GameManager.Instance.bitConnectionChains.Add(thisConnection.ToList());
            foreach ((int cell, GameObject bit, GameObject ramp) tuple in thisConnection)
            {
                resetRamp(tuple.ramp);
            }
        }

        if(thisConnection.Count > 0)
        {
            referenceRampScript = thisConnection[0].ramp.GetComponent<Ramp>();
        }


        print("-------All bit connections-------");
        foreach (List<(int cell, GameObject bit, GameObject ramp)> listOfCells in GameManager.Instance.bitConnectionChains)
        {
            print("Connection chain:");
            foreach ((int cell, GameObject bit, GameObject ramp) tuple in listOfCells)
            {
                print(tuple.cell);
            }
        }
        print("thisConnection.Count = " + thisConnection.Count);

    }



    private void resetRamp(GameObject ramp)
    {
        ramp.GetComponent<Ramp>().resetRamp();
    }

    private GameObject getRamp(GameObject bitParent)
    {
        foreach (Transform child in bitParent.transform)
        {
            if (child.tag == "BitTop")
            {
                foreach (Transform child2 in child.gameObject.transform)
                {
                    if (child2.tag == "Ramp")
                    {
                        return child2.gameObject;
                    }
                }
            }
        }
        return null;
    }

    // Detect clicking on the Gear
    private void OnMouseOver()
    {
        // Stop from modifying the gears already placed
        if (LevelManager.Instance.lockedCellCenters.Contains(gameObject.transform.position))
        {
            return;
        }

        // Mouse right-click to remove the object
        if (Input.GetMouseButtonDown(1))
        {
            GameManager.Instance.availableGearCellCenters.Add(gameObject.transform.position);
            removeConnection();
            GameManager.Instance.increaseCounter(GameManager.Instance.getObjTypeByTag(gameObject.tag));
            GameManager.Instance.allGears.Remove(gameObject);
            Debug.Log("<color=grey>Deleting the gear");
            Destroy(gameObject);
        }

    }


    void removeConnection()
    {
        if(neighbourBitCells.Count == 0)
        {
            return;
        }
        foreach (List<(int cell, GameObject bit, GameObject ramp)> chain in GameManager.Instance.bitConnectionChains)
        {
            if (chain.Any(chain => chain.cell == neighbourBitCells[0]))
            {
                foreach ((int cell, GameObject bit, GameObject ramp) tuple in thisConnection.ToList())
                {
                    print("Removing cell " + tuple.cell);
                    chain.Remove(tuple);
                }
                // If this connection chain is completely empty, remove it from the main list
                if(chain.Count == 0)
                {
                    GameManager.Instance.bitConnectionChains.Remove(chain);
                }
                break;
            }
        }

        print("-------All bit connections after deleting gear:-------");
        foreach (List<(int cell, GameObject bit, GameObject ramp)> chain in GameManager.Instance.bitConnectionChains)
        {
            print("Connection chain:");
            foreach ((int cell, GameObject bit, GameObject ramp) tuple in chain)
            {
                print(tuple.cell);
            }
        }
    }


    public List<int> getNeighbourBitCells()
    {
        return neighbourBitCells;
    }
}
