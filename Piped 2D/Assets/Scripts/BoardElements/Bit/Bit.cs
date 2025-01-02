using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

public class Bit : MonoBehaviour
{
    Block block = new Block();
    SpriteRenderer[] spriteRenderers;

    [SerializeField]
    GameObject bitRamp;

    public bool isOn;
    int cell = 0;

    private void Start()
    {
        isOn = false;
        updateBitConnection();
        cell = LevelManager.Instance.posToCellDictionary[gameObject.transform.position];
        spriteRenderers = gameObject.transform.GetComponentsInChildren<SpriteRenderer>(false);
    }

    // Find and connect with any neighbouring Gears
    private void updateBitConnection(int ignoredBitCell = 0)
    {

        // On any Bit connection updates caused by a Bit, rewrite all the connection chains
        GameManager.Instance.bitConnectionChains.Clear();
        foreach(GameObject gear in GameManager.Instance.allGears)
        {
            gear.GetComponent<GearBlock>().detectAndConnectNeighbourBits(true, ignoredBitCell);
        }
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
            block.destroyBlock(gameObject);
        }

    }

    private void OnMouseEnter()
    {
        changeSpriteColor(block.highlightColor);
    }

    private void OnMouseExit()
    {
        changeSpriteColor(Color.white);
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
                updateBitConnection(cell);

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

    private void changeSpriteColor(Color color)
    {
        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.material.color = color;
        }

    }

    public void resetBit()
    {
        GameObject ramp = getRamp();
        if (ramp.CompareTag("Ramp"))
        {
            ramp.GetComponent<Ramp>().resetRamp();
            isOn = false;
        }
    }

    public void setBit()
    {
        GameObject ramp = getRamp();
        if (ramp.CompareTag("Ramp"))
        {
            ramp.GetComponent<Ramp>().setRamp();
            isOn = true;
        }
    }

    public GameObject getRamp()
    {
        if (bitRamp != null)
        {
            return bitRamp;
        }
        Debug.LogError("ERROR: No ramp found on this block!");
        return new GameObject();
    }




}
