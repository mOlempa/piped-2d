using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGearRotation : MonoBehaviour
{
    float angle = 0;
    public float value = 70f;

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        angle += value * Time.deltaTime;
        if (!AudioManager.Instance.sfxSource.isPlaying)
        {
            AudioManager.Instance.playSfx("MenuGears");
        }
    }

    private void OnDestroy()
    {
        if(AudioManager.Instance.sfxSource != null)
            AudioManager.Instance.sfxSource.Stop();
    }
}
