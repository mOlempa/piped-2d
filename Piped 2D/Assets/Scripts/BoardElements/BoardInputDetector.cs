using UnityEngine;

public class BoardInputDetector : MonoBehaviour
{
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
