using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitEnterTriggers : MonoBehaviour
{
    public GameObject blockingCollider;
    public Ramp rampScript;

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

            
            //rampScript.setReferenceForConnectedRamps();
        }
    }
}
