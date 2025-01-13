using System.Collections;
using UnityEngine;

/**
 * A class responsible for output of the board.
 */
public class OutputRamp : MonoBehaviour
{
    /**
     * A private bool field that decides whether the marbles in the output are to be stored or not.
     */
    bool countBalls = true;

    /**
     * A private void method called when another object enters the trigger.
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (countBalls)
        {
            if (other.CompareTag("RedBall"))
            {
                GameManager.Instance.addBallToCurrentBallOutput(Ball.Red);
            }
            if (other.CompareTag("BlueBall"))
            {
                GameManager.Instance.addBallToCurrentBallOutput(Ball.Blue);
            }
            StartCoroutine(wait());
        }
    }

    /**
     * A private coroutine that waits before counting another marble.
     */
    IEnumerator wait()
    {
        countBalls = false;
        yield return new WaitForSeconds(3);
        countBalls = true;
    }
}
