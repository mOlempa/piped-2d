using UnityEngine;

/**
 * A class handling the trigger events of the Crossover triggers.
 */
public class CrossoverTriggers : MonoBehaviour
{
    /**
     * A public GameObject field for referencing the right-to-left path through the Crossover block.
     */
    public GameObject RightLeftPath;

    /**
     * A public GameObject field for referencing the left-to-right path through the Crossover block.
     */
    public GameObject LeftRightPath;

    /**
     * A public bool that gets set whenever a marble enters (or reset when marble leaves) the Crossover
     */
    public bool ballInObj = false;

    /**
     * A private void method called when another object enters the trigger.
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("BlueBall") || other.gameObject.CompareTag("RedBall"))
        {
            if (gameObject.CompareTag("LeftTrigger"))
            {
                Vector3 pos = RightLeftPath.transform.localPosition;
                if (!ballInObj)
                {
                    ballInObj = true;
                    gameObject.GetComponent<PolygonCollider2D>().enabled = true;
                    RightLeftPath.transform.localPosition = new Vector3(pos.x, pos.y, -2);

                }
                else
                {
                    ballInObj = false;
                    gameObject.GetComponent<PolygonCollider2D>().enabled = false;
                    RightLeftPath.transform.localPosition = new Vector3(pos.x, pos.y, 0);
                }
            }

            if (gameObject.CompareTag("RightTrigger"))
            {
                if (!ballInObj)
                {
                    ballInObj = true;
                    gameObject.GetComponent<PolygonCollider2D>().enabled = true;
                }
                else
                {
                    ballInObj = false;
                    gameObject.GetComponent<PolygonCollider2D>().enabled = false;
                }
            }
        }
        
    }
}
