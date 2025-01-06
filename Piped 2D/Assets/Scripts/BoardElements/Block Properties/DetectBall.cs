using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DetectBall : MonoBehaviour
{
    [SerializeField]
    GameObject mainObject;

    int cell = 0;

    private void Start()
    {
        cell = LevelManager.Instance.posToCellDictionary[mainObject.transform.position];
    }


    // Detect the ball entering and leaving the block for ball's ignoreFall control
    private void OnTriggerEnter2D(Collider2D other)
    {
        detectBallEnterBlock(other.gameObject, cell);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        detectBallExitBlock(other.gameObject, cell);
    }

    public void detectBallEnterBlock(GameObject gameObject, int blockCell)
    {
        if (gameObject.CompareTag("BlueBall") || gameObject.CompareTag("RedBall"))
        {
            var script = gameObject.GetComponent<BallScript>();
            script.handleBlockEnter(blockCell);
        }
    }

    public void detectBallExitBlock(GameObject gameObject, int blockCell)
    {
        if (gameObject.CompareTag("BlueBall") || gameObject.CompareTag("RedBall"))
        {
            var script = gameObject.GetComponent<BallScript>();
            script.handleBlockExit(blockCell);
        }
    }
}
