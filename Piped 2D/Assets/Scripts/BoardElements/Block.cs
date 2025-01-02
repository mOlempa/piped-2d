using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    public Color highlightColor = new Color(0.8f, 0.6f, 1);

    public void detectBallEnterBlock(GameObject gameObject, int blockCell)
    {
        if (gameObject.CompareTag("BlueBall") || gameObject.CompareTag("RedBall"))
        {
            var script = gameObject.GetComponent<BallScript>();
            script.handleBlockEnter(blockCell);
        }
    }

    public void detectBallExitBlock(GameObject gameObject, int blockCell)
    {
        if (gameObject.CompareTag("BlueBall") || gameObject.CompareTag("RedBall"))
        {
            var script = gameObject.GetComponent<BallScript>();
            script.handleBlockExit(blockCell);
        }
    }


    public void destroyBlock(GameObject gameObject)
    {
        GameManager.Instance.availableRegularCellCenters.Add(gameObject.transform.position);
        Object.Destroy(gameObject);
        Debug.Log("<color=grey>Block deleted");
        GameManager.Instance.increaseCounter(GameManager.Instance.getObjTypeByTag(gameObject.tag));
        AudioManager.Instance.playRandomBlockPlaceSfx();
    }
}
