using UnityEngine;

public class CrossoverTriggers : MonoBehaviour
{

    public GameObject RightLeftPath;
    public GameObject LeftRightPath;

    public bool ballInObj = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("BlueBall") || other.gameObject.CompareTag("RedBall"))
        {
            if (gameObject.CompareTag("LeftTrigger"))
            {
                Vector3 pos = RightLeftPath.transform.localPosition;
                if (!ballInObj)
                {
                    ballInObj = true;
                    gameObject.GetComponent<PolygonCollider2D>().enabled = true;
                    RightLeftPath.transform.localPosition = new Vector3(pos.x, pos.y, -2);

                }
                else
                {
                    ballInObj = false;
                    gameObject.GetComponent<PolygonCollider2D>().enabled = false;
                    RightLeftPath.transform.localPosition = new Vector3(pos.x, pos.y, 0);
                }
            }

            if (gameObject.CompareTag("RightTrigger"))
            {
                if (!ballInObj)
                {
                    ballInObj = true;
                    gameObject.GetComponent<PolygonCollider2D>().enabled = true;
                }
                else
                {
                    ballInObj = false;
                    gameObject.GetComponent<PolygonCollider2D>().enabled = false;
                }
            }
        }
        
    }
}
