using UnityEngine;

/**
 * A class that handles detecting if the marbles are inside the block, used for destroying the marble if it falls out of path.
 */
public class DetectBall : MonoBehaviour
{
    /**
     * A private GameObject field that references the main object of the block, used for getting the trigger covering the whole block area.
     * Serialized in order to access it from the Unity editor.
     */
    [SerializeField]
    GameObject mainObject;

    /**
     * A private int field that stores the board cell number the block is in.
     */
    int cell = 0;

    /**
    * A private void method called when the script instance is being loaded. It sets necessary variables.
    */
    private void Start()
    {
        cell = LevelManager.Instance.posToCellDictionary[mainObject.transform.position];
    }

    /**
     * A private void method that gets called when another object enters the trigger attached to this object.
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        detectBallEnterBlock(other.gameObject, cell);
    }

    /**
     * A private void method that gets called when another object exits the trigger attached to this object.
     */
    private void OnTriggerExit2D(Collider2D other)
    {
        detectBallExitBlock(other.gameObject, cell);
    }

    /**
     * A public void method handling the marble entering the block.
     */
    public void detectBallEnterBlock(GameObject gameObject, int blockCell)
    {
        if (gameObject.CompareTag("BlueBall") || gameObject.CompareTag("RedBall"))
        {
            var script = gameObject.GetComponent<BallScript>();
            script.handleBlockEnter(blockCell);
        }
    }

    /**
     * A public void method handling the marble exiting the block.
     */
    public void detectBallExitBlock(GameObject gameObject, int blockCell)
    {
        if (gameObject.CompareTag("BlueBall") || gameObject.CompareTag("RedBall"))
        {
            var script = gameObject.GetComponent<BallScript>();
            script.handleBlockExit(blockCell);
        }
    }
}
