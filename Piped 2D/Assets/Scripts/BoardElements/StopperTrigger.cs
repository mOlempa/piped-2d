using UnityEngine;

/**
 * A class responsible for handling the Stopper trigger
 */
public class StopperTrigger : MonoBehaviour
{
    /**
     * A private void method called when another object enters the trigger.
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Don't check anything if the actual marbles are not running yet
        if (!GameManager.Instance.marblesAreRunning)
        {
            return;
        }

        int cellNum = LevelManager.Instance.posToCellDictionary[gameObject.transform.position];
        if (other.CompareTag("BlueBall"))
        {
            if (GameManager.Instance.currentStopperContents.ContainsKey(0))
            {
                GameManager.Instance.currentStopperContents[0] = Ball.Blue;
            }
            else
            {
                GameManager.Instance.currentStopperContents[cellNum] = Ball.Blue;
            }
            GameManager.Instance.caseFinished = true;
        }
        if (other.CompareTag("RedBall"))
        {
            if (GameManager.Instance.currentStopperContents.ContainsKey(0))
            {
                GameManager.Instance.currentStopperContents[0] = Ball.Red;
            }
            else {
                GameManager.Instance.currentStopperContents[cellNum] = Ball.Red;
            }
            GameManager.Instance.caseFinished = true;
        }
    }
}
