using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputWheel : MonoBehaviour
{
    int deg = 0;
    public int num = 1;

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
