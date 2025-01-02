using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenserWheel : MonoBehaviour
{
    int deg = 0;
    public int num = 1;

    bool playedSound = false;

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

    void playWheelsSfx()
    {
        AudioManager.Instance.playSfx("Wheels");
    }
}
