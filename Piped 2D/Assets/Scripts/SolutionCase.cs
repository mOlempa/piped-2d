using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionCase
{
    public List<Ball> ballOutput;
    public Dictionary<int, bool> bitFinalStates;
    public Dictionary<int, bool> bitSetupCase;
    // If it exists in a level, required setup is the same for all cases of a solution
    public Dictionary<int, bool> bitRequiredSetup;
    public Dictionary<int, Ball> stopperContents;
    public Ball startingSideCase;

    public bool resetBallsAfterCase;

    public SolutionCase(List<Ball> _ballOutput = null, Dictionary<int, bool> _bitRequiredSetup = null,
        Dictionary<int, bool> _bitSetupCase = null, Dictionary<int, bool> _bitFinalStates = null,
        Dictionary<int, Ball> _stopperContents = null, bool _resetBallsAfterCase = false, Ball _startingSideCase = Ball.Neither)
    {
        if (_ballOutput != null)
        {
            ballOutput = _ballOutput;
        }
        else
        {
            ballOutput = new List<Ball>();
        }

        if (_bitRequiredSetup != null)
        {
            bitRequiredSetup = _bitRequiredSetup;
        }
        else
        {
            bitRequiredSetup = new Dictionary<int, bool>();
        }

        if (_bitSetupCase != null)
        {
            bitSetupCase = _bitSetupCase;
        }
        else
        {
            bitSetupCase = new Dictionary<int, bool>();
        }

        if (_bitFinalStates != null)
        {
            bitFinalStates = _bitFinalStates;
        }
        else
        {
            bitFinalStates = new Dictionary<int, bool>();
        }

        if (_stopperContents != null)
        {
            stopperContents = _stopperContents;
        }
        else
        {
            stopperContents = new Dictionary<int, Ball>();
        }

        this.resetBallsAfterCase = _resetBallsAfterCase;
        this.startingSideCase = _startingSideCase;
    }

}
