using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * A class handling the loading text screen animation.
 */
public class LoadingScreen : MonoBehaviour
{
    /**
    * A private TextMeshProUGUI field for referencing the text on the screen
    * Serialized in order to access it from the Unity editor.
    */
    [SerializeField]
    TextMeshProUGUI loadingText;

    /**
    * A private List field defining the text values.
    */
    List<string> texts = new List<string>() { "Loading", "Loading.", "Loading..", "Loading..."};

    /**
    * A private int field that serves as a counter
    */
    int counter = 0;

    /**
     * A private void method called at the start of the scene.
     */
    private void Start()
    {
        StartCoroutine(textAnimation());
    }

    /**
     * A coroutine responsible for the text animation.
     */
    IEnumerator textAnimation()
    {
        while (true)
        {
            loadingText.text = texts[(counter%4)];
            yield return new WaitForSeconds(0.1f);
            counter++;
            if (counter % 4 == 0)
            {
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    
}
