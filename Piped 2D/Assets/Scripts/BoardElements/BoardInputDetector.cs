using UnityEngine;

/**
 * A class handling the amount of marbles that will get respawned after restarting the board.
 */
public class BoardInputDetector : MonoBehaviour
{
    /**
     * A private void method called when another object enters the trigger.
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("BlueBall") || other.gameObject.CompareTag("RedBall"))
        {
            Ball ballType = other.gameObject.CompareTag("BlueBall") ? Ball.Blue : Ball.Red;

            GameManager.Instance.ballAmountToRespawn[ballType] += 1;
            GameManager.Instance.ballsToRespawn.Add(other.gameObject);
        }
    }
}
