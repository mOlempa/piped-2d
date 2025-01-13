using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * A class responsible for Gear block behaviour.
 */
public class GearBlock : MonoBehaviour
{
    /**
     * A private bool field informing whether the Gear is currently rotating or not.
     */
    bool isRotating = false;

    /**
     * A private RigidBody field for referencing the rigidbody of the Gear.
     */
    Rigidbody2D rb;

    /**
     * A public List of ints field that stores the numbers of closest cells
     */
    public List<int> neighbourBitCells = new List<int>();

    /**
     * A private List of tuple field that stores the cell numbers, Bit objects and ramp objects of the closest connection.
     */
    List<(int pos, GameObject bit, GameObject ramp)> thisConnection = new List<(int pos, GameObject bit, GameObject ramp)>();

    /**
     * A private Ramp field for referencing the ramp script.
     */
    Ramp referenceRampScript;

    /**
    * A private void method called when the script instance is being loaded. It sets necessary variables.
    */
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        detectAndConnectNeighbourBits(false);
        GameManager.Instance.allGears.Add(gameObject);
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 1);
    }

    /**
    * A private void method called every frame.
    */
    private void Update()
    {
        // Rotate the gear together with the ramps from the connection
        if (thisConnection.Count > 0)
        {
            if (referenceRampScript.isRotating)
            {
                StartCoroutine(rotateCoroutine());
            }
        }
    }

    /**
     * A private coroutine that rotates the Gear.
     */
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

    /**
     * A public void method that detects and connects the neighbouring Bit blocks
     * 
     * @param bool calledByNewBit used to find whether the method was called by the added Bit object or the Gear.
     * @param optional bool ignoredBitCell to be set if a Bit was deleted and the method is called by it.
     */
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


    /**
     * A private void method that resets the ramp of the Bit.
     * 
     * @param GameObject ramp object.
     */
    private void resetRamp(GameObject ramp)
    {
        ramp.GetComponent<Ramp>().resetRamp();
    }

    /**
     * A private void method that gets the ramp of the Bit.
     * 
     * @param GameObject the parent Bit.
     * @return GameObject ramp object.
     */
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

    /**
    * A private void method that is called when the mouse cursor is over the object.
    */
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

    /**
     * A private void method that removes the connection resulting from the Gear.
     */
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

    /**
     * A public method that returns the list of cell numbers that are neighbouring with the Gear.
     */
    public List<int> getNeighbourBitCells()
    {
        return neighbourBitCells;
    }
}
