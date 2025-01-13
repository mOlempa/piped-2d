using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * A class that handles the Level Complete text feedback.
 */
public class LevelCompleteText : MonoBehaviour
{
    /**
     * A private TextMeshProUGUI field for referencing the text.
     */
    private TextMeshProUGUI text;

    /**
     * A private Color field for referencing the base color of the text.
     */
    private Color baseColor;

    /**
     * A private Color field used in animating the text.
     */
    private Color color_0 = new Color();

    /**
     * A private Color field used in animating the text.
     */
    private Color color_1 = new Color();

    /**
     * A public int field defining the rate of the text animation pulsation.
     */
    public int rate = 1;

    /**
     * A private float field for animation counter.
     */
    float i = 0;

    /**
    * A private void method called when the script instance is being loaded. It sets necessary variables.
    */
    void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
        baseColor = text.color;
        color_1 = text.color;
        color_0 = color_1;
        color_0.a = 0;
        text.color = color_0;
        text.enabled = false;
    }

    /**
    * A private void method called every frame.
    */
    void Update()
    {
        if (GameManager.Instance.levelCompleted)
        {
            if (!text.enabled)
            {
                text.enabled = true;
            }
            else
            {
                i += Time.deltaTime * rate;
                text.color = Color.Lerp(color_0, color_1, Mathf.PingPong(i * 2, 1));
                // If we've got to the current target colour, choose a new one
                if (i >= 1)
                {
                    i = 0;
                    color_1 = baseColor;
                    color_0 = color_1;
                    color_1.a = 0;
                }
            }


        }
        else
        {
            if (text.enabled)
            {
                text.enabled = false;
            }
        }

    }

}
