using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * An enum that describes the possible speeds of the game speed.
 */
public enum GameSpeed
{
    Regular,
    Fast,
    Slow
}

public enum Language
{
    English,
    Polski
}

/**
 * A class handling communication between other scripts and multiple events behind the game.
 */
public class GameManager : MonoBehaviour
{
    public Language currentLanguage = Language.English;

    /**
     * A public bool field informing whether the Level Complete text should be displayed.
     */
    public bool showLevelCompleteText = false;

    /**
     * A public Dictionary field for storing the copy of available blocks for the level.
     */
    public Dictionary<BlockType, int> availableBlocksCopy;

    /**
     * A public bool field informing whether to update the counter text displayed next to block buttons.
     */
    public bool updateCounterText { get; set; } = false;

    /**
     * A public bool field informing whether the level has started or not.
     */
    public bool levelStarted = true;

    /**
     * A public Ball field informing which dispenser is set to start.
     */
    public Ball startingBallType = Ball.Blue;

    /**
     * A public bool field informing whether to rotate the left dispenser wheel.
     */
    public bool turnLeftWheels { get; set; } = false;

    /**
     * A public bool field informing whether to rotate the right dispenser wheel.
     */
    public bool turnRightWheels { get; set; } = false;

    /**
     * A public Level field informing which level is currently opened.
     */
    public Level currentLevel = Level.Level1;

    /**
     * A public GameSpeed field informing what is the current speed of the game.
     */
    public GameSpeed currentGameSpeed = GameSpeed.Regular;

    /**
     * A private GameUI field for referencing the Game UI script.
     */
    private GameUI gameUI;

    /**
    * A public bool field informing whether the object to be spawned is active or not.
    */
    public bool objToSpawnIsActive = false;

    /**
    * A public bool field informing whether the game should block spawning test marbles.
    */
    public bool blockSpawningTestBalls = false;


    /**
    * A public List of Vector2 for storing the cell center positions of the available regular cells
    */
    public List<Vector2> availableRegularCellCenters = new List<Vector2>();

    /**
    * A public List of Vector2 for storing the cell center positions of the available gear cells
    */
    public List<Vector2> availableGearCellCenters = new List<Vector2>();

    /**
    * A public Dictionary for storing the current states of the Bits on the board.
    */
    public Dictionary<int, bool> currentBitStates = new Dictionary<int, bool>();

    /**
    * A public Dictionary for storing the Bit objects from the board along with their cell number.
    */
    public Dictionary<int, GameObject> bitObjects = new Dictionary<int, GameObject>();

    /**
    * A public List for storing the marble objects from the board.
    */
    public List<GameObject> ballObjects = new List<GameObject>();

    /**
   * A public List for storing the test marble objects from the board.
   */
    public List<GameObject> testBallObjects = new List<GameObject>();

    /**
   * A public List for storing the current output of the board.
   */
    public List<Ball> currentBallOutput = new List<Ball>();

    /**
   * A public Dictionary for storing the current content of the Stoppers from the board.
   */
    public Dictionary<int, Ball> currentStopperContents = new Dictionary<int, Ball>();

    /**
    * A public List of tuples for storing the Bit connection nets from the board.
    */
    public List<List<(int cell, GameObject bit, GameObject ramp)>> bitConnectionChains = new List<List<(int, GameObject, GameObject)>>();

    /**
    * A public List for storing the Gear objects from the board.
    */
    public List<GameObject> allGears = new List<GameObject>();

    /**
     * A public bool field informing whether the simulation is running or not.
     */
    public bool marblesAreRunning { get; set; } = false;

    /**
    * A public List for storing the current solution cases of the level,
    */
    public List<SolutionCase> currentSolutionCases = new List<SolutionCase>();

    /**
    * A public SolutionCase field for storing the currently being checked solution case.
    */
    public SolutionCase currentSolutionCase = null;

    /**
    * A public Dictionary for storing the setup of the Bits that was dependent on the player, for possible restoring later.
    */
    public Dictionary<int, bool> playerBitCustomSetup = new Dictionary<int, bool>();


    /**
    * A public bool field informing whether the level was completed or not.
    */
    public bool levelCompleted = false;

    /**
    * A public bool field informing whether the current solution case was finished or not.
    */
    public bool caseFinished = false;

    /**
    * A public bool field for requesting stopping the simulation.
    */
    public bool stopSimulation = false;

    /**
    * A public bool field informing whether the next level is being loaded.
    */
    public bool loadingNextLevel = false;

    /**
    * A public bool field informing whether the level is being loaded after the plot.
    */
    public bool loadingLevelAfterPlot = false;

    /**
    * A public bool field informing whether the level is being reloaded.
    */
    public bool reloadingLevel = false;

    /**
    * A public bool field informing whether the current level was a final level of the game.
    */
    public bool wasLastLevel = false;

    /**
    * A public bool field informing whether the game was finished.
    */
    public bool finishedGame = false;

    /**
    * A public bool field informing whether the marbles are respawning
    */
    public bool marblesAreSpawning;

    /**
    * A public Dictionary for storing the amount of marbles to be respawned along with their type.
    */
    public Dictionary<Ball, int> ballAmountToRespawn = new Dictionary<Ball, int>() { { Ball.Blue, 0 }, { Ball.Red, 0 } };

    /**
    * A public List storing the marble objects to be respawned
    */
    public List<GameObject> ballsToRespawn = new List<GameObject>();

    /**
    * A public void method requesting spawning the marbles.
    */
    public void setMarblesAreSpawning(bool value)
    {
        marblesAreSpawning = value;
    }

    /**
     * A private instance of the Game Manager singleton.
     */
    private static GameManager _instance;

    /**
     * A public reference to the instance of the Game Manager singleton.
     */
    public static GameManager Instance
    {
        get
        {
            if( _instance is null)
            {
                Debug.LogError("Game Manager is NULL");
            }
            return _instance;
        }
    }

    /**
     * A public void method called on awake of the script.
     */
    public void Awake()
    {
        _instance = this;
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    /**
     * A public void method that loads loading screen.
     */
    public void LoadLoadingScreen()
    {
        LoadScene(3);
    }

    /**
     * A public void method that loads the level
     */
    public void LoadLevel()
    {
        marblesAreSpawning = true;
        SceneManager.LoadScene(2);
    }

    /**
     * A public void method that reloads the level
     */
    public void ReloadLevel()
    {
        reloadingLevel = true;
        resetSimulation();
        clearAllLevelLists();
        SceneManager.LoadScene(3);
    }

    /**
     * A public void method that loads the next level
     */
    public void LoadNextLevel()
    {
        // If the current level was the last level, load the plot scene
        if(currentLevel == Enum.GetValues(typeof(Level)).Cast<Level>().Last())
        {
            wasLastLevel = true;
        }
        else
        {
            loadingNextLevel = true;
        }
        resetSimulation();
        clearAllLevelLists();
        SceneManager.LoadScene(3);
    }

    /**
     * A public void method that loads the Level Selection Menu
     */
    public void LoadLevelSelection() {
        clearAllLevelLists();
        resetSimulation();
        SceneManager.LoadScene(1);
        AudioManager.Instance.playMenuMusic();
    }

    /**
     * A public void method that loads the Main Menu
     */
    public void LoadMainMenu()
    {
        clearAllLevelLists();
        resetSimulation();
        SceneManager.LoadScene(0);
        AudioManager.Instance.playMenuMusic();
    }

    /**
     * A public void method that loads the Credits scene
     */
    public void LoadCredits()
    {
        SceneManager.LoadScene(5);
    }

    /**
     * A public void method that loads a scene by its number.
     * @param sceneId number of the scene.
     */
    public void LoadScene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }

    /**
     * A private void method called on object enable.
     */
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    /**
     * A private void method called on object disable.
     */
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    /**
     * A private void method called on loading a scene.
     * @param scene scene to be loaded.
     * @param mode of loading the scene.
     */
    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        print("<color=white>Loading scene...");
        if (scene.name == "Level")
        {
            availableBlocksCopy = LevelManager.Instance.getLevelBlocks();

            GameObject gameUIObject = GameObject.FindGameObjectWithTag("GameUI");
            gameUI = gameUIObject.GetComponent<GameUI>();
            if (gameUI == null)
            {
                Debug.LogError("ERROR: GameUI script not found!!!");
                return;
            }
            gameUI.setAvailableButtons();

            startingBallType = LevelManager.Instance.getLevelStartingSide();


            // Get current set of solutions for the current level
            currentSolutionCases = LevelManager.Instance.getLevelSolutionCases();
            print("OnSceneLoad: currentSolutionCases.Count = " + currentSolutionCases.Count);
        }
        if (scene.name == "LevelLoading")
        {
            if (loadingNextLevel)
            {
                currentLevel = currentLevel + 1;
                print("Setting level to " + currentLevel);
                loadingNextLevel = false;
            }
            StartCoroutine(loadingScreen());
        }
    }

    /**
     * A coroutine handling the loading screen.
     */
    IEnumerator loadingScreen()
    {
        yield return new WaitForSeconds(1);

        if (finishedGame)
        {
            finishedGame = false;
            LoadCredits();
            yield break;
        }

        // Load plot scene if the level has a plot
        if (LevelManager.Instance.levelsWithPlot.Contains(currentLevel) && !loadingLevelAfterPlot)
        {
            if (reloadingLevel)
            {
                reloadingLevel = false;
                print("Reloading the level");
                LoadLevel();
            }
            else
            {
                // If it was not the last level, load next level after loading the plot. If it was, set level to 0 (last level)
                if (wasLastLevel)
                {
                    finishedGame = true;
                }
                else
                {
                    loadingLevelAfterPlot = true;
                }
                // Load plot scene
                LoadScene(4);
            }
        }
        else
        {
            if (loadingLevelAfterPlot)
            {
                loadingLevelAfterPlot = false;
            }
            if (reloadingLevel)
            {
                reloadingLevel = false;
                print("Reloading the level");
            }
            LoadLevel();
        }
    }

    /**
     * A public void method clearing all lists of objects and other information stored for the current level.
     */
    public void clearAllLevelLists()
    {
        availableRegularCellCenters.Clear();
        availableGearCellCenters.Clear();


        currentBitStates.Clear();
        bitObjects.Clear();

        ballObjects.Clear();
        testBallObjects.Clear();
        ballAmountToRespawn[Ball.Blue] = 0;
        ballAmountToRespawn[Ball.Red] = 0;
        currentBallOutput.Clear();
        currentStopperContents.Clear();

        bitConnectionChains.Clear();
        allGears.Clear();
        playerBitCustomSetup.Clear();
    }

    /**
     * A public void method decreasing the counter of the given type of block.
     * @param blockType the type of the block in form of the BlockType enum.
     */
    public void decreaseCounter(BlockType blockType)
    {
        if (availableBlocksCopy[blockType] > 0)
        {
            availableBlocksCopy[blockType] = availableBlocksCopy[blockType] - 1;
            if(gameUI != null)
            {
                gameUI.updateAvailableBlockButtons();
            }
        }
        else
        {
            Debug.LogError("ERROR: Cannot decrease the counter - it reached 0!");
        }

    }

    /**
     * A public void method increasing the counter of the given type of block.
     * @param blockType the type of the block in form of the BlockType enum.
     */
    public void increaseCounter(BlockType blockType)
    {
        availableBlocksCopy[blockType] = availableBlocksCopy[blockType] + 1;
        if(gameUI != null)
        {
            gameUI.updateAvailableBlockButtons();
        }
    }

    /**
     * A public BlockType method returning the type of the block by its tag.
     * @param tag the tag of the block.
     */
    public BlockType getObjTypeByTag(string tag)
    {
        switch (tag)
        {
            case "Pass":
                return BlockType.Pass;
            case "Crossover":
                return BlockType.Crossover;
            case "Bit":
                return BlockType.Bit;
            case "Stopper":
                return BlockType.Stopper;
            case "Gear":
                return BlockType.Gear;
            default:
                Debug.LogError("ERROR: Unregistered object or tag, returning pass type");
                return BlockType.Pass;
        }
    }

    /**
     * A public void method returning the counter of the block of a given type.
     * @param blockType the type of the block in form of the BlockType enum.
     * @return int the counter of the given type of block.
     */
    public int getBlocktypeCounter(BlockType blockType)
    {
        return availableBlocksCopy[blockType];
    }

    /**
     * A public void method handling adding marbles to the current output
     * @param Ball the type of the marble in the form of Ball enum.
     */
    public void addBallToCurrentBallOutput(Ball ball)
    {
        currentBallOutput.Add(ball);
        //print("Adding ball " + ball + " to output");

        if(currentSolutionCase != null) { 
            if(currentBallOutput.Count == currentSolutionCase.ballOutput.Count)
            {
                // If there is no waiting for Stopper solution necessary, otherwise do caseFinished in Stopper checking
                if(currentSolutionCase.stopperContents.Count == 0)
                {
                    caseFinished = true;
                }
            }
        }
    }


    /**
     * A public void method changing the state of the Bit in the list of current Bit states.
     * @param cellNum the number of the cell of the Bit.
     * @param state the true/false state of the Bit.
     */
    public void changeBitStateVar(int cellNum, bool state)
    {
        if(currentBitStates.ContainsKey(cellNum))
        {
            if (currentBitStates[cellNum] != state)
            {
                currentBitStates[cellNum] = state;

            }
        }
    }


    /**
     * A private coroutine simulating different cases given for the current level.
     */
    IEnumerator simulateCasesCheck()
    {
        List<bool> solutionStates = new List<bool>();
        // If this solution has any bits that have to be in a required given state (same in each case, so we take just first one)
        if (currentSolutionCases[0].bitRequiredSetup.Count > 0)
        {
            setResetBits(bitObjects, currentSolutionCases[0].bitRequiredSetup);
        }
        // For each case of solution
        foreach (SolutionCase solutionCase in currentSolutionCases)
        {
            print("<color=#3CB371>Running a case");

            // Set current solution case to compare results in other functions
            currentSolutionCase = solutionCase;

            // CLEANING UP (AFTER CHECKING PREVIOUS SOLUTION CASE)

            if (solutionCase.resetBallsAfterCase)
            {
                print("balls run count: " + ballsToRespawn.Count);
                resetBalls();

                // If balls are reset, it means this case will have a different ball output that needs to be reset as well
                currentBallOutput.Clear();

                // If balls are reset, then player custom Bit setup from the beginning of simulation needs to be back
                restorePlayerBitSetup();
            }

            if (solutionCase.stopperContents.Count > 0)
            {
                // Clear current stopper contents from the previous case simulation
                currentStopperContents.Clear();
            }



            // SETUP BEFORE CHECKING THIS SOLUTION CASE

            // If this solution has a bit setup (which are different for each case)
            if (solutionCase.bitSetupCase.Count > 0)
            {
                setResetBits(bitObjects, solutionCase.bitSetupCase);
            }

            // Add records to the current stopper contents dictionary if these exist in the solution case.
            // It will be updated once a ball reaches a stopper during a check.
            foreach(KeyValuePair<int, Ball> content in solutionCase.stopperContents)
            {
                currentStopperContents.Add(content.Key, Ball.Neither);
            }

            // If the marbles will be reset, wait a moment for all the balls to spawn before turning wheels
            if (solutionCase.resetBallsAfterCase)
            {
                yield return new WaitForSeconds(1.5f);
            }

            if (stopSimulation)
            {
                print("<color=orange>Simulation stopped.");
                stopSimulation = false;
                yield break;
            }

            if(solutionCase.startingSideCase != Ball.Neither)
            {
                startingBallType = solutionCase.startingSideCase;
            }

            if (startingBallType == Ball.Blue)
            {
                //print("<color=yellow>Setting turnWheels to true");
                turnLeftWheels = true;
            }
            else
            {
                turnRightWheels = true;
            }



            // WAITING FOR THIS CASE SIMULATION TO END

            // Wait for this case to finish running (e.g. for the ball to finish rolling)
            while (!caseFinished && !stopSimulation)
            {
                yield return null;
            }
            caseFinished = false;
            print("<color=#3CB371>Finished the case.");


            // CHECKING THIS SOLUTION CASE

            solutionStates.Add(checkCaseSolution(solutionCase));
            print("<color=#3CB371>Finished checking the case.");

            // If the player wants to stop the simulation, leave the coroutine
            if (stopSimulation)
            {
                print("<color=orange>Simulation stopped.");
                stopSimulation = false;
                yield break;
            }

            // If the marbles will be reset
            if (solutionCase.resetBallsAfterCase)
            {
                // Give the player a moment to see the result before jumping to the next case
                yield return new WaitForSeconds(0.5f);
            }
        }


        // AFTER ALL CASES WERE CHECKED

        resetGameSpeed();

        // If all cases were solved correctly, show feedback
        bool solvedAllCases = true;
        foreach(bool state in solutionStates)
        {
            if (!state)
            {
                solvedAllCases = false;
            }
        }
        if (solvedAllCases)
        {
            setLevelComplete();
        }
    }

    /**
     * A public bool method checking the simulation results.
     * @param solutionCase the solution case object with correct results assigned for the current level.
     * @return bool value of the check result.
     */
    public bool checkCaseSolution(SolutionCase solutionCase)
    {
        bool solved = true;

        // If this solution involves stopper content
        if (solutionCase.stopperContents.Count > 0)
        {
            foreach(KeyValuePair<int, Ball> b in currentStopperContents)
            {
                int key = b.Key;
                // If there is a stopper content solution that didn't require the stopper to be at a certain position (so written with cell 0)
                if (solutionCase.stopperContents.ContainsKey(0))
                {
                    key = 0;
                }
                if (solutionCase.stopperContents[key] != b.Value)
                {
                    print("<color=red>Wrong stopper content");
                    print("   Current content: " + b.Value + " in " + b.Key);
                    print("   Correct content: " + solutionCase.stopperContents[b.Key] + " in " + b.Key);
                    solved = false;
                }
                else
                {
                    print("<color=green>Correct stopper content");
                }
            }
        }

        // If this solution involves final states of the bits
        if (solutionCase.bitFinalStates.Count > 0)
        {
            foreach(KeyValuePair<int, bool> state in solutionCase.bitFinalStates)
            {
                if (currentBitStates[state.Key] != state.Value)
                {
                    print("<color=red>Wrong bit states");
                    print("   Current bit state: " + currentBitStates[state.Key] + " at sequence index " + state.Key);
                    print("   Correct bit state: " + state.Value + " at sequence index " + state.Key);
                    solved = false;
                }
            }
        }

        // If this solution involves ball output pattern
        if (solutionCase.ballOutput.Count > 0)
        {
            if (currentBallOutput.Count < currentSolutionCase.ballOutput.Count)
            {
                print("<color=red>Wrong ball output - length is insufficient.");
                print("Current output: ");
                foreach(Ball ball in currentBallOutput)
                {
                    print(ball);
                }
                print("Correct case output: ");
                foreach (Ball ball in currentSolutionCase.ballOutput)
                {
                    print(ball);
                }
                solved = false;
            }

            // Check each ball that has rolled into the output
            for (int ballNum = 0; ballNum < currentBallOutput.Count; ballNum++)
            {
                // If any of them is not as it is described in this solution case
                if (currentBallOutput[ballNum] != solutionCase.ballOutput[ballNum])
                {
                    print("<color=red>Wrong ball output");
                    print("   Current output ball: " + currentBallOutput[ballNum] + " at sequence index " + ballNum);
                    print("   Correct output ball: " + solutionCase.ballOutput[ballNum] + " at sequence index " + ballNum);
                    solved = false;
                }
            }
        }

        return solved;
    }

    /**
     * A public void method setting the level as complete.
     */
    public void setLevelComplete()
    {
        showLevelCompleteText = true;
        gameUI.showNextLevelButton(true);
        levelCompleted = true;
        LevelManager.Instance.setLevelComplete(currentLevel);
        AudioManager.Instance.playSfx("LevelComplete");
    }

    /**
     * A private void method resetting the marbles.
     */
    void resetBalls()
    {
        GameObject board = GameObject.FindGameObjectWithTag("Board");
        if (board != null)
        {
            BoardScript script = board.GetComponent<BoardScript>();

            if (script.spawningCoroutineStarted[Ball.Blue] || script.spawningCoroutineStarted[Ball.Red])
            {
                script.stopBallSpawning();
            }

            foreach (GameObject ball in ballObjects)
            {
                Destroy(ball);
            }
            ballObjects.Clear();
            ballsToRespawn.Clear();

            script.spawnBalls();
        }
        /*foreach (GameObject ball in ballObjects)
        {
            Destroy(ball);
        }
        ballObjects.Clear();
        GameObject board = GameObject.FindGameObjectWithTag("Board");
        if (board != null)
        {
            board.GetComponent<BoardScript>().spawnBalls();
        }*/
    }


    /**
     * A public void method deleting the test marbles
     */
    public void deleteTestBalls()
    {
        foreach(GameObject ball in testBallObjects)
        {
            Destroy(ball);
        }
        testBallObjects.Clear();
    }

    /**
     * A public void method starting the simulation coroutine.
     */
    public void runSimulation()
    {
        if (!AudioManager.Instance.sfxSource.mute)
        {
            AudioManager.Instance.playSfx("RunSystem");
        }

        // Save eventual custom player setting of Bits, because the system might reset balls for each case
        foreach(KeyValuePair<int, bool> state in currentBitStates)
        {
            if(playerBitCustomSetup.ContainsKey(state.Key))
            {
                playerBitCustomSetup[state.Key] = state.Value;
            }
        }

        marblesAreRunning = true;
        stopSimulation = false;

        deleteTestBalls();

        StartCoroutine(simulateCasesCheck());

    }

    /**
     * A public void method requesting reset of the simulation.
     */
    public void resetSimulation()
    {
        stopSimulation = true;
        marblesAreRunning = false;
        levelCompleted = false;
        showLevelCompleteText = false;
        currentBallOutput.Clear();
        currentStopperContents.Clear();
        //resetBalls();

        if (!reloadingLevel)
        {
            respawnRunBalls();
            restorePlayerBitSetup();
        }
    }

    /**
     * A private void method respawning the marbles that were run during the simulation.
     */
    void respawnRunBalls()
    {
        GameObject board = GameObject.FindGameObjectWithTag("Board");
        if (board != null)
        {
            BoardScript script = board.GetComponent<BoardScript>();

            foreach (GameObject ball in ballsToRespawn)
            {
                Destroy(ball);
            }
            ballsToRespawn.Clear();

            script.respawnBalls();
        }
    }

    /**
     * A public void method restoring the player Bit setup.
     */
    public void restorePlayerBitSetup()
    {
        var bitsToNotRestore = new Dictionary<int, bool>();
        foreach (KeyValuePair<int, bool> state in playerBitCustomSetup)
        {
            // If the Bit added by the player is in a chain with Bits that are used in setup simulation of solution cases, don't restore them
            foreach (List<(int cell, GameObject, GameObject)> chain in bitConnectionChains)
            {
                // If this Bit is found in a chain
                if (chain.Any(c => c.cell == state.Key))
                {
                    // Take this chain and search through the cells
                    foreach((int cell, GameObject, GameObject) tuple in chain)
                    {
                        // If this chain contains a cell that is used in setup case, delete this cell from ones to restore
                        if (currentSolutionCase.bitSetupCase.ContainsKey(tuple.cell))
                        {
                            // Do not add duplicates
                            if (!bitsToNotRestore.ContainsKey(state.Key))
                            {
                                bitsToNotRestore.Add(state.Key, state.Value);
                            }
                        }
                    }
                }
            }
            print("Going back to player setting of " + state.Value + " for bit " + state.Key);
        }
        foreach(KeyValuePair<int, bool> state in bitsToNotRestore)
        {
            playerBitCustomSetup.Remove(state.Key);
        }
        setResetBits(bitObjects, playerBitCustomSetup);
    }

    /**
     * A public void method setting and resetting Bits accordingly to the provided setup.
     * @param Dictionary<int, GameObject> containing all Bits
     * @param Dictionary<int, bool> containing all the states the Bits should be set to.
     */
    public void setResetBits(Dictionary<int, GameObject> bitObjects, Dictionary<int, bool> bitSetup)
    {
        print("<color=#2E8B57>Bit setup:");
        foreach (KeyValuePair<int, bool> setup in bitSetup)
        {
            print("<color=#2E8B57>Bit " + setup.Value + " at cell " + setup.Key);
            // Set bit (to ON) if setup.Value is true, else reset this bit (set to OFF)
            if (setup.Value)
            {
                bitObjects[setup.Key].GetComponent<Bit>().setBit();
            }
            else
            {
                bitObjects[setup.Key].GetComponent<Bit>().resetBit();
            }
        }
    }

    /**
     * A public void method deleting a Bit from the list by cell number.
     * @param cell number of the Bit to be deleted.
     */
    public void deleteBitFromLists(int cell)
    {
        playerBitCustomSetup.Remove(cell);
        bitObjects.Remove(cell);
        currentBitStates.Remove(cell);
    }

    /**
     * A public void method speeding up the game.
     */
    public void speedUpGame()
    {
        currentGameSpeed = GameSpeed.Fast;
        Time.timeScale = 3;
        AudioManager.Instance.changeSfxPitch(3);
        gameUI.updateSpeedButon();
    }

    /**
     * A public void method resetting the game speed.
     */
    public void resetGameSpeed()
    {
        currentGameSpeed = GameSpeed.Regular;
        Time.timeScale = 1;
        AudioManager.Instance.changeSfxPitch(1);
        gameUI.updateSpeedButon();
    }

    /**
     * A public void method slowing down the game.
     */
    public void slowDownGame()
    {
        currentGameSpeed = GameSpeed.Slow;
        Time.timeScale = 0.5f;
        AudioManager.Instance.changeSfxPitch(2);
        gameUI.updateSpeedButon();
    }

    /**
     * A public void method pausing the game.
     */
    public void pauseGameSpeed()
    {
        Time.timeScale = 0;
    }

    /**
     * A public void method changing starting dispenser.
     */
    public void changeStartingSide(Ball ball)
    {
        startingBallType = ball;
    }
}
