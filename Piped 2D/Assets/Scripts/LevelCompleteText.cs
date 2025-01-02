using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelCompleteText : MonoBehaviour
{
    private TextMeshProUGUI text;
    private Color baseColor;
    private Color color_0 = new Color();
    private Color color_1 = new Color();
    public int rate = 1;
    float i = 0;


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

    public float fadeTime = 10f; 


}
