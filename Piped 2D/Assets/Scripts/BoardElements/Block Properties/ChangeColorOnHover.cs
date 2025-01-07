using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChangeColorOnHover : MonoBehaviour
{
    Color highlightColor = new Color(0.8f, 0.6f, 1);
    List<SpriteRenderer> spriteRenderers;

    private void Start()
    {
        spriteRenderers = gameObject.transform.GetComponentsInChildren<SpriteRenderer>(false).ToList();
        spriteRenderers.Add(gameObject.GetComponent<SpriteRenderer>());
    }

    private void OnMouseEnter()
    {
        if (GameManager.Instance.marblesAreRunning ||
            LevelManager.Instance.lockedCellCenters.Contains(gameObject.transform.position))
        {
            return;
        }
        changeSpriteColor(highlightColor);
    }

    private void OnMouseExit()
    {
        if (GameManager.Instance.marblesAreRunning ||
            LevelManager.Instance.lockedCellCenters.Contains(gameObject.transform.position))
        {
            return;
        }
        changeSpriteColor(Color.white);
    }


    private void changeSpriteColor(Color color)
    {
        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.material.color = color;
        }
    }
}
