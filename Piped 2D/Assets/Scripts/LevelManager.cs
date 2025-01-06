using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = System.Random;

public enum Ball
{
    Blue,
    Red,
    Neither
}

public enum BlockType
{
    Pass,
    Crossover,
    Stopper,
    Bit,
    Gear
}

public enum Level
{
    Level1, Level2, Level3, Level4, Level5,
    Level6, Level7, Level8, Level9, Level10,
    Level11, Level12, Level13, Level14, Level15,
    Level16, Level17, Level18, Level19, Level20,
    Level21, Level22, Level23, Level24, Level25,
    Level26, Level27, Level28, Level29, Level30,
    Level31, Level32, Level33, Level34, Level35,
    Level36, Level37, Level38, Level39, Level40,
    Level41, Level42, Level43, Level44, Level45,
    Level46, Level47
}

public class LevelManager : MonoBehaviour
{

    public List<Level> levelsWithPlot = new List<Level>()
    {
        Level.Level1, Level.Level4, Level.Level5
    };

    public int[] irregularCells = {120, 119, 118, 117, 116, 114, 113, 112, 111, 110,
                                            108, 106, 104, 102, 100, 98, 96, 94, 92, 90, 88,
                                            86, 84, 82, 80, 78, 76, 74, 72, 70, 68, 66, 64,
                                            62, 60, 58, 56, 54, 52, 50, 48, 46, 44, 42, 40,
                                            38, 36, 34, 32, 30, 28, 26, 24, 22, 21, 20, 18,
                                            16, 14, 12, 11, 10, 9, 8, 6, 5, 4, 2, 1, 0};

    public int[] gearsCells = { 108, 106, 104, 102, 100, 98, 96, 94, 92, 90, 88,
                                86, 84, 82, 80, 78, 76, 74, 72, 70, 68, 66, 64,
                                62, 60, 58, 56, 54, 52, 50, 48, 46, 44, 42, 40,
                                38, 36, 34, 32, 30, 28, 26, 24, 22, 20, 18,
                                16, 14, 12, 8, 6, 4, 2};

    public List<Vector2> lockedCellCenters;

    public Dictionary<Vector3, int> posToCellDictionary = new Dictionary<Vector3, int>();
    public Dictionary<int, Vector3> cellToPosDictionary = new Dictionary<int, Vector3>();

    public Dictionary<Vector3, int> gearPosToCellDictionary = new Dictionary<Vector3, int>();
    public Dictionary<int, Vector3> gearCellToPosDictionary = new Dictionary<int, Vector3>();

    public TextMeshProUGUI counterText1;
    public TextMeshProUGUI counterText2;

    string completedLevelsString = "";
    public Dictionary<Level, bool> currentLevelStates;
    //public List<bool> currentLevelStates;

    private static LevelManager _instance;
    public static LevelManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("Level Manager is NULL");
            }
            return _instance;
        }
    }
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

    private void Start()
    {
        setupLevelStateList();
    }

    void setupLevelStateList()
    {
        //List<bool> levelStates = new List<bool>();
        Dictionary<Level, bool> levelStates = new Dictionary<Level, bool>();
        completedLevelsString = PlayerPrefs.GetString("LevelStates", "");

        // If the string was empty, create a new one
        if (completedLevelsString == "")
        {
            foreach (int i in Enum.GetValues(typeof(Level)))
            {
                completedLevelsString += "0";
                levelStates.Add((Level)i, false);
            }
        }
        // Otherwise, create a dictionary from this stirng
        else
        {
            int counter = 0;
            foreach (char c in completedLevelsString)
            {
                if (c == '1')
                {
                    levelStates[(Level)counter] = true;
                }
                else if (c == '0')
                {
                    levelStates[(Level)counter] = false;
                }
                else
                {
                    Debug.LogError("Wrong character in saved level states!\nChar: " + c);
                }
                counter++;
            }
        }
        currentLevelStates = levelStates;
    }

    public void setLevelComplete(Level level)
    {
        currentLevelStates[level] = true;
        print("Setting state of level " + ((int)level + 1) + " to true");
        saveLevelStates();
    }

    void saveLevelStates()
    {
        string newStates = "";
        foreach (KeyValuePair<Level, bool> state in currentLevelStates)
        {
            if (state.Value)
            {
                newStates += "1";
            }
            else
            {
                newStates += "0";
            }
        }
        completedLevelsString = newStates;
        PlayerPrefs.SetString("LevelStates", completedLevelsString);
    }

    // Resetting the saved states (completeness) of the levels
    public void resetLevelStates()
    {
        completedLevelsString = "";
        PlayerPrefs.SetString("LevelStates", completedLevelsString);
        currentLevelStates.Clear();
        setupLevelStateList();
        print("Current completedLevelsString: " + completedLevelsString);
    }




    public Ball getLevelStartingSide()
    {
        switch (GameManager.Instance.currentLevel)
        {
            case Level.Level4:
                return Ball.Red;
            case Level.Level27:
                return Ball.Neither;
            case Level.Level32:
                return Ball.Neither;
            case Level.Level33:
                return Ball.Neither;
            case Level.Level35:
                return Ball.Neither;
            case Level.Level36:
                return Ball.Neither;
            case Level.Level37:
                return Ball.Neither;
            case >= Level.Level40:
                return Ball.Neither;
            default:
                Debug.Log("<color=grey>INFO: This level starts simulation from Blue side (default).");
                return Ball.Blue;
        }
    }


    public Dictionary<Ball, int> getLevelBallAmounts()
    {
        switch (GameManager.Instance.currentLevel)
        {
            case Level.Level9:
                return new Dictionary<Ball, int>() {
                    { Ball.Blue, 10 },
                    { Ball.Red, 10 }
                };
            case Level.Level11:
                return new Dictionary<Ball, int>() {
                    { Ball.Blue, 2 },
                    { Ball.Red, 0 }
                };
            case Level.Level21:
                return new Dictionary<Ball, int>() {
                    { Ball.Blue, 15 }
                };
            case Level.Level22:
                return new Dictionary<Ball, int>() {
                    { Ball.Blue, 15 }
                };
            case Level.Level24:
                return new Dictionary<Ball, int>() {
                    { Ball.Blue, 12 },
                    { Ball.Red, 12 }
                };
            case Level.Level26:
                return new Dictionary<Ball, int>() {
                    { Ball.Blue, 10 },
                    { Ball.Red, 10 }
                };
            case Level.Level30:
                return new Dictionary<Ball, int>() {
                    { Ball.Blue, 15 }
                };
            case Level.Level31:
                return new Dictionary<Ball, int>() {
                    { Ball.Blue, 7 }
                };
            case Level.Level32:
                return new Dictionary<Ball, int>() {
                    { Ball.Blue, 1 },
                    { Ball.Red, 1 }
                };
            case Level.Level37:
                return new Dictionary<Ball, int>() {
                    { Ball.Blue, 16 },
                    { Ball.Red, 16 }
                };
            case Level.Level39:
                return new Dictionary<Ball, int>() {
                    { Ball.Blue, 12 },
                    { Ball.Red, 12 }
                };
            case Level.Level40:
                return new Dictionary<Ball, int>() {
                    { Ball.Blue, 7 }
                };
            case Level.Level41:
                return new Dictionary<Ball, int>() {
                    { Ball.Blue, 1 }
                };
            case Level.Level42:
                return new Dictionary<Ball, int>() {
                    { Ball.Blue, 16 },
                    { Ball.Red, 16 }
                };
            case Level.Level44:
                return new Dictionary<Ball, int>() {
                    { Ball.Blue, 16 },
                    { Ball.Red, 16 }
                };
            case Level.Level45:
                return new Dictionary<Ball, int>() {
                    { Ball.Blue, 16 },
                    { Ball.Red, 1 }
                };
            case Level.Level47:
                return new Dictionary<Ball, int>() {
                    { Ball.Blue, 0 },
                    { Ball.Red, 2 }
                };
            default:
                Debug.Log("<color=grey>INFO: This level does not have a requied setup for ball dispenser, returning standard (8, 8) set");
                return new Dictionary<Ball, int>() {
                    { Ball.Blue, 8 },
                    { Ball.Red, 8 }
                };
        }
    }

    



    // GET: STARTING BOARD for the level
    public Dictionary<BlockType, int[]> getLevelStartingBoard(int levelNum = 0)
    {
        Level currentLevel;
        if(levelNum != 0)
        {
            currentLevel = (Level)(levelNum - 1);
        }
        else
        {
            currentLevel = GameManager.Instance.currentLevel;
        }
        switch (currentLevel)
        {
            case Level.Level1:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Pass, new int[]{ -1, 4, -8, -19, -30, -41 } }
                };
            case Level.Level2:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Pass, new int[]{ -1, -4, -9, -15, -21, -27 } }
                };
            case Level.Level3:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Pass, new int[]{ -1, 2, 33, -38, 44, -49 } }
                };
            case Level.Level4:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Pass, new int[]{ 1, -2, 3, -6, 7, -11 } }
                };
            case Level.Level5:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Pass, new int[]{ -1, -4, -15, 21, -26, 32, -37, 43, -48 } },
                    { BlockType.Crossover, new int[]{ 9 } }
                };
            case Level.Level6:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Pass, new int[]{ -4, 5, -14, 15, -25, 26, -36, 37, -47, 48 } }
                };
            case Level.Level7:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Crossover, new int[]{ 8, 9, 18, 25, 35, 41 } }
                };
            case Level.Level8:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Pass, new int[]{ -1, 2, -4, 5 } },
                    { BlockType.Bit, new int[]{ 9 } }
                };
            case Level.Level9:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Pass, new int[]{ 2, -4, 5 } },
                    { BlockType.Bit, new int[]{ -1 } },
                    { BlockType.Crossover, new int[]{ 9 } }
                };
            case Level.Level10:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Bit, new int[]{ -1, 2 } },
                    { BlockType.Crossover, new int[]{ 9 } }
                };
            case Level.Level11:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Bit, new int[] { 9, 29, 30, 31, 32, 33 } }
                };
            case Level.Level12:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Pass, new int[]{ -1, -4, 32, 37 } },
                    { BlockType.Bit, new int[]{ 9 } },
                    { BlockType.Stopper, new int[]{ 42 } },
                };
            case Level.Level13:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Pass, new int[]{ -1, -4, -15, -21, 27, 32, 37 } },
                    { BlockType.Bit, new int[]{ -9 } },
                    { BlockType.Stopper, new int[]{ 42 } },
                };
            case Level.Level14:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Bit, new int[] { 9 } },
                    { BlockType.Stopper, new int[] { 42 } }
                };
            case Level.Level15:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Pass, new int[]{ 2, -4, 5 } },
                    { BlockType.Bit, new int[]{ -1, 42 } },
                    { BlockType.Stopper, new int[]{ 9 } },
                };
            case Level.Level16:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Pass, new int[]{ -3 } },
                    { BlockType.Bit, new int[]{ 1, 8 } },
                    { BlockType.Stopper, new int[]{ 13 } },
                };
            case Level.Level17:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Pass, new int[]{ 4, -5 } },
                    { BlockType.Bit, new int[]{ 1, 2, -8, 10 } },
                    { BlockType.Stopper, new int[]{ 15 } },
                };
            case Level.Level18:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Bit, new int[] { 9, 20 } },
                    { BlockType.Stopper, new int[] { 30, 32 } }
                };
            case Level.Level19:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Bit, new int[] { 9, 20 } },
                    { BlockType.Stopper, new int[] { 51 } }
                };
            case Level.Level20:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Bit, new int[] { 9, 20 } },
                    { BlockType.Stopper, new int[] { 51 } }
                };
            case Level.Level21:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Bit, new int[] { 1, 8, 19, 30 } },
                    { BlockType.Pass, new int[] { -3, -4, -13, -14, -24, -25, -35, -36, -41, -47 } }
                };
            case Level.Level22:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Bit, new int[] { -1, -8, -19, -30 } },
                    { BlockType.Pass, new int[] { 3, 4, 13, 14, 24, 25, 35, 36, 41, 46 } }
                };
            case Level.Level23:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Pass, new int[] { 3, 4, -7, 13, 14, -18, 24, -29, 35, -40, 46 } },
                    { BlockType.Bit, new int[] { 1, 8, 19 } },
                    { BlockType.Stopper, new int[] { 25 } }
                };
            case Level.Level24:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Bit, new int[] { 1, 8, 19, 30 } }
                };
            case Level.Level25:
                return new Dictionary<BlockType, int[]>() {
                };
            case Level.Level26:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Bit, new int[] { 2 } },
                    { BlockType.Stopper, new int[] { 6 } }
                };
            case Level.Level27:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Bit, new int[] { 3, 6, 9, 13, 20, 24, 31, 35, 42 } }
                };
            case Level.Level28:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Pass, new int[] { -1, 2, -4, 5, -19, 21, 25, -26, -30, 32, 36, -37, -41, 43, 47, -48 } },
                    { BlockType.Bit, new int[] { 9 } }
                };
            case Level.Level29:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Bit, new int[] { 1 } }
                };
            case Level.Level30:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Bit, new int[] { 1, 8, 19, 24 } }
                };
            case Level.Level31:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Bit, new int[] { 1, 18, 20, 29, 31, 40, 42 } }
                };
            case Level.Level32:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Bit, new int[] { 14, 15, 20 } },
                    { BlockType.Gear, new int[] { 18 } }
                };
            case Level.Level33:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Bit, new int[] { 21, 22, 24, 27, 29, 30 } },
                    { BlockType.Gear, new int[] { 25, 33 } }
                };
            case Level.Level34:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Bit, new int[] { 9, 20 } },
                    { BlockType.Stopper, new int[] { 31 } }
                };
            case Level.Level35:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Bit, new int[] { 9, 20, 31 } },
                    { BlockType.Stopper, new int[] { 42 } }
                };
            case Level.Level36:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Bit, new int[] { 14, 15 } },
                    { BlockType.Stopper, new int[] { 31 } }
                };
            case Level.Level37:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Bit, new int[] { 1, 2, 8, 10, 19, 21, 32 } }
                };
            case Level.Level38:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Bit, new int[] { 31, 42, 51 } }
                };
            case Level.Level39:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Pass, new int[] { -2, 6, -10, 16, -21, 27, -32, 38, 48, -49, 51 } },
                    { BlockType.Crossover, new int[] { 43 } }
                };
            case Level.Level40:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Bit, new int[] { 4, 14, 25, 36, 47 } }
                };
            case Level.Level41:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Bit, new int[] { 9, 20, 31, 42 } }
                };
            case Level.Level42:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Bit, new int[] { 1, 2, 8, 10, 19, 21, 30, 32 } },
                    { BlockType.Stopper, new int[] { 47 }}
                };
            case Level.Level43:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Bit, new int[] { 14, 15 } },
                    { BlockType.Stopper, new int[] { 31 }}
                };
            case Level.Level44:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Bit, new int[] { 1, 2, 8, 10, 19, 21, 32 } },
                    { BlockType.Stopper, new int[] { 47 }}
                };
            case Level.Level45:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Bit, new int[] { 13, 15, 24, 26, 35, 37 } },
                    { BlockType.Stopper, new int[] { 51 }}
                };
            case Level.Level46:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Bit, new int[] { 40, 41, 42, 43, 44 } }
                };
            case Level.Level47:
                return new Dictionary<BlockType, int[]>() {
                    { BlockType.Bit, new int[] { 10, 20, 21, 31, 42 } }
                };
            default:
                Debug.LogError("This level does not have a starting board!");
                return new Dictionary<BlockType, int[]>() { };
        }
    }

    public Dictionary<BlockType, int> getLevelBlocks()
    {
        switch (GameManager.Instance.currentLevel)
        {
            case Level.Level1:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 4 }
                };
            case Level.Level2:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 5 }
                };
            case Level.Level3:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 6 }
                };
            case Level.Level4:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 13 }
                };
            case Level.Level5:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 9 }
                };
            case Level.Level6:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 2 },
                    { BlockType.Crossover, 5 },
                };
            case Level.Level7:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 6 },
                };
            case Level.Level8:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 14 },
                };
            case Level.Level9:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 18 },
                };
            case Level.Level10:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 22 },
                };

                // CHANGE THIS LATER!!!!!!!!!!!!!!!!!!!!!!!!!!!
            case Level.Level11:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 15 },
                    { BlockType.Crossover, 3 },
                    { BlockType.Bit, 4 },
                    { BlockType.Gear, 4 }
                };
            case Level.Level12:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 3 },
                };
            case Level.Level13:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 12 },
                };
            case Level.Level14:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 20 }
                };
            case Level.Level15:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 11 },
                    { BlockType.Crossover, 2 },
                };
            case Level.Level16:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 10 }
                };
            case Level.Level17:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 50 }
                };
            case Level.Level18:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 7 }
                };
            case Level.Level19:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 50 },
                    { BlockType.Crossover, 2 },
                };
            case Level.Level20:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 50 },
                    { BlockType.Crossover, 2 },
                };
            case Level.Level21:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 5 }
                };
            case Level.Level22:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 4 }
                };
            case Level.Level23:
                return new Dictionary<BlockType, int>() {
                };
            case Level.Level24:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 14 },
                    { BlockType.Stopper, 1 },
                };
            case Level.Level25:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 50 },
                    { BlockType.Bit, 5 },
                    { BlockType.Stopper, 1 },
                };
            case Level.Level26:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 50 },
                    { BlockType.Bit, 2 },
                    { BlockType.Crossover, 2 },
                };
            case Level.Level27:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 50 },
                    { BlockType.Bit, 1 },
                    { BlockType.Stopper, 1 },
                };
            case Level.Level28:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 1 },
                    { BlockType.Bit, 1 },
                    { BlockType.Gear, 1 },
                };
            case Level.Level29:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 50 },
                    { BlockType.Crossover, 1 },
                    { BlockType.Bit, 1 },
                    { BlockType.Gear, 1 },
                };
            case Level.Level30:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 14 },
                    { BlockType.Bit, 1 },
                    { BlockType.Gear, 1 },
                };
            case Level.Level31:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 50 },
                    { BlockType.Crossover, 1 },
                    { BlockType.Bit, 2 },
                    { BlockType.Gear, 1 },
                };
            case Level.Level32:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 20 }
                };
            case Level.Level33:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 50 },
                    { BlockType.Crossover, 2 },
                    { BlockType.Stopper, 1 },
                    { BlockType.Bit, 1 },
                };
            case Level.Level34:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 50 },
                    { BlockType.Crossover, 2 },
                    { BlockType.Bit, 1 },
                    { BlockType.Gear, 1 },
                };
            case Level.Level35:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 50 }
                };
            case Level.Level36:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 50 },
                    { BlockType.Crossover, 4 },
                    { BlockType.Bit, 1 },
                };
            case Level.Level37:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 50 },
                    { BlockType.Crossover, 2 },
                    { BlockType.Stopper, 1 },
                    { BlockType.Bit, 50 },
                };
            case Level.Level40:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 50 },
                };
            case Level.Level41:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 11 },
                    { BlockType.Bit, 4 },
                    { BlockType.Gear, 4 },
                };
            case Level.Level47:
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 16 },
                    { BlockType.Crossover, 2 },
                    { BlockType.Bit, 1 },
                };
            default:
                Debug.Log("<color=grey>INFO: This level does not have a set of blocks, returning default - all possible.");
                return new Dictionary<BlockType, int>() {
                    { BlockType.Pass, 50 },
                    { BlockType.Crossover, 50 },
                    { BlockType.Stopper, 50 },
                    { BlockType.Bit, 50 },
                    { BlockType.Gear, 50 },
                };
        }
    }



    // Get full list of solution cases that need to be solved correctly for the level to be complete
    public List<SolutionCase> getLevelSolutionCases()
    {
        switch (GameManager.Instance.currentLevel)
        {
            case Level.Level1:
                {
                    var ballOutput = new List<Ball>() {
                        Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue
                    };
                    return new List<SolutionCase> { new SolutionCase(_ballOutput: ballOutput) };
                }
            case Level.Level2:
                {
                    var ballOutput = new List<Ball>() {
                        Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue
                    };
                    return new List<SolutionCase> { new SolutionCase(_ballOutput: ballOutput) };
                }
            case Level.Level3:
                {
                    var ballOutput = new List<Ball>() {
                        Ball.Blue, Ball.Red, Ball.Red, Ball.Red, Ball.Red, Ball.Red, Ball.Red, Ball.Red, Ball.Red
                    };
                    return new List<SolutionCase> { new SolutionCase(_ballOutput: ballOutput) };
                }
            case Level.Level4:
                {
                    var ballOutput = new List<Ball>() {
                        Ball.Red, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue
                    };
                    return new List<SolutionCase> { new SolutionCase(_ballOutput: ballOutput) };
                }
            case Level.Level5:
                {
                    var ballOutput = new List<Ball>() {
                        Ball.Blue, Ball.Red, Ball.Blue, Ball.Red, Ball.Blue, Ball.Red, Ball.Blue, Ball.Red, Ball.Blue, 
                        Ball.Red, Ball.Blue, Ball.Red, Ball.Blue, Ball.Red, Ball.Blue, Ball.Red
                    };
                    return new List<SolutionCase> { new SolutionCase(_ballOutput: ballOutput) };
                }
            case Level.Level6:
                {
                    var ballOutput = new List<Ball>() {
                        Ball.Blue, Ball.Red, Ball.Blue, Ball.Red, Ball.Blue, Ball.Red, Ball.Blue, Ball.Red, Ball.Blue,
                        Ball.Red, Ball.Blue, Ball.Red, Ball.Blue, Ball.Red, Ball.Blue, Ball.Red
                    };
                    return new List<SolutionCase> { new SolutionCase(_ballOutput: ballOutput) };
                }
            case Level.Level7:
                {
                    var ballOutput = new List<Ball>() {
                        Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue
                    };
                    return new List<SolutionCase> { new SolutionCase(_ballOutput: ballOutput) };
                }
            case Level.Level8:
                {
                    var ballOutput = new List<Ball>() {
                        Ball.Blue, Ball.Red, Ball.Blue, Ball.Red, Ball.Blue, Ball.Red, Ball.Blue, Ball.Red, Ball.Blue,
                        Ball.Red, Ball.Blue, Ball.Red, Ball.Blue, Ball.Red, Ball.Blue, Ball.Red
                    };
                    return new List<SolutionCase> { new SolutionCase(_ballOutput: ballOutput) };
                }
            case Level.Level9:
                {
                    var bitRequiredSetup = new Dictionary<int, bool>() { { 1, true } };
                    var ballOutput = new List<Ball>() {
                        Ball.Blue, Ball.Blue, Ball.Red, Ball.Blue, Ball.Blue, Ball.Red, Ball.Blue, Ball.Blue, Ball.Red, 
                        Ball.Blue, Ball.Blue, Ball.Red,Ball.Blue,  Ball.Blue, Ball.Red
                    };
                    return new List<SolutionCase> { new SolutionCase(_ballOutput: ballOutput, _bitRequiredSetup: bitRequiredSetup) };
                }
            case Level.Level10:
                {
                    var bitRequiredSetup = new Dictionary<int, bool>() { { 1, true }, { 2, false } };
                    var ballOutput = new List<Ball>() {
                        Ball.Blue, Ball.Blue, Ball.Red, Ball.Red, Ball.Blue, Ball.Blue, Ball.Red, Ball.Red, Ball.Blue, Ball.Blue,
                        Ball.Red, Ball.Red, Ball.Blue, Ball.Blue, Ball.Red, Ball.Red
                    };
                    return new List<SolutionCase> { new SolutionCase(_ballOutput: ballOutput, _bitRequiredSetup: bitRequiredSetup) };
                }
            case Level.Level11:
                {
                    var bitRequiredSetup = new Dictionary<int, bool>() {
                        { 29, false },
                        { 30, false },
                        { 31, false },
                        { 32, false },
                        { 33, false }
                    };
                    var bitFinalStates = new Dictionary<int, bool>() {
                        { 29, false },
                        { 30, true },
                        { 31, false },
                        { 32, false },
                        { 33, true }
                    };
                    var ballOutput = new List<Ball>() { Ball.Blue, Ball.Blue };
                    return new List<SolutionCase> { new SolutionCase(_ballOutput: ballOutput, 
                        _bitRequiredSetup: bitRequiredSetup, _bitFinalStates: bitFinalStates) };
                }
            case Level.Level12:
                {
                    var stopperContent = new Dictionary<int, Ball>() { { 42, Ball.Blue } };

                    return new List<SolutionCase> {
                        new SolutionCase(_stopperContents: stopperContent)
                    };
                }
            case Level.Level13:
                {
                    var stopperContent = new Dictionary<int, Ball>() { { 42, Ball.Red } };

                    return new List<SolutionCase> {
                        new SolutionCase(_stopperContents: stopperContent)
                    };
                }
            case Level.Level14:
                {
                    var bitSetupCase_1 = new Dictionary<int, bool>() { { 9, false }};
                    var stopperContent_1 = new Dictionary<int, Ball>() { { 42, Ball.Blue } };

                    var bitSetupCase_2 = new Dictionary<int, bool>() { { 9, true } };
                    var stopperContent_2 = new Dictionary<int, Ball>() { { 42, Ball.Red } };

                    return new List<SolutionCase> {
                        new SolutionCase(_bitSetupCase: bitSetupCase_1, _stopperContents: stopperContent_1, _resetBallsAfterCase: true),
                        new SolutionCase(_bitSetupCase: bitSetupCase_2, _stopperContents: stopperContent_2, _resetBallsAfterCase: true)
                    };
                }
            case Level.Level15:
                {
                    var bitSetupCase_1 = new Dictionary<int, bool>() { { 42, false } };
                    var stopperContent_1 = new Dictionary<int, Ball>() { { 9, Ball.Blue } };

                    var bitSetupCase_2 = new Dictionary<int, bool>() { { 42, true } };
                    var stopperContent_2 = new Dictionary<int, Ball>() { { 9, Ball.Red } };

                    return new List<SolutionCase> {
                        new SolutionCase(_bitSetupCase: bitSetupCase_1, _stopperContents: stopperContent_1, _resetBallsAfterCase: true),
                        new SolutionCase(_bitSetupCase: bitSetupCase_2, _stopperContents: stopperContent_2, _resetBallsAfterCase: true)
                    };
                }
            case Level.Level16:
                {
                    var stopperContent = new Dictionary<int, Ball>() { { 13, Ball.Blue } };
                    var ballOutput = new List<Ball>() { Ball.Blue, Ball.Blue, Ball.Blue };

                    return new List<SolutionCase> {
                        new SolutionCase(_ballOutput: ballOutput, _stopperContents: stopperContent)
                    };
                }
            case Level.Level17:
                {
                    var stopperContent = new Dictionary<int, Ball>() { { 15, Ball.Red } };
                    var ballOutput = new List<Ball>() { Ball.Blue, Ball.Blue, Ball.Blue, Ball.Red, Ball.Red, Ball.Red };

                    return new List<SolutionCase> {
                        new SolutionCase(_ballOutput: ballOutput, _stopperContents: stopperContent)
                    };
                }
            case Level.Level18:
                {
                    var bitSetupCase_1 = new Dictionary<int, bool>() { { 9, true }, { 20, true } };
                    var stopperContent_1 = new Dictionary<int, Ball>() { { 30, Ball.Blue }, { 32, Ball.Neither } };

                    var bitSetupCase_2 = new Dictionary<int, bool>() { { 9, true }, { 20, false } };
                    var bitSetupCase_3 = new Dictionary<int, bool>() { { 9, false }, { 20, false } };
                    var bitSetupCase_4 = new Dictionary<int, bool>() { { 9, false }, { 20, true } };
                    var stopperContent_2 = new Dictionary<int, Ball>() { { 30, Ball.Neither }, { 32, Ball.Blue } };

                    return new List<SolutionCase> {
                        new SolutionCase(_bitSetupCase: bitSetupCase_1, _stopperContents: stopperContent_1, _resetBallsAfterCase: true),
                        new SolutionCase(_bitSetupCase: bitSetupCase_2, _stopperContents: stopperContent_2, _resetBallsAfterCase: true),
                        new SolutionCase(_bitSetupCase: bitSetupCase_3, _stopperContents: stopperContent_2, _resetBallsAfterCase: true),
                        new SolutionCase(_bitSetupCase: bitSetupCase_4, _stopperContents: stopperContent_2, _resetBallsAfterCase: true)
                    };
                }
            case Level.Level19:
                {
                    var bitSetupCase_1 = new Dictionary<int, bool>() { { 9, true }, { 20, true } };
                    var stopperContent_1 = new Dictionary<int, Ball>() { { 51, Ball.Blue } };

                    var bitSetupCase_2 = new Dictionary<int, bool>() { { 9, true }, { 20, false } };
                    var bitSetupCase_3 = new Dictionary<int, bool>() { { 9, false }, { 20, false } };
                    var bitSetupCase_4 = new Dictionary<int, bool>() { { 9, false }, { 20, true } };
                    var stopperContent_2 = new Dictionary<int, Ball>() { { 51, Ball.Red } };

                    return new List<SolutionCase> {
                        new SolutionCase(_bitSetupCase: bitSetupCase_1, _stopperContents: stopperContent_1, _resetBallsAfterCase: true),
                        new SolutionCase(_bitSetupCase: bitSetupCase_2, _stopperContents: stopperContent_2, _resetBallsAfterCase: true),
                        new SolutionCase(_bitSetupCase: bitSetupCase_3, _stopperContents: stopperContent_2, _resetBallsAfterCase: true),
                        new SolutionCase(_bitSetupCase: bitSetupCase_4, _stopperContents: stopperContent_2, _resetBallsAfterCase: true)
                    };
                }
            case Level.Level20:
                {
                    var bitSetupCase_1 = new Dictionary<int, bool>() { { 9, false }, { 20, false } };
                    var stopperContent_1 = new Dictionary<int, Ball>() { { 51, Ball.Red } };

                    var bitSetupCase_2 = new Dictionary<int, bool>() { { 9, true }, { 20, false } };
                    var bitSetupCase_3 = new Dictionary<int, bool>() { { 9, true }, { 20, true } };
                    var bitSetupCase_4 = new Dictionary<int, bool>() { { 9, false }, { 20, true } };
                    var stopperContent_2 = new Dictionary<int, Ball>() { { 51, Ball.Blue } };

                    return new List<SolutionCase> {
                        new SolutionCase(_bitSetupCase: bitSetupCase_1, _stopperContents: stopperContent_1, _resetBallsAfterCase: true),
                        new SolutionCase(_bitSetupCase: bitSetupCase_2, _stopperContents: stopperContent_2, _resetBallsAfterCase: true),
                        new SolutionCase(_bitSetupCase: bitSetupCase_3, _stopperContents: stopperContent_2, _resetBallsAfterCase: true),
                        new SolutionCase(_bitSetupCase: bitSetupCase_4, _stopperContents: stopperContent_2, _resetBallsAfterCase: true)
                    };
                }
            case Level.Level21:
                {
                    var bitRequiredSetup = new Dictionary<int, bool>() {
                        { 1, false },
                        { 8, false },
                        { 19, false },
                        { 30, false }
                    };

                    List<SolutionCase> solutionCases = new List<SolutionCase>();
                    var ballOutput = new List<Ball>();

                    // Create new solution cases in a loop and add them to the list
                    for (int regValue = 1; regValue < 16; regValue++)
                    {
                        // Add a ball to the output solution case with each iteration
                        ballOutput.Add(Ball.Blue);

                        // Add a copy of the ball output to the solution along with bit final states that change with every loop iteration
                        solutionCases.Add(new SolutionCase(_ballOutput: ballOutput.ToList(), _bitRequiredSetup: bitRequiredSetup, 
                        _bitFinalStates: getBitStatesFromNumber(bitRequiredSetup.Keys.ToList(), regValue)));

                       /* print("------Solution Case------");
                        string ballstring = "";
                        foreach (Ball ball in ballOutput)
                        {
                            ballstring = ballstring + "B-ball ";
                        }
                        print(ballstring);
                        foreach (KeyValuePair<int, bool> state in getBinaryInBitStates(bitRequiredSetup.Keys.ToList(), regValue))
                        {
                            print("Bit: " + state.Key + ", " + state.Value);
                        }*/
                    }

                    return solutionCases;
                }
            case Level.Level22:
                {
                    var bitRequiredSetup = new Dictionary<int, bool>() {
                        { 1, true },
                        { 8, true },
                        { 19, true },
                        { 30, true }
                    };

                    List<SolutionCase> solutionCases = new List<SolutionCase>();
                    var ballOutput = new List<Ball>();

                    // Create new solution cases in a loop and add them to the list
                    for (int regValue = 14; regValue >= 0 ; regValue--)
                    {
                        // Add a ball to the output solution case with each iteration
                        ballOutput.Add(Ball.Blue);

                        // Add a copy of the ball output to the solution along with bit final states that change with every loop iteration
                        solutionCases.Add(new SolutionCase(_ballOutput: ballOutput.ToList(), _bitRequiredSetup: bitRequiredSetup,
                        _bitFinalStates: getBitStatesFromNumber(bitRequiredSetup.Keys.ToList(), regValue)));
                    }

                    return solutionCases;
                }
            case Level.Level23:
                {
                    var stopperContent = new Dictionary<int, Ball>() { { 25, Ball.Blue } };
                    var ballOutput = new List<Ball>() { Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue };

                    return new List<SolutionCase> {
                        new SolutionCase(_ballOutput: ballOutput, _stopperContents: stopperContent)
                    };
                }
            case Level.Level24:
                {
                    // 0 in cell position means the Stopper will be placed by the player, so no cell is defined here
                    var stopperContent = new Dictionary<int, Ball>() { { 0, Ball.Blue } };
                    var ballOutput = new List<Ball>() { Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue,
                    Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue };

                    return new List<SolutionCase> {
                        new SolutionCase(_ballOutput: ballOutput, _stopperContents: stopperContent)
                    };
                }
            case Level.Level25:
                {
                    // 0 in cell position means the Stopper will be placed by the player, so no cell is defined here
                    var stopperContent = new Dictionary<int, Ball>() { { 0, Ball.Red } };
                    var ballOutput = new List<Ball>() { Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue,
                    Ball.Red, Ball.Red, Ball.Red };

                    return new List<SolutionCase> {
                        new SolutionCase(_ballOutput: ballOutput, _stopperContents: stopperContent)
                    };
                }
            case Level.Level26:
                {
                    // 0 in cell position means the Stopper will be placed by the player, so no cell is defined here
                    var stopperContent = new Dictionary<int, Ball>() { { 0, Ball.Red } };
                    var ballOutput = new List<Ball>() { Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Red,
                    Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue };

                    return new List<SolutionCase> {
                        new SolutionCase(_ballOutput: ballOutput, _stopperContents: stopperContent)
                    };
                }
            case Level.Level27:
                {
                    var bitSetupCase = new Dictionary<int, bool>() {
                        { 3, false },
                        { 13, false },
                        { 24, false },
                        { 35, false },
                        { 9, false },
                        { 20, false },
                        { 31, false },
                        { 42, false },
                        { 6, false }
                    };

                    List<SolutionCase> solutionCases = new List<SolutionCase>();

                    // Create new solution cases in a loop and add them to the list
                    for (int i = 0; i < 3; i++)
                    {
                        Random random = new Random();

                        for (int j = 0; j < 5; j++)
                        {
                            int index = random.Next(bitSetupCase.Count);
                            KeyValuePair<int, bool> pair = bitSetupCase.ElementAt(index);
                            bitSetupCase[pair.Key] = !pair.Value;
                        }
                        var newBitSetupCase = new Dictionary<int, bool>(bitSetupCase);

                        solutionCases.Add(new SolutionCase(_bitSetupCase: newBitSetupCase,
                        _bitFinalStates: getOppositeBitStates(bitSetupCase), _resetBallsAfterCase: true));
                    }

                    return solutionCases;
                }
            case Level.Level28:
                {
                    var ballOutput = new List<Ball>() {
                        Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue
                    };
                    return new List<SolutionCase> { new SolutionCase(_ballOutput: ballOutput) };
                }
            case Level.Level29:
                {
                    var ballOutput = new List<Ball>() {
                        Ball.Blue, Ball.Red, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue
                    };
                    return new List<SolutionCase> { new SolutionCase(_ballOutput: ballOutput) };
                }
            case Level.Level30:
                {
                    var bitRequiredSetup = new Dictionary<int, bool>() {
                        { 1, false },
                        { 8, false },
                        { 19, false },
                        { 24, false }
                    };

                    List<SolutionCase> solutionCases = new List<SolutionCase>();
                    var ballOutput = new List<Ball>();

                    // Create new solution cases in a loop and add them to the list
                    for (int regValue = 1; regValue < 16; regValue++)
                    {
                        // Add a ball to the output solution case with each iteration
                        ballOutput.Add(Ball.Blue);

                        // Add a copy of the ball output to the solution along with bit final states that change with every loop iteration
                        solutionCases.Add(new SolutionCase(_ballOutput: ballOutput.ToList(), _bitRequiredSetup: bitRequiredSetup,
                        _bitFinalStates: getBitStatesFromNumber(bitRequiredSetup.Keys.ToList(), regValue)));
                    }

                    return solutionCases;

                    /*List<SolutionCase> solutionCases = new List<SolutionCase>();
                    var ballOutput = new List<Ball>();

                    // The register is supposed to count to 7 twice - once normally, and then with the overflow Bit set to true
                    for(int i = 0; i < 2; i++)
                    {
                        // Set overflow to true in second iteration
                        bool overflow = (i == 0) ? false : true;

                        // Create new solution cases in a loop and add them to the list
                        for (int regValue = 0; regValue < 8; regValue++)
                        {
                            // Add a ball to the output solution case with each iteration
                            ballOutput.Add(Ball.Blue);

                            List<int> registerKeys = bitRequiredSetup.Keys.ToList();
                            // Remove overflow Bit from the register binary
                            registerKeys.RemoveAt(3);
                            Dictionary<int, bool> finalBitStates = getBinaryInBitStates(registerKeys, regValue);

                            // Add an overflow Bit state
                            finalBitStates.Add(24, overflow);

                            // Add a copy of the ball output to the solution along with bit final states that change with every loop iteration
                            solutionCases.Add(new SolutionCase(_ballOutput: ballOutput.ToList(), _bitRequiredSetup: bitRequiredSetup,
                            _bitFinalStates: finalBitStates));
                        }
                    }

                    return solutionCases;*/
                }
            case Level.Level31:
                {
                    var bitRequiredSetup = new Dictionary<int, bool>() {
                        { 18, false },
                        { 29, false },
                        { 40, false },
                        { 20, false },
                        { 31, false },
                        { 42, false },
                    };

                    var bitFinalStates_1 = new Dictionary<int, bool>() {
                        { 18, true },
                        { 29, true },
                        { 40, true },
                        { 20, false },
                        { 31, false },
                        { 42, false },
                    };

                    var bitFinalStates_2 = new Dictionary<int, bool>() {
                        { 18, false },
                        { 29, false },
                        { 40, false },
                        { 20, true },
                        { 31, true },
                        { 42, true },
                    };

                    var bitSetupCase_1 = new Dictionary<int, bool>() {
                        { 1, false },
                        { 18, false },
                        { 29, false },
                        { 40, false },
                        { 20, false },
                        { 31, false },
                        { 42, false },
                    };

                    var bitSetupCase_2 = new Dictionary<int, bool>() {
                        { 1, true },
                        { 18, false },
                        { 29, false },
                        { 40, false },
                        { 20, false },
                        { 31, false },
                        { 42, false },
                    };

                    var ballOutput = new List<Ball>() { Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Blue};

                    return new List<SolutionCase> {
                        new SolutionCase(_bitSetupCase: bitSetupCase_1, _bitRequiredSetup: bitRequiredSetup, 
                        _bitFinalStates: bitFinalStates_1, _ballOutput: ballOutput, _resetBallsAfterCase: true),
                        new SolutionCase(_bitSetupCase: bitSetupCase_2, _bitRequiredSetup: bitRequiredSetup,
                        _bitFinalStates: bitFinalStates_2, _ballOutput: ballOutput, _resetBallsAfterCase: true),
                    };
                }
            case Level.Level32:
                {
                    var bitSetupCase_A = new Dictionary<int, bool>()
                    {
                        { 20, false }
                    };
                    var bitSetupCase_B = new Dictionary<int, bool>()
                    {
                        { 20, true }
                    };
                    var ballOutput_1 = new List<Ball>() { Ball.Blue };
                    var ballOutput_2 = new List<Ball>() { Ball.Red };
                    var bitFinalStates_1 = new Dictionary<int, bool>()
                    {
                        { 20, false }
                    };
                    var bitFinalStates_2 = new Dictionary<int, bool>()
                    {
                        { 20, true }
                    };

                    return new List<SolutionCase> {
                        new SolutionCase(_bitSetupCase: bitSetupCase_A, _startingSideCase: Ball.Blue,
                        _bitFinalStates: bitFinalStates_1, _ballOutput: ballOutput_1, _resetBallsAfterCase: true),
                        new SolutionCase(_bitSetupCase: bitSetupCase_A, _startingSideCase: Ball.Red,
                        _bitFinalStates: bitFinalStates_2, _ballOutput: ballOutput_2, _resetBallsAfterCase: true),
                        new SolutionCase(_bitSetupCase: bitSetupCase_B, _startingSideCase: Ball.Blue,
                        _bitFinalStates: bitFinalStates_1, _ballOutput: ballOutput_1, _resetBallsAfterCase: true),
                        new SolutionCase(_bitSetupCase: bitSetupCase_B, _startingSideCase: Ball.Red,
                        _bitFinalStates: bitFinalStates_2, _ballOutput: ballOutput_2, _resetBallsAfterCase: true),
                    };
                }
            case Level.Level33:
                {
                    var bitSetupCase_1 = new Dictionary<int, bool>()
                    {
                        { 24, false },
                        { 27, true },
                    };
                    var bitSetupCase_2 = new Dictionary<int, bool>()
                    {
                        { 24, true },
                        { 27, false },
                    };
                    var bitSetupCase_3 = new Dictionary<int, bool>()
                    {
                        { 24, false },
                        { 27, false },
                    };
                    var bitSetupCase_4 = new Dictionary<int, bool>()
                    {
                        { 24, true },
                        { 27, true },
                    };
                    var bitFinalStates_1 = new Dictionary<int, bool>()
                    {
                        { 24, false },
                        { 27, false },
                    };
                    var bitFinalStates_2 = new Dictionary<int, bool>()
                    {
                        { 24, true },
                        { 27, true },
                    };

                    return new List<SolutionCase> {
                        new SolutionCase(_bitSetupCase: bitSetupCase_1, _bitFinalStates: bitFinalStates_1, 
                        _resetBallsAfterCase: true),
                        new SolutionCase(_bitSetupCase: bitSetupCase_3, _bitFinalStates: bitFinalStates_1,
                        _resetBallsAfterCase: true),
                        new SolutionCase(_bitSetupCase: bitSetupCase_2, _bitFinalStates: bitFinalStates_2,
                        _resetBallsAfterCase: true),
                        new SolutionCase(_bitSetupCase: bitSetupCase_4, _bitFinalStates: bitFinalStates_2,
                        _resetBallsAfterCase: true),
                    };
                }
            case Level.Level34:
                {
                    var bitSetupCase_1 = new Dictionary<int, bool>()
                    {
                        { 9, false },
                        { 20, false },
                    };
                    var bitSetupCase_2 = new Dictionary<int, bool>()
                    {
                        { 9, true },
                        { 20, false },
                    };
                    var bitSetupCase_3 = new Dictionary<int, bool>()
                    {
                        { 9, false },
                        { 20, true },
                    };
                    var bitSetupCase_4 = new Dictionary<int, bool>()
                    {
                        { 9, true },
                        { 20, true },
                    };

                    var stopperContent_1 = new Dictionary<int, Ball>()
                    {
                        { 31, Ball.Red }
                    };
                    var stopperContent_2 = new Dictionary<int, Ball>()
                    {
                        { 31, Ball.Blue }
                    };

                    return new List<SolutionCase> {
                        new SolutionCase(_bitSetupCase: bitSetupCase_1, _stopperContents: stopperContent_1,
                        _resetBallsAfterCase: true),
                        new SolutionCase(_bitSetupCase: bitSetupCase_2, _stopperContents: stopperContent_2,
                        _resetBallsAfterCase: true),
                        new SolutionCase(_bitSetupCase: bitSetupCase_3, _stopperContents: stopperContent_2,
                        _resetBallsAfterCase: true),
                        new SolutionCase(_bitSetupCase: bitSetupCase_4, _stopperContents: stopperContent_1,
                        _resetBallsAfterCase: true),
                    };
                }
            case Level.Level35:
                {

                    List<SolutionCase> solutionCases = new List<SolutionCase>();

                    // Create new solution cases in a loop and add them to the list
                    for (int regValue = 1; regValue < 8; regValue++)
                    {

                        // Make the stopper content a blue ball if register value is even, otherwise a white ball
                        var stopperContent = new Dictionary<int, Ball>() { { 42, (regValue % 2 == 0) ? Ball.Blue : Ball.Red } };

                        solutionCases.Add(new SolutionCase(_bitSetupCase: getBitStatesFromNumber(new List<int>(){ 9, 20, 31 }, regValue),
                            _stopperContents: stopperContent, _resetBallsAfterCase: true));

                        /*print("------Solution Case------");
                        string ballstring = "";
                        foreach(Ball ball in ballOutput)
                        {
                            ballstring = ballstring + "B-ball ";
                        }
                        print(ballstring);
                        foreach(KeyValuePair<int, bool> state in makeBitRegisterStates(bitRequiredSetup.Keys.ToList(), 4, regValue))
                        {
                            print("Bit: " + state.Key + ", " + state.Value);
                        }*/
                    }

                    return solutionCases;
                }
            case Level.Level36:
                {
                    var bitSetupCase_1 = new Dictionary<int, bool>()
                    {
                        { 14, false },
                        { 15, false },
                    };
                    var bitSetupCase_2 = new Dictionary<int, bool>()
                    {
                        { 14, true },
                        { 15, false },
                    };
                    var bitSetupCase_3 = new Dictionary<int, bool>()
                    {
                        { 14, false },
                        { 15, true },
                    };
                    var bitSetupCase_4 = new Dictionary<int, bool>()
                    {
                        { 14, true },
                        { 15, true },
                    };

                    var stopperContent_1 = new Dictionary<int, Ball>()
                    {
                        { 31, Ball.Red }
                    };
                    var stopperContent_2 = new Dictionary<int, Ball>()
                    {
                        { 31, Ball.Blue }
                    };

                    return new List<SolutionCase> {
                        new SolutionCase(_bitSetupCase: bitSetupCase_1, _stopperContents: stopperContent_1,
                        _resetBallsAfterCase: true),
                        new SolutionCase(_bitSetupCase: bitSetupCase_2, _stopperContents: stopperContent_1,
                        _resetBallsAfterCase: true),
                        new SolutionCase(_bitSetupCase: bitSetupCase_3, _stopperContents: stopperContent_1,
                        _resetBallsAfterCase: true),
                        new SolutionCase(_bitSetupCase: bitSetupCase_4, _stopperContents: stopperContent_2,
                        _resetBallsAfterCase: true),
                    };
                }
            case Level.Level37:
                {
                    var bitRegister_A = new Dictionary<int, bool>() {
                        { 1, false },
                        { 8, false },
                        { 19, false }
                    };
                    var bitRegister_B = new Dictionary<int, bool>() {
                        { 2, false },
                        { 10, false },
                        { 21, false },
                        { 32, false }
                    };

                    List<SolutionCase> solutionCases = new List<SolutionCase>();

                    // Create new solution cases in a loop and add them to the list
                    for (int i = 0; i < 3; i++)
                    {
                        Random random = new Random();

                        int num_A = random.Next(1, 7);
                        int num_B = random.Next(1, 8);

                        var bitSetupCase_A = getBitStatesFromNumber(bitRegister_A.Keys.ToList(), num_A);
                        var bitSetupCase_B = getBitStatesFromNumber(bitRegister_B.Keys.ToList(), num_B);

                        var bitSetupCase = new Dictionary<int, bool>();
                        foreach (var state in bitSetupCase_A)
                        {
                            if (!bitSetupCase.ContainsKey(state.Key))
                            {
                                bitSetupCase[state.Key] = state.Value;
                            }
                        }
                        foreach (var state in bitSetupCase_B)
                        {
                            if (!bitSetupCase.ContainsKey(state.Key))
                            {
                                bitSetupCase[state.Key] = state.Value;
                            }
                        }

                        var bitRegBFinalState = getBitStatesFromNumber(bitRegister_B.Keys.ToList(), num_A + num_B);

                        // Add a copy of the ball output to the solution along with bit final states that change with every loop iteration
                        solutionCases.Add(new SolutionCase(_bitSetupCase: bitSetupCase, _bitFinalStates: bitRegBFinalState,
                            _resetBallsAfterCase: true));
                    }

                    return solutionCases;

                }
            case Level.Level38:
                {
                    var bitRegister = new Dictionary<int, bool>() {
                        { 31, false },
                        { 42, false },
                        { 51, false }
                    };

                    List<SolutionCase> solutionCases = new List<SolutionCase>();
                    var listOfNumbers = new List<int>() { 1, 6, 7 };

                    // Create new solution cases in a loop and add them to the list
                    for (int i = 0; i < 3; i++)
                    {
                        // First blue ball is added because in this task it should be ignowhite by the register
                        var ballOutput = new List<Ball>() { Ball.Blue };

                        // Get some three numbers for the register to check solution on
                        /*Random random = new Random();
                        int num = random.Next(1, 7);*/
                        int num = listOfNumbers[i];
                        string binary = Convert.ToString(num, 2);
                        int amountOfZeros = bitRegister.Count - binary.Length;
                        for (int j = 0; j < amountOfZeros; j++)
                        {
                            binary = "0" + binary;
                        }
                        binary = new string(binary.ToCharArray().Reverse().ToArray());

                        var bitSetupCase = getBitStatesFromNumber(bitRegister.Keys.ToList(), num);

                        // For each register Bit, to the output add a blue ball if a register Bit state it true, white if false
                        for (int c = 0; c < binary.Length; c++)
                        {
                            double state = char.GetNumericValue(binary[c]);
                            ballOutput.Add((state == 1) ? Ball.Blue : Ball.Red);
                        }

                        // Add a copy of the ball output to the solution along with bit final states that change with every loop iteration
                        solutionCases.Add(new SolutionCase(_bitSetupCase: bitSetupCase, _ballOutput: ballOutput,
                            _resetBallsAfterCase: true));
                    }

                    return solutionCases;

                }
            case Level.Level39:
                {
                    var ballOutput = new List<Ball>() {
                        Ball.Blue, Ball.Blue, Ball.Blue, Ball.Red, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Red,
                        Ball.Blue, Ball.Blue, Ball.Blue, Ball.Red, Ball.Blue, Ball.Blue, Ball.Blue, Ball.Red
                    };
                    return new List<SolutionCase> { new SolutionCase(_ballOutput: ballOutput) };
                }
            case Level.Level40:
                {
                    var bitRequiredSetup = new Dictionary<int, bool>() {
                        { 4, false },
                        { 14, false },
                        { 25, false },
                        { 36, false },
                        { 47, false },
                    };

                    List<SolutionCase> solutionCases = new List<SolutionCase>();
                    var ballOutput = new List<Ball>();

                    // Create new solution cases in a loop and add them to the list
                    for (int regValue = 1; regValue < 8; regValue++)
                    {
                        // Add a ball to the output solution case with each iteration
                        ballOutput.Add(Ball.Blue);

                        // Add a copy of the ball output to the solution along with bit final states that change with every loop iteration
                        solutionCases.Add(new SolutionCase(_ballOutput: ballOutput.ToList(), _bitRequiredSetup: bitRequiredSetup,
                        _bitFinalStates: getBitStatesFromNumber(bitRequiredSetup.Keys.ToList(), regValue*4)));
                    }

                    return solutionCases;
                }
            case Level.Level41:
                {
                    // All final states in the register are 0
                    var bitFinalStates = new Dictionary<int, bool>() {
                        { 9, false },
                        { 20, false },
                        { 31, false },
                        { 42, false }
                    };

                    List<SolutionCase> solutionCases = new List<SolutionCase>();
                    var ballOutput = new List<Ball>() { Ball.Blue };
                    var numbers = new List<int>() { 1, 3, 4, 8, 13 };

                    // Create new solution cases in a loop and add them to the list
                    for (int i = 0; i < numbers.Count; i++)
                    {
                        solutionCases.Add(new SolutionCase(_ballOutput: ballOutput.ToList(), _bitFinalStates: bitFinalStates,
                            _bitSetupCase: getBitStatesFromNumber(bitFinalStates.Keys.ToList(), numbers[i]), _resetBallsAfterCase: true));
                    }

                    return solutionCases;
                }
            case Level.Level42:
                {
                    var bitRegister_A = new Dictionary<int, bool>() {
                        { 1, false },
                        { 8, false },
                        { 19, false },
                        { 30, false },
                    };
                    var bitRegister_B = new Dictionary<int, bool>() {
                        { 2, false },
                        { 10, false },
                        { 21, false },
                        { 32, false }
                    };

                    List<SolutionCase> solutionCases = new List<SolutionCase>();

                    var numbers_A = new List<int>() { 2, 3, 1, 12 };
                    var numbers_B = new List<int>() { 2, 2, 2, 0 };

                    var stopperContent_1 = new Dictionary<int, Ball>() { { 47, Ball.Red } };
                    var stopperContent_2 = new Dictionary<int, Ball>() { { 47, Ball.Blue } };

                    // Create new solution cases in a loop and add them to the list
                    for (int i = 0; i < numbers_A.Count; i++)
                    {
                        int num_A = numbers_A[i];
                        int num_B = numbers_B[i];

                        var bitSetupCase_A = getBitStatesFromNumber(bitRegister_A.Keys.ToList(), num_A);
                        var bitSetupCase_B = getBitStatesFromNumber(bitRegister_B.Keys.ToList(), num_B);

                        // Merge these registers into one dictionary of all Bit states
                        var bitSetupCase = new Dictionary<int, bool>();
                        foreach (var state in bitSetupCase_A)
                        {
                            if (!bitSetupCase.ContainsKey(state.Key))
                            {
                                bitSetupCase[state.Key] = state.Value;
                            }
                        }
                        foreach (var state in bitSetupCase_B)
                        {
                            if (!bitSetupCase.ContainsKey(state.Key))
                            {
                                bitSetupCase[state.Key] = state.Value;
                            }
                        }

                        var stopperContent = (num_A > num_B) ? stopperContent_2 : stopperContent_1;

                        // Add a copy of the ball output to the solution along with bit final states that change with every loop iteration
                        solutionCases.Add(new SolutionCase(_bitSetupCase: bitSetupCase, _stopperContents: stopperContent,
                            _resetBallsAfterCase: true));
                    }

                    return solutionCases;

                }
            case Level.Level43:
                {
                    var bitSetupCase_1 = new Dictionary<int, bool>()
                    {
                        { 14, false },
                        { 15, false },
                    };
                    var bitSetupCase_2 = new Dictionary<int, bool>()
                    {
                        { 14, true },
                        { 15, false },
                    };
                    var bitSetupCase_3 = new Dictionary<int, bool>()
                    {
                        { 14, false },
                        { 15, true },
                    };
                    var bitSetupCase_4 = new Dictionary<int, bool>()
                    {
                        { 14, true },
                        { 15, true },
                    };

                    var stopperContent_1 = new Dictionary<int, Ball>()
                    {
                        { 31, Ball.Red }
                    };
                    var stopperContent_2 = new Dictionary<int, Ball>()
                    {
                        { 31, Ball.Blue }
                    };

                    return new List<SolutionCase> {
                        new SolutionCase(_bitSetupCase: bitSetupCase_1, _stopperContents: stopperContent_1,
                        _resetBallsAfterCase: true),
                        new SolutionCase(_bitSetupCase: bitSetupCase_2, _stopperContents: stopperContent_2,
                        _resetBallsAfterCase: true),
                        new SolutionCase(_bitSetupCase: bitSetupCase_3, _stopperContents: stopperContent_2,
                        _resetBallsAfterCase: true),
                        new SolutionCase(_bitSetupCase: bitSetupCase_4, _stopperContents: stopperContent_1,
                        _resetBallsAfterCase: true),
                    };
                }
            case Level.Level44:
                {
                    var bitRegister_A = new Dictionary<int, bool>() {
                        { 1, false },
                        { 8, false },
                        { 19, false }
                    };
                    var bitRegister_B = new Dictionary<int, bool>() {
                        { 2, false },
                        { 10, false },
                        { 21, false },
                        { 32, false }
                    };

                    List<SolutionCase> solutionCases = new List<SolutionCase>();
                    var numbers_A = new List<int>() { 1, 4, 7, 7 };
                    var numbers_B = new List<int>() { 15, 8, 10, 7 };

                    // Create new solution cases in a loop and add them to the list
                    for (int i = 0; i < numbers_A.Count; i++)
                    {
                        Random random = new Random();

                        int num_A = numbers_A[i];
                        int num_B = numbers_B[i];

                        var bitSetupCase_A = getBitStatesFromNumber(bitRegister_A.Keys.ToList(), num_A);
                        var bitSetupCase_B = getBitStatesFromNumber(bitRegister_B.Keys.ToList(), num_B);

                        var bitSetupCase = new Dictionary<int, bool>();
                        foreach (var state in bitSetupCase_A)
                        {
                            if (!bitSetupCase.ContainsKey(state.Key))
                            {
                                bitSetupCase[state.Key] = state.Value;
                            }
                        }
                        foreach (var state in bitSetupCase_B)
                        {
                            if (!bitSetupCase.ContainsKey(state.Key))
                            {
                                bitSetupCase[state.Key] = state.Value;
                            }
                        }

                        var bitRegBFinalState = getBitStatesFromNumber(bitRegister_B.Keys.ToList(), num_A - num_B);

                        // Add a copy of the ball output to the solution along with bit final states that change with every loop iteration
                        solutionCases.Add(new SolutionCase(_bitSetupCase: bitSetupCase, _bitFinalStates: bitRegBFinalState,
                            _resetBallsAfterCase: true));
                    }

                    return solutionCases;

                }
            case Level.Level45:
                {
                    var bitRegister_A = new Dictionary<int, bool>() {
                        { 13, false },
                        { 24, false },
                        { 35, false }
                    };
                    var bitRegister_B = new Dictionary<int, bool>() {
                        { 15, false },
                        { 26, false },
                        { 37, false }
                    };

                    List<SolutionCase> solutionCases = new List<SolutionCase>();

                    var numbers_A = new List<int>() { 1, 3, 3, 7 };
                    var numbers_B = new List<int>() { 1, 7, 2, 3 };

                    var stopperContent_1 = new Dictionary<int, Ball>() { { 51, Ball.Red } };
                    var stopperContent_2 = new Dictionary<int, Ball>() { { 51, Ball.Blue } };

                    // Create new solution cases in a loop and add them to the list
                    for (int i = 0; i < numbers_A.Count; i++)
                    {
                        int num_A = numbers_A[i];
                        int num_B = numbers_B[i];

                        var bitSetupCase_A = getBitStatesFromNumber(bitRegister_A.Keys.ToList(), num_A);
                        var bitSetupCase_B = getBitStatesFromNumber(bitRegister_B.Keys.ToList(), num_B);

                        // Merge these registers into one dictionary of all Bit states
                        var bitSetupCase = new Dictionary<int, bool>();
                        foreach (var state in bitSetupCase_A)
                        {
                            if (!bitSetupCase.ContainsKey(state.Key))
                            {
                                bitSetupCase[state.Key] = state.Value;
                            }
                        }
                        foreach (var state in bitSetupCase_B)
                        {
                            if (!bitSetupCase.ContainsKey(state.Key))
                            {
                                bitSetupCase[state.Key] = state.Value;
                            }
                        }

                        var stopperContent = (num_A <= num_B) ? stopperContent_2 : stopperContent_1;

                        // Add a copy of the ball output to the solution along with bit final states that change with every loop iteration
                        solutionCases.Add(new SolutionCase(_bitSetupCase: bitSetupCase, _stopperContents: stopperContent,
                            _resetBallsAfterCase: true));
                    }

                    return solutionCases;

                }
            case Level.Level46:
                {
                    var bitSetupCase = new Dictionary<int, bool>() {
                        { 40, false },
                        { 41, false },
                        { 42, false },
                        { 43, false },
                        { 44, false }
                    };

                    List<SolutionCase> solutionCases = new List<SolutionCase>();

                    // Create new solution cases in a loop and add them to the list
                    for (int i = 0; i < 3; i++)
                    {
                        Random random = new Random();

                        // Create a random setup of the Bits
                        for (int j = 0; j < 5; j++)
                        {
                            int index = random.Next(bitSetupCase.Count);
                            KeyValuePair<int, bool> pair = bitSetupCase.ElementAt(index);
                            bitSetupCase[pair.Key] = !pair.Value;
                        }
                        var newBitSetupCase = new Dictionary<int, bool>(bitSetupCase);

                        solutionCases.Add(new SolutionCase(_bitSetupCase: newBitSetupCase,
                        _bitFinalStates: getOppositeBitStates(bitSetupCase), _resetBallsAfterCase: true));
                    }

                    return solutionCases;
                }
            case Level.Level47:
                {
                    var bitRegister_A = new Dictionary<int, bool>() {
                        { 10, false },
                        { 21, false },
                    };
                    var bitRegister_B = new Dictionary<int, bool>() {
                        { 20, false },
                        { 31, false },
                        { 42, false }
                    };

                    List<SolutionCase> solutionCases = new List<SolutionCase>();

                    var numbers_A = new List<int>() { 1, 2, 3, 3 };
                    var numbers_B = new List<int>() { 1, 4, 0, 4 };

                    // Create new solution cases in a loop and add them to the list
                    for (int i = 0; i < 3; i++)
                    {
                        int num_A = numbers_A[i];
                        int num_B = numbers_B[i];

                        var bitSetupCase_A = getBitStatesFromNumber(bitRegister_A.Keys.ToList(), num_A);
                        var bitSetupCase_B = getBitStatesFromNumber(bitRegister_B.Keys.ToList(), num_B);

                        var bitSetupCase = new Dictionary<int, bool>();
                        foreach (var state in bitSetupCase_A)
                        {
                            if (!bitSetupCase.ContainsKey(state.Key))
                            {
                                bitSetupCase[state.Key] = state.Value;
                            }
                        }
                        foreach (var state in bitSetupCase_B)
                        {
                            if (!bitSetupCase.ContainsKey(state.Key))
                            {
                                bitSetupCase[state.Key] = state.Value;
                            }
                        }

                        var bitRegBFinalState = getBitStatesFromNumber(bitRegister_B.Keys.ToList(), num_A + num_B);

                        // Add a copy of the ball output to the solution along with bit final states that change with every loop iteration
                        solutionCases.Add(new SolutionCase(_bitSetupCase: bitSetupCase, _bitFinalStates: bitRegBFinalState,
                            _resetBallsAfterCase: true));
                    }

                    return solutionCases;

                }
            default:
                Debug.LogWarning("This level does not have any solution!");
                return new List<SolutionCase> { new SolutionCase() };
        }
    }

    private Dictionary<int, bool> getBitStatesFromNumber(List<int> regBitPositions, int regValue)
    {
        var bitFinalStates = new Dictionary<int, bool>();

        // Make sure register bit positions are sorted
        regBitPositions.Sort();

        // Convert value given in parameters to binary
        string binary = Convert.ToString(regValue, 2);

        // Make sure the string is long enough (add 0s at the beginning if shorter than register length)
        int amountOfZeros = regBitPositions.Count - binary.Length;
        for(int i = 0; i < amountOfZeros; i++)
        {
            binary = "0" + binary;
        }
        binary = new string(binary.ToCharArray().Reverse().ToArray());

        // Add bit states to the dictionary based on this binary number
        for (int i = 0; i < binary.Length; i++)
        {
            double num = char.GetNumericValue(binary[i]);
            bool state = (num == 1) ? true : false;
            bitFinalStates.Add(regBitPositions[i], state);
        }

        return bitFinalStates;
    }


    private Dictionary<int, bool> getOppositeBitStates(Dictionary<int, bool> inputStates)
    {
        var outputStates = new Dictionary<int, bool>();
        foreach(KeyValuePair<int, bool> state in inputStates)
        {
            outputStates.Add(state.Key, !state.Value);
        }
        return outputStates;
    }




}
