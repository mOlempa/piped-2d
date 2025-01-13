using UnityEngine;

/**
 * A class responsible for dispenser wheel behaviour.
 */
public class DispenserWheel : MonoBehaviour
{
    /**
     * A private int field that describes the amount of degrees the wheel has currently,
     */
    int deg = 0;

    /**
     * A private int field that describes the amount of degrees the wheel will turn at a time.
     */
    public int num = 1;

    /**
     * A private bool field describing whether the wheel sound was already played or not.
     */
    bool playedSound = false;

    /**
    * A private void method called at a measured time step.
    */
    void FixedUpdate()
    {
        if (gameObject.CompareTag("LeftWheel"))
        {
            if (GameManager.Instance.turnLeftWheels || deg % 90 != 0)
            {
                deg = deg - num;
                transform.rotation = Quaternion.Euler(Vector3.forward * deg);

                if(playedSound == false)
                {
                    playWheelsSfx();
                    playedSound = true;
                }
            }
            else
            {
                if(playedSound)
                {
                    playedSound = false;
                }
            }
        }
        if (gameObject.CompareTag("RightWheel"))
        {
            if (GameManager.Instance.turnRightWheels || deg % 90 != 0)
            {
                deg = deg + num;
                transform.rotation = Quaternion.Euler(Vector3.forward * deg);

                if (playedSound == false)
                {
                    playWheelsSfx();
                    playedSound = true;
                }
            }
            else
            {
                if (playedSound)
                {
                    playedSound = false;
                }
            }
        }
    }

    /**
    * A private void method for playing a wheel rotating sound effect
    */
    void playWheelsSfx()
    {
        AudioManager.Instance.playSfx("Wheels");
    }
}
