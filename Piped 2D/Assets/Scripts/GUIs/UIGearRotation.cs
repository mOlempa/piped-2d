using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A class responsible for the rotation of the gears in the Main Menu.
 */
public class UIGearRotation : MonoBehaviour
{
    /**
     * A private float field that describes the amount of degrees the gear has currently.
     */
    float angle = 0;

    /**
     * A public float field that describes the multiplier for the rotation.
     */
    public float value = 70f;

    /**
    * A private void method called at a measured time step.
    */
    void FixedUpdate()
    {
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        angle += value * Time.deltaTime;
        if (!AudioManager.Instance.sfxSource.isPlaying)
        {
            AudioManager.Instance.playSfx("MenuGears");
        }
    }

    /**
    * A private void method called on destroying the object.
    */
    private void OnDestroy()
    {
        if(AudioManager.Instance.sfxSource != null)
            AudioManager.Instance.sfxSource.Stop();
    }
}
