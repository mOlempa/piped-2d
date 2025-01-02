using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

public class Block : MonoBehaviour
{
    public bool isOn;

    Color highlightColor = new Color(0.8f, 0.6f, 1);

    [SerializeField]
    GameObject bitRamp;

    int cell = 0;

    private void Start()
    {
        isOn = false;

        if (gameObject.CompareTag("Bit"))
        {
            updateBitConnection();
        }
        cell = LevelManager.Instance.posToCellDictionary[gameObject.transform.position];
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

    // Detect the ball entering and leaving the block for ball's ignoreFall control
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.CompareTag("Bit"))
        {
            return;
        }
        if (other.gameObject.CompareTag("BlueBall") || other.gameObject.CompareTag("RedBall"))
        {
            //print("<color=green>Entering Trigger --> no damage");
            other.gameObject.GetComponent<BallScript>().ignoreFall = true;
            var script = other.gameObject.GetComponent<BallScript>();

            if (!script.blocksBallIsIn.Contains(cell))
            {
                script.blocksBallIsIn.Add(cell);
            }
            script.ignoreFall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (gameObject.CompareTag("Bit"))
        {
            return;
        }
        if (other.gameObject.CompareTag("BlueBall") || other.gameObject.CompareTag("RedBall"))
        {
            //print("<color=yellow>Exiting Trigger");
            var script = other.gameObject.GetComponent<BallScript>();

            if (script.blocksBallIsIn.Contains(cell))
            {
                script.blocksBallIsIn.Remove(cell);
                if (script.blocksBallIsIn.Count == 0)
                {
                    if (!script.isAtOutput)
                    {
                        //print("<color=orange>Ball can take damage again");
                        script.ignoreFall = false;
                    }
                }
            }
        }
    }

    private void OnMouseOver()
    {
        // Block player interaction with the blocks once simulation has started
        if (GameManager.Instance.marblesAreRunning)
        {
            return;
        }

        // Block spawning test marbles in a crossover since its colliders depend on triggers placed on their entrances
        if (gameObject.CompareTag("Crossover"))
        {
            GameManager.Instance.blockSpawningTestBalls = true;
        }

        if (LevelManager.Instance.lockedCellCenters.Contains(gameObject.transform.position))
        {
            return;
        }

        changeSpriteColor(highlightColor);

        // Mouse right-click to remove the object
        if (Input.GetMouseButtonDown(1))
        {

            GameManager.Instance.availableRegularCellCenters.Add(gameObject.transform.position);
            Destroy(gameObject);
            Debug.Log("<color=grey>Block deleted");
            if (gameObject.CompareTag("Bit"))
            {
                GameManager.Instance.deleteBitFromLists(cell);
                deleteBitFromConnectionChain();
            }
            GameManager.Instance.increaseCounter(GameManager.Instance.getObjTypeByTag(gameObject.tag));

            AudioManager.Instance.playRandomBlockPlaceSfx();
        }

        // Mouse left-click to mirror the object or replace with another
        if (Input.GetMouseButtonDown(0))
        {
            // Require holding left control to remove and replace (BoardScript function) (?) the object with mouse left-click
            if (Input.GetKey(KeyCode.LeftControl))
            {
                GameManager.Instance.availableRegularCellCenters.Add(gameObject.transform.position);
                Destroy(gameObject);
                Debug.Log("<color=grey>Block replaced");
                GameManager.Instance.increaseCounter(GameManager.Instance.getObjTypeByTag(gameObject.tag));
            }
            else
            {
                // Only mirror the object if it's of a Pass type
                if (gameObject.CompareTag("Pass"))
                {
                    int flipVar = -1;
                    gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * flipVar, 
                        gameObject.transform.localScale.y, 
                        gameObject.transform.localScale.z);

                    AudioManager.Instance.playSfx("BlockMirror");
                }

            }
        }

    }

    private void OnMouseExit()
    {
        changeSpriteColor(Color.white);

        if (GameManager.Instance.marblesAreRunning)
        {
            return;
        }
        // Block spawning test marbles in a crossover since its colliders depend on triggers placed on their entrances
        if (gameObject.CompareTag("Crossover"))
        {
            GameManager.Instance.blockSpawningTestBalls = false;
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
        if (gameObject.CompareTag("Pass") || gameObject.CompareTag("Stopper"))
        {
            gameObject.GetComponent<SpriteRenderer>().material.color = color;
        }
        if (gameObject.CompareTag("Crossover"))
        {
            foreach(SpriteRenderer renderer in gameObject.transform.GetComponentsInChildren<SpriteRenderer>(false))
            {
                renderer.material.color = color;
            }
        }
        if (gameObject.CompareTag("Bit"))
        {
            foreach (SpriteRenderer renderer in gameObject.transform.GetComponentsInChildren<SpriteRenderer>(false))
            {
                renderer.material.color = color;
            }
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
