using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * A class that controls changing color of the sprites.
 */
public class ChangeColorOnHover : MonoBehaviour
{
    /**
     * A private Color field defining the color of the highlight on mouse hover.
     */
    Color highlightColor = new Color(0.8f, 0.6f, 1);

    /**
     * A private List of SpriteRenderers field that stores the sprites of the object and its children.
     */
    List<SpriteRenderer> spriteRenderers;

    /**
    * A private void method called when the script instance is being loaded. It sets necessary variables.
    */
    private void Start()
    {
        spriteRenderers = gameObject.transform.GetComponentsInChildren<SpriteRenderer>(false).ToList();
        spriteRenderers.Add(gameObject.GetComponent<SpriteRenderer>());
    }

    /**
     * A private void method called when the mouse cursor enters the object.
     */
    private void OnMouseEnter()
    {
        if (GameManager.Instance.marblesAreRunning ||
            LevelManager.Instance.lockedCellCenters.Contains(gameObject.transform.position))
        {
            return;
        }
        changeSpriteColor(highlightColor);
    }

    /**
     * A private void method called when the mouse cursor exits the object
     */
    private void OnMouseExit()
    {
        if (GameManager.Instance.marblesAreRunning ||
            LevelManager.Instance.lockedCellCenters.Contains(gameObject.transform.position))
        {
            return;
        }
        changeSpriteColor(Color.white);
    }

    /**
     * A private void method that changes the color of the sprites of the object and its children.
     */
    private void changeSpriteColor(Color color)
    {
        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.material.color = color;
        }
    }
}
