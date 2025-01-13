using System.Collections.Generic;

public class SolutionCase
{
    /**
     * A public List storing the ball output for the solution case.
     */
    public List<Ball> ballOutput;

    /**
     * A public Dictionary storing the final states of the Bits for the solution case.
     */
    public Dictionary<int, bool> bitFinalStates;

    /**
     * A public Dictionary storing the setup states of the Bits for the solution case.
     */
    public Dictionary<int, bool> bitSetupCase;

    /**
     * A public Dictionary storing the required setup states of the Bits for the solution case.
     */
    public Dictionary<int, bool> bitRequiredSetup;

    /**
     * A public Dictionary storing the stopper contents for the solution case.
     */
    public Dictionary<int, Ball> stopperContents;

    /**
     * A public Ball field storing the starting dispenser for the solution case.
     */
    public Ball startingSideCase;

    /**
     * A public bool for the decision about resetting the marbles after each case check.
     */
    public bool resetBallsAfterCase;

    /**
     * A public SolutionCase constructor.
     */
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
