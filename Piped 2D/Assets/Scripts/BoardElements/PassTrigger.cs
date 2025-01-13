using System.Collections;
using UnityEngine;

/**
 * A class responsible for handling the Pass trigger
 */
public class PassTrigger : MonoBehaviour
{
    /**
    * A private GameObject field for referencing the blocking collider that block the marbles from falling out on the other side.
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    GameObject blockingCollider;

    /**
     * A private void method called when another object enters the trigger.
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("BlueBall") || other.gameObject.CompareTag("RedBall"))
        {
            StartCoroutine(enableCollider());
        }
    }

    /**
     * A private coroutine for enabling and disabling the blocking collider.
     */
    IEnumerator enableCollider()
    {
        blockingCollider.SetActive(true);
        yield return new WaitForSeconds(1);
        blockingCollider.SetActive(false);
    }
}
