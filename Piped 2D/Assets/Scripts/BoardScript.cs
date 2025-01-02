using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BoardScript : MonoBehaviour
{
    [SerializeField]
    GameObject objToSpawnShadow;

    [SerializeField]
    GameObject objToSpawn;

    [SerializeField]
    GameObject pass;
    
    [SerializeField] 
    GameObject crossover;

    [SerializeField]
    GameObject bit;

    [SerializeField]
    Sprite bitSprite;

    [SerializeField]
    GameObject stopper;

    [SerializeField] 
    GameObject gear;

    [SerializeField]
    GameObject blueBall;

    [SerializeField]
    GameObject redBall;

    [SerializeField]
    GameObject ballSpawn;


    public bool spawnBlueBallsCoroutineStarted;
    public bool spawnRedBallsCoroutineStarted;


    int[] irregularCells = LevelManager.Instance.irregularCells;
    int[] gearCells = LevelManager.Instance.gearsCells;

    Vector2 halfCellSize;
    const int xCellAmount = 11;
    const int yCellAmount = 11;
    
    Vector2[] regularCellCenters = new Vector2[xCellAmount * yCellAmount - LevelManager.Instance.irregularCells.Length];
    Vector2[] gearCellCenters = new Vector2[LevelManager.Instance.gearsCells.Length];
    //List<Vector2> availableCellCenters = new List<Vector2>();

    private SpriteRenderer shadowRenderer;



    void Start()
    {
        shadowRenderer = objToSpawnShadow.gameObject.GetComponent<SpriteRenderer>();

        Vector2[] cellCenters = new Vector2[xCellAmount * yCellAmount];

        //Get the board size
        Vector2 boardSize;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        boardSize = new Vector2(spriteRenderer.bounds.size.x, spriteRenderer.bounds.size.y);

        // Get cell size and half of the cell size
        Vector2 cellSize;
        cellSize = new Vector2(boardSize.x / xCellAmount, boardSize.y / yCellAmount);
        halfCellSize = new Vector2(cellSize.x / 2, cellSize.y / 2);

        // Find top left coordinate of the board
        Vector2 boardTopLeftPos = new Vector2(transform.position.x - boardSize.x / 2, transform.position.y + boardSize.y / 2);

        // Check if cell dictionaries were filled before
        bool posToCellDictionaryFull = (LevelManager.Instance.posToCellDictionary.Count == 0) ? false : true;
        bool cellToPosDictionaryFull = (LevelManager.Instance.cellToPosDictionary.Count == 0) ? false : true;
        bool gearPosToCellDictionaryFull = (LevelManager.Instance.gearPosToCellDictionary.Count == 0) ? false : true;
        bool gearCellToPosDictionaryFull = (LevelManager.Instance.gearCellToPosDictionary.Count == 0) ? false : true;

        // A counter that will become a number of each regular cell
        int counter = 0;

        // A counter that will become a number of each gear cell
        int gCounter = 0;

        // CALCULATING CELL CENTERS FOR FUTURE POSITIONS OF OBJECTS
        // Go through each cell
        for (int i = 0; i < xCellAmount * yCellAmount; i++)
        {
            // Find relative position of the cell (like in chess)
            int xCellNumber = i % xCellAmount;
            int yCellNumber = i / xCellAmount;

            // Calculate world coordinates of each cell center
            cellCenters[i] = new Vector2((boardTopLeftPos.x + xCellNumber * cellSize.x) + cellSize.x / 2, 
                (boardTopLeftPos.y - yCellNumber * cellSize.y) - cellSize.y / 2);

            // If the cell is a regular cell where you can place blocks
            if (!irregularCells.Contains(i))
            {
                // Add the positions where blocks can be placed to the empty-board-available cells' positions array
                regularCellCenters[counter] = cellCenters[i];

                // If cell dictionary hasn't been filled yet
                if (!posToCellDictionaryFull && !cellToPosDictionaryFull)
                {
                    // Add the positions where blocks can be placed with the number of the cell as a key
                    LevelManager.Instance.posToCellDictionary.Add(cellCenters[i], counter + 1);

                    // Same as above but reversed key and value
                    LevelManager.Instance.cellToPosDictionary.Add(counter + 1, cellCenters[i]);
                }

                // Increase counter which will be the number of the next cell
                counter++;
            }
            else
            {
                // If the cell is an irregular cell but it is a gear cell
                if (gearCells.Contains(i))
                {
                    gearCellCenters[gCounter] = cellCenters[i];

                    if(!gearPosToCellDictionaryFull && !gearCellToPosDictionaryFull)
                    {
                        LevelManager.Instance.gearPosToCellDictionary.Add(cellCenters[i], gCounter + 1);
                        LevelManager.Instance.gearCellToPosDictionary.Add(gCounter + 1, cellCenters[i]);
                    }
                    gCounter++;
                }
            }
        }

        // GENERATING STARTING BOARD
        generateStartingBoard();
    }



    void generateStartingBoard()
    {
        GameManager.Instance.clearAllLevelLists();
        Dictionary<BlockType, int[]> board = LevelManager.Instance.getLevelStartingBoard();
        List<Vector2> lockedCellCenters = new List<Vector2>();

        // SETTING UP THE STARTING BOARD
        // For each block type in the board setup...
        foreach (KeyValuePair<BlockType, int[]> entry in board)
        {
            // ...get an array of cells this block is supposed to be set up in.
            int[] cells = entry.Value;

            // For each cell
            for (int i = 0; i < cells.Length; i++)
            {
                // If it's not a cell numbered 0 (such does not exist, they are numbered from 1 up and -1 down for mirrored blocks)
                if (cells[i] != 0)
                {
                    // HANDLING BLOCK INSTANTIATION
                    // Check whether the block type is a gear type and set stuff accordingly
                    Vector2 newObjectPos = new Vector2();
                    if (entry.Key == BlockType.Gear)
                    {
                        // HERE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                        // Add the position of cells where blocks are generated to locked cells positions
                        lockedCellCenters.Add(gearCellCenters[Math.Abs(cells[i]) - 1]);

                        // Define a position for the new block to be generated in
                        newObjectPos = gearCellCenters[Math.Abs(cells[i]) - 1];
                    }
                    else
                    {
                        // Add the position of cells where blocks are generated to locked cells positions
                        lockedCellCenters.Add(regularCellCenters[Math.Abs(cells[i]) - 1]);

                        // Define a position for the new block to be generated in
                        newObjectPos = regularCellCenters[Math.Abs(cells[i]) - 1];
                    }

                    // Instantiate a new game object as the type of block from the main loop at position defined earlier
                    GameObject newObj = Instantiate(getObjectByBlockType(entry.Key), newObjectPos, Quaternion.identity);


                    // HANDLING BLOCK DIRECTION (defined in LevelManager in starting boards)
                    int direction = (Math.Sign(cells[i]) < 0) ? -1 : 1;

                    // If it is a Bit and direction is reversed, add force to the ramp instead of mirroring
                    if(entry.Key == BlockType.Bit)
                    {
                        // Add the newly generated Bit as a record to the currentBitStates dictionary in Game Manager
                        GameManager.Instance.currentBitStates.Add(LevelManager.Instance.posToCellDictionary[newObjectPos], 
                            (direction < 0) ? true : false);

                        if (direction == -1)
                        {
                            print("Setting the ramp");
                            Ramp script = newObj.transform.GetChild(0).GetChild(0).GetComponent<Ramp>();
                            script.isOn = false;
                            script.setRamp();
                        }
                        else
                        {
                            print("Resetting the ramp");
                            newObj.transform.GetChild(0).GetChild(0).GetComponent<Ramp>().resetRamp();
                        }

                        // Add this bit to the bit objects list in Game Manager
                        GameManager.Instance.bitObjects.Add(LevelManager.Instance.posToCellDictionary[newObjectPos], newObj);
                    }

                    // If it is a Pass, set transform according to direction (mirror if -1)
                    if (entry.Key == BlockType.Pass)
                    {
                        newObj.transform.localScale = new Vector3(newObj.transform.localScale.x*direction, newObj.transform.localScale.y, 1);
                    }
                    // No other block types can be mirrored!
                }
            }
        }

        LevelManager.Instance.lockedCellCenters = lockedCellCenters;

        for (int i = 0; i < regularCellCenters.Length; i++)
        {
            if (!lockedCellCenters.Contains(regularCellCenters[i]))
            {
                GameManager.Instance.availableRegularCellCenters.Add(regularCellCenters[i]);
            }
        }

        for (int i = 0; i < gearCellCenters.Length; i++)
        {
            if (!lockedCellCenters.Contains(gearCellCenters[i]))
            {
                GameManager.Instance.availableGearCellCenters.Add(gearCellCenters[i]);
            }
        }

        spawnBalls();
    }

    // Spawning balls
    public void spawnBalls()
    {
        // Spawn balls specified for the level in coroutines to create "animation"
        foreach (Transform child in ballSpawn.transform)
        {
            if (child.CompareTag("BallSpawnLeft"))
            {
                if (spawnBlueBallsCoroutineStarted)
                {
                    return;
                }
                StartCoroutine(spawnBallsCoroutine(child.transform.position, Ball.Blue));
            }
            if (child.CompareTag("BallSpawnRight"))
            {
                if (spawnRedBallsCoroutineStarted)
                {
                    return;
                }
                StartCoroutine(spawnBallsCoroutine(child.transform.position, Ball.Red));
            }
        }
    }

    IEnumerator spawnBallsCoroutine(Vector3 spawnPos, Ball type)
    {
        if(type == Ball.Red)
        {
            spawnRedBallsCoroutineStarted = true;
        }
        if (type == Ball.Blue)
        {
            spawnBlueBallsCoroutineStarted = true;
        }

        int amount;

        // In case the ball amount was not specified for the level, treat it as 0 and do not spawn any
        if(!LevelManager.Instance.getLevelBallAmounts().TryGetValue(type, out amount))
        {
            Debug.Log("<color=grey>No key for ball of type " + type + " found.");
            if (type == Ball.Red)
            {
                spawnRedBallsCoroutineStarted = false;
            }
            if (type == Ball.Blue)
            {
                spawnBlueBallsCoroutineStarted = false;
            }
            yield break;
        }

        GameObject ball = (type == Ball.Blue) ? blueBall : redBall;
        for (int i = 0; i < amount; i++)
        {
            GameObject newBall = Instantiate(ball, new Vector3(spawnPos.x, spawnPos.y, ball.transform.position.z), Quaternion.identity);
            GameManager.Instance.ballObjects.Add(newBall);
            yield return new WaitForSeconds(0.5f);
        }

        if (type == Ball.Red)
        {
            spawnRedBallsCoroutineStarted = false;
        }
        if (type == Ball.Blue)
        {
            spawnBlueBallsCoroutineStarted = false;
        }
    }




    void Update()
    {
        // Stop any input on the board from the player once the simulation is running
        if (GameManager.Instance.marblesAreRunning)
        {
            if(GameManager.Instance.objToSpawnIsActive) {
                deactivateObjToSpawnShadow();
            }


            return;
        }

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        objToSpawnShadow.transform.position = mouseWorldPos;

        // Set available cells for the player to place blocks depending on the type of block they want to place
        List<Vector2> availableCellCenters = (objToSpawn == gear) ? 
            GameManager.Instance.availableGearCellCenters : GameManager.Instance.availableRegularCellCenters;

        for (int i = 0; i < availableCellCenters.Count; i++)
        {
            // If block is near an available cell, put it in the center of it
            if (Vector2.Distance(mouseWorldPos, availableCellCenters[i]) < halfCellSize.x)
            {
                objToSpawnShadow.transform.position = availableCellCenters[i];

                // PLACING BLOCKS - place blocks if left mouse button clicked
                if (Input.GetMouseButtonDown(0))
                {
                    if(objToSpawn != null && objToSpawnShadow.activeSelf)
                    {
                        placeBlock();
                    }
                }
            }
        }

        // Spawn balls with Q key (for testing)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Block spawning test marbles in a crossover since its colliders depend on triggers placed on their entrances
            if (GameManager.Instance.blockSpawningTestBalls == false)
            {
                GameObject newTestBall = Instantiate(blueBall, new Vector3(mouseWorldPos.x, mouseWorldPos.y, 1), Quaternion.identity);
                GameManager.Instance.testBallObjects.Add( newTestBall );
            }
            
        }

        //
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            deactivateObjToSpawnShadow();
        }


        //checkForNewBitConnections();

    }

    void placeBlock()
    {
        // If counter of the chosen block is bigger than 0
        if (GameManager.Instance.getBlocktypeCounter(GameManager.Instance.getObjTypeByTag(objToSpawn.tag)) > 0)
        {
            // Spawn object where the object shadow currently is
            GameObject newObj = Instantiate(objToSpawn, objToSpawnShadow.transform.position, Quaternion.identity);

            AudioManager.Instance.playRandomBlockPlaceSfx();

            if(newObj.CompareTag("Bit"))
            {
                newObj.transform.GetChild(0).GetChild(0).GetComponent<Ramp>().resetRamp();

                // Add this bit to Game Manager dictionaries to manage it in simulation (in case of resetting the case setup)
                int cell = LevelManager.Instance.posToCellDictionary[newObj.transform.position];
                GameManager.Instance.currentBitStates.Add(cell, false);
                GameManager.Instance.playerBitCustomSetup.Add(cell, false);
                GameManager.Instance.bitObjects.Add(cell, newObj);
            }

            // Remove the cell coordinates from available ones
            if (newObj.CompareTag("Gear"))
            {
                GameManager.Instance.availableGearCellCenters.Remove(objToSpawnShadow.transform.position);
            }
            else
            {
                GameManager.Instance.availableRegularCellCenters.Remove(objToSpawnShadow.transform.position);
            }

            // Decrease the counter for this block type
            GameManager.Instance.decreaseCounter(GameManager.Instance.getObjTypeByTag(objToSpawn.tag));
        }
        else
        {
            Debug.Log("<color=yellow>Cannot place object - counter is 0</color>");
        }
        
    }

    public void setSpawnObject(int objID)
    {
        switch (objID)
        {
            case 1:
                objToSpawn = pass;
                break;
            case 2:
                objToSpawn = crossover;
                break;
            case 3:
                objToSpawn = bit;
                break;
            case 4:
                objToSpawn = stopper;
                break;
            case 5:
                objToSpawn = gear;
                break;
            default:
                objToSpawn = null;
                break;
        }
        if(objToSpawn == bit)
        {
            shadowRenderer.sprite = bitSprite;
        }
        else
        {
            shadowRenderer.sprite = objToSpawn.GetComponent<SpriteRenderer>().sprite;
        }
        shadowRenderer.color = new Color(1, 1, 1, 0.5f);
        activateObjToSpawnShadow();
    }

    private GameObject getObjectByBlockType(BlockType type)
    {
        if (type == BlockType.Crossover)
        {
            return crossover;
        }
        if (type == BlockType.Pass)
        {
            return pass;
        }
        if (type == BlockType.Bit)
        {
            return bit;
        }
        if (type == BlockType.Stopper)
        {
            return stopper;
        }
        if(type == BlockType.Gear)
        {
            return gear;
        }
        else
        {
            Debug.LogError("getObjectByBlockType: Returning empty GameObject");
            return new GameObject();
        }
    }

    public void activateObjToSpawnShadow()
    {
        if (objToSpawnShadow != null)
        {
            objToSpawnShadow.gameObject.SetActive(true);
            GameManager.Instance.objToSpawnIsActive = true;
        }
    }

    public void deactivateObjToSpawnShadow()
    {
        objToSpawnShadow.gameObject.SetActive(false);
        objToSpawn = null;
        GameManager.Instance.objToSpawnIsActive = false;
    }


    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("RedBall") || other.gameObject.CompareTag("BlueBall"))
        {
            other.gameObject.GetComponent<BallScript>().isAtOutput = true;
        }
    }


}
