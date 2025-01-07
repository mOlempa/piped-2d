using System.Collections;
using UnityEngine;

public class OutputRamp : MonoBehaviour
{
    bool countBalls = true;

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
    IEnumerator wait()
    {
        countBalls = false;
        yield return new WaitForSeconds(3);
        countBalls = true;
    }
}
