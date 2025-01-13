using UnityEngine;
/**
 * 
 * A class responsible for output wheel behaviour.
 */
public class OutputWheel : MonoBehaviour
{
    /**
     * A private int field that describes the amount of degrees the wheel has currently.
     */
    int deg = 0;

    /**
     * A private int field that describes the amount of degrees the wheel will turn at a time.
     */
    public int num = 1;

    /**
    * A private void method called at a measured time step.
    */
    void FixedUpdate()
    {
        if (gameObject.CompareTag("LeftWheel"))
        {
            if (GameManager.Instance.turnLeftWheels)
            {
                deg = deg - num;
                transform.rotation = Quaternion.Euler(Vector3.forward * deg);
                if (deg % 90 == 0)
                {
                    GameManager.Instance.turnLeftWheels = false;
                }
            }
        }
        if (gameObject.CompareTag("RightWheel"))
        {
            if (GameManager.Instance.turnRightWheels)
            {
                deg = deg + num;
                transform.rotation = Quaternion.Euler(Vector3.forward * deg);
                if (deg % 90 == 0)
                {
                    GameManager.Instance.turnRightWheels = false;
                }
            }
        }
    }

    /**
     * A private void method called when another object enters the trigger.
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Do not let broken marbles or test marbles trigger the wheels
        if (other.gameObject.CompareTag("BrokenBall") || GameManager.Instance.marblesAreRunning == false)
        {
            return;
        }
        if (gameObject.CompareTag("RightWheel"))
        {
            GameManager.Instance.turnRightWheels = true;
        }
        if (gameObject.CompareTag("LeftWheel"))
        {
            GameManager.Instance.turnLeftWheels = true;
        }
        
    }
}
