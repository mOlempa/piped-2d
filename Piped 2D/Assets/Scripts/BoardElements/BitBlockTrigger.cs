using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitBlockTrigger : MonoBehaviour
{
    int cell = 0;
    private void Start()
    {
        cell = LevelManager.Instance.posToCellDictionary[gameObject.transform.parent.position];
    }


    // Detect the ball entering and leaving the Bit for ball's ignoreFall control
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BlueBall") || other.gameObject.CompareTag("RedBall"))
        {
            //print("<color=green>Entering Trigger --> no damage");
            other.gameObject.GetComponent<BallScript>().ignoreFall = true;
            var script = other.gameObject.GetComponent<BallScript>();

            if (!script.blocksBallIsIn.Contains(cell))
            {
                script.blocksBallIsIn.Add(cell);
            }

            script.ignoreFall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BlueBall") || other.gameObject.CompareTag("RedBall"))
        {
            if (gameObject.CompareTag("Bit"))
            {
                print("<color=red>Exiting Bit");
            }
            var script = other.gameObject.GetComponent<BallScript>();

            if (script.blocksBallIsIn.Contains(cell))
            {
                script.blocksBallIsIn.Remove(cell);
                if (script.blocksBallIsIn.Count == 0)
                {
                    if (!script.isAtOutput)
                    {
                        script.ignoreFall = false;
                    }
                }
            }

        }
    }
}
