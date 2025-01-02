using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject blockingCollider;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("BlueBall") || other.gameObject.CompareTag("RedBall"))
        {
            StartCoroutine(enableCollider());
        }
    }

    IEnumerator enableCollider()
    {
        blockingCollider.SetActive(true);
        yield return new WaitForSeconds(1);
        blockingCollider.SetActive(false);
    }
}
