using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class Block : MonoBehaviour
{
    public Color highlightColor = new Color(0.8f, 0.6f, 1);
    List<SpriteRenderer> spriteRenderers;
    int cell = 0;

    private void Start()
    {
        cell = LevelManager.Instance.posToCellDictionary[gameObject.transform.position];
        spriteRenderers = gameObject.transform.GetComponentsInChildren<SpriteRenderer>(false).ToList();
        spriteRenderers.Add(gameObject.GetComponent<SpriteRenderer>());
    }

    // Detect the ball entering and leaving the block for ball's ignoreFall control
    private void OnTriggerEnter2D(Collider2D other)
    {
        detectBallEnterBlock(other.gameObject, cell);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        detectBallExitBlock(other.gameObject, cell);
    }

    private void OnMouseOver()
    {
        // Mouse right-click to remove the object
        if (Input.GetMouseButtonDown(1))
        {
            destroyBlock(gameObject);
        }
    }

    private void OnMouseEnter()
    {
        changeSpriteColor(highlightColor);
    }

    private void OnMouseExit()
    {
        changeSpriteColor(Color.white);
    }


    private void changeSpriteColor(Color color)
    {
        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.material.color = color;
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


    


}
