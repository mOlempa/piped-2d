using UnityEngine;

/**
 * A class controlling the behaviour of the bit triggers on the entrance of the Bit.
 */
public class BitEnterTriggers : MonoBehaviour
{
    /**
     * A public GameObject field for referencing a collider that blocks the marble from falling out on the other side of the Bit.
     */
    public GameObject blockingCollider;

    /**
     * A public GameObject field for referencing the script of the ramp of the Bit.
     */
    public Ramp rampScript;

    /**
    * A private void method called when another object enters the trigger. It detects the marbles entering the Bit.
    */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BlueBall") || other.CompareTag("RedBall"))
        {
            if (gameObject.CompareTag("LeftTrigger"))
            {
                blockingCollider.transform.localPosition = new Vector2(0.12f, 0.3f);
            }

            if (gameObject.CompareTag("RightTrigger"))
            {
                blockingCollider.transform.localPosition = new Vector2(-0.12f, 0.3f);
            }

        }
    }
}
