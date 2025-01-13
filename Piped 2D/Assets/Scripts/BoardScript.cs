using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BoardScript : MonoBehaviour
{
    /**
    * A private GameObject field for referencing the shadow of the object to be spawned by the player.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    GameObject objToSpawnShadow;

    /**
    * A private GameObject field for referencing the object to be spawned by the player.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    GameObject objToSpawn;

    /**
    * A private GameObject field for referencing the Pass type block.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    GameObject pass;

    /**
    * A private GameObject field for referencing the Crossover type block.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField] 
    GameObject crossover;

    /**
    * A private GameObject field for referencing the Bit type block.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    GameObject bit;

    /**
    * A private Sprite field for referencing the whole sprite of the Bit.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    Sprite bitSprite;

    /**
    * A private GameObject field for referencing the Stopper type block.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    GameObject stopper;

    /**
    * A private GameObject field for referencing the Gear type block.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField] 
    GameObject gear;

    /**
    * A private GameObject field for referencing the blue marble.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    GameObject blueBall;

    /**
    * A private GameObject field for referencing the red marble.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    GameObject redBall;

    /**
    * A private GameObject field for referencing the spawner of the marbles.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    GameObject ballSpawn;

    /**
    * A public Dictionary field that stored information which and whether the marble spawning coroutine has already started
    */
    public Dictionary<Ball, bool> spawningCoroutineStarted = new Dictionary<Ball, bool>() { { Ball.Red, false },{ Ball.Blue, false } };

    /**
     * A private bool field holding information about the stop marble spawning coroutine request.
     */
    bool stopBallSpawnCoroutine = false;

    /**
     * A public void method that requests the stop of marble spawning coroutine
     */
    public void stopBallSpawning()
    {
        stopBallSpawnCoroutine = true;
    }

    /**
     * A private int array field that stores a copy of irregular cell numbers.
     */
    int[] irregularCells = LevelManager.Instance.irregularCells;
    /**
     * A private int array field that stores a copy of gear cell numbers.
     */
    int[] gearCells = LevelManager.Instance.gearsCells;

    /**
     * A private Vector2 variable that stores the size of half of the cell for finding center position.
     */
    Vector2 halfCellSize;

    /**
     * A private int variable that defines the amount of cells in a row.
     */
    const int xCellAmount = 11;

    /**
     * A private int variable that defines the amount of cells in a column.
     */
    const int yCellAmount = 11;

    /**
     * A private Vector2 array variable that stores the center positions of each regular cell.
     */
    Vector2[] regularCellCenters = new Vector2[xCellAmount * yCellAmount - LevelManager.Instance.irregularCells.Length];

    /**
    * A private Vector2 array variable that stores the center positions of each gear cell.
    */
    Vector2[] gearCellCenters = new Vector2[LevelManager.Instance.gearsCells.Length];

    /**
    * A private GameObject field for referencing the renderer of the shadow of the object to be spawned.
    */
    SpriteRenderer shadowRenderer;



    /**
    * A private void method called when the script instance is being loaded. It sets necessary variables.
    */
    void Start()
    {
        shadowRenderer = objToSpawnShadow.gameObject.GetComponent<SpriteRenderer>();

        Vector2[] cellCenters = new Vector2[xCellAmount * yCellAmount];

        //Get the board size
        Vector2 boardSize;
        Vector3 spriteRendererSize = GetComponent<SpriteRenderer>().bounds.size;
        boardSize = new Vector2(spriteRendererSize.x, spriteRendererSize.y);

        // Calculate board cell size
        Vector2 cellSize;
        cellSize = new Vector2(boardSize.x / xCellAmount, boardSize.y / yCellAmount);

        // Calculate half of the cell size for "click in" of the blocks when player places them
        halfCellSize = new Vector2(cellSize.x / 2, cellSize.y / 2);

        // Find top left coordinate of the board
        Vector2 boardTopLeftPos = new Vector2(transform.position.x - boardSize.x / 2, transform.position.y + boardSize.y / 2);

        // Check if cell dictionaries were filled before
        bool posToCellDictionaryFull = (LevelManager.Instance.posToCellDictionary.Count == 0) ? false : true;
        bool cellToPosDictionaryFull = (LevelManager.Instance.cellToPosDictionary.Count == 0) ? false : true;
        bool gearPosToCellDictionaryFull = (LevelManager.Instance.gearPosToCellDictionary.Count == 0) ? false : true;
        bool gearCellToPosDictionaryFull = (LevelManager.Instance.gearCellToPosDictionary.Count == 0) ? false : true;

        // A counter that will become an index of each regular cell
        int counter = 0;

        // A counter that will become an index of each gear cell
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


            // Setup of cell centers and conversion dictionaries:

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


    /**
     * A private void method that generates the board of the level.
     */
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

                    
                    if(entry.Key == BlockType.Bit)
                    {
                        // Add the newly generated Bit as a record to the currentBitStates dictionary in Game Manager
                        GameManager.Instance.currentBitStates.Add(LevelManager.Instance.posToCellDictionary[newObjectPos], 
                            (direction < 0) ? true : false);

                        // Handle setting state of the bit
                        setupGeneratedBit(newObj, direction);

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


        // Save locked cell centers and setup available cell centers for the player
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

    /**
     * A private void method that sets the Bits after the generation of the board.
     */
    void setupGeneratedBit(GameObject bit, int direction)
    {
        // Get script of the Bit's ramp
        Ramp script = bit.transform.GetChild(0).GetChild(0).GetComponent<Ramp>();

        // If direction is reversed, set Bit's ramp to ON, otherwise to OFF. Bit will react to the ramp and set itself
        if (direction == -1)
        {
            // Ramp has isOn = true by default, so in order to set the ramp, isOn needs to be changed
            script.isOn = false;
            script.setRamp();
        }
        else
        {
            script.resetRamp();
        }
    }


    /**
     * A public void method that calls a coroutine which spawns the marbles in the dispensers.
     */
    public void spawnBalls()
    {
        // Spawn balls specified for the level in coroutines to create "animation"
        foreach (Transform child in ballSpawn.transform)
        {
            if (child.CompareTag("BallSpawnLeft"))
            {
                if (spawningCoroutineStarted[Ball.Blue])
                {
                    return;
                }
                StartCoroutine(spawnBallsCoroutine(child.transform.position, Ball.Blue));
            }
            if (child.CompareTag("BallSpawnRight"))
            {
                if (spawningCoroutineStarted[Ball.Red])
                {
                    return;
                }
                StartCoroutine(spawnBallsCoroutine(child.transform.position, Ball.Red));
            }
        }
    }

    /**
     * A private coroutine that spawns the marbles in the dispensers.
     * @param spawnPos position of the marbles to be spawned at.
     * @param type the type of the marble.
     */
    IEnumerator spawnBallsCoroutine(Vector3 spawnPos, Ball type)
    {
        spawningCoroutineStarted[type] = true;

        int amount;

        // In case the ball amount was not specified for the level, treat it as 0 and do not spawn any
        if(!LevelManager.Instance.getLevelBallAmounts().TryGetValue(type, out amount))
        {
            //Debug.Log("<color=grey>No key for ball of type " + type + " found.");
            spawningCoroutineStarted[type] = false;
            yield break;
        }
        //print("<color=green>Starting spawning "+ type +" marbles");
        GameManager.Instance.setMarblesAreSpawning(true);
        //print("<color=white>Blocking spawning the marbles!!!!!!");

        GameObject ball = (type == Ball.Blue) ? blueBall : redBall;
        for (int i = 0; i < amount; i++)
        {
            if (stopBallSpawnCoroutine)
            {
                spawningCoroutineStarted[type] = false;
                stopBallSpawnCoroutine = false;
                GameManager.Instance.setMarblesAreSpawning(false);
                yield break;
            }
            GameObject newBall = Instantiate(ball, new Vector3(spawnPos.x, spawnPos.y, ball.transform.position.z), Quaternion.identity);
            GameManager.Instance.ballObjects.Add(newBall);
            yield return new WaitForSeconds(0.5f);
        }
        //print("<color=yellow>Finished spawning "+ type +" marbles");

        spawningCoroutineStarted[type] = false;
        GameManager.Instance.setMarblesAreSpawning(false);
        //print("<color=white>Unblocking spawning the marbles!!!!!!");
    }

    /**
     * A public void method that calls a coroutine which respawns the marbles in the dispensers.
     */
    public void respawnBalls()
    {
        // Spawn balls specified for the level in coroutines to create "animation"
        foreach (Transform child in ballSpawn.transform)
        {
            if (child.CompareTag("BallSpawnLeft"))
            {
                StartCoroutine(respawnReleasedBallsCoroutine(child.transform.position, Ball.Blue));
            }
            if (child.CompareTag("BallSpawnRight"))
            {
                StartCoroutine(respawnReleasedBallsCoroutine(child.transform.position, Ball.Red));
            }
        }
    }

    /**
     * A private coroutine that respawns the marbles in the dispensers.
     * @param spawnPos position of the marbles to be respawned at.
     * @param type the type of the marble.
     */
    IEnumerator respawnReleasedBallsCoroutine(Vector3 spawnPos, Ball type)
    {
        //print("<color=green>Starting coroutine");
        int amount = GameManager.Instance.ballAmountToRespawn[type];
        GameObject ball = (type == Ball.Blue) ? blueBall : redBall;

        if (spawningCoroutineStarted[type])
        {
            yield break;
        }
        GameManager.Instance.marblesAreSpawning = true;

        spawningCoroutineStarted[type] = true;
        print("<color=yellow>Spawning " + amount + " marbles of type " + type);
        //print("<color=white>Blocking spawning marbles");


        for (int i = 0; i < amount; i++)
        {
            if (stopBallSpawnCoroutine)
            {
                stopBallSpawnCoroutine = false;
                spawningCoroutineStarted[type] = false;
                GameManager.Instance.marblesAreSpawning = false;
                yield break;
            }
            GameObject newBall = Instantiate(ball, new Vector3(spawnPos.x, spawnPos.y, ball.transform.position.z), Quaternion.identity);
            GameManager.Instance.ballAmountToRespawn[type] -= 1;
            GameManager.Instance.ballObjects.Add(newBall);
            yield return new WaitForSeconds(0.5f);
        }
        
        GameManager.Instance.marblesAreSpawning = false;
        spawningCoroutineStarted[type] = false;
        //print("<color=white>Unblocking spawning marbles");

    }

    /**
    * A private void method called every frame.
    */
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

        // If block shadow is active (player has chosen a type of block to place)
        if (objToSpawnShadow.activeSelf)
        {
            // Make block shadow follow the cursor
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
                        if(objToSpawn != null)
                        {
                            placeBlock();
                        }
                    }
                }
            }
        }

        // Spawn test balls with Q key
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Block spawning test marbles in a crossover since its colliders depend on triggers placed on their entrances
            if (GameManager.Instance.blockSpawningTestBalls == false)
            {
                GameObject newTestBall = Instantiate(blueBall, new Vector3(mouseWorldPos.x, mouseWorldPos.y, 1), Quaternion.identity);
                GameManager.Instance.testBallObjects.Add( newTestBall );
            }
            
        }

        // Deactivate the shadow of the block following the cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            deactivateObjToSpawnShadow();
        }
    }

    /**
    * A private void method that handles placing the blocks on the board by the player.
    */
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

    /**
    * A public void method that sets the object to be spawned.
    * @param objID the number assigned to the block.
    */
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

    /**
    * A public void method that returns the object by the enum block type.
    * @param type of the block as BlockType enum
    */
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

    /**
     * A public void method that activated the shadow of the object to be spawned.
     */
    public void activateObjToSpawnShadow()
    {
        if (objToSpawnShadow != null)
        {
            objToSpawnShadow.gameObject.SetActive(true);
            GameManager.Instance.objToSpawnIsActive = true;
        }
    }

    /**
    * A public void method that deactivated the shadow of the object to be spawned.
    */
    public void deactivateObjToSpawnShadow()
    {
        objToSpawnShadow.gameObject.SetActive(false);
        objToSpawn = null;
        GameManager.Instance.objToSpawnIsActive = false;
    }

    /**
     * A private void method called when another object enters the trigger.
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("RedBall") || other.gameObject.CompareTag("BlueBall"))
        {
            other.gameObject.GetComponent<BallScript>().isAtOutput = true;
        }
    }


}
