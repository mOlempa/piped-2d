using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI loadingText;

    List<string> texts = new List<string>() { "Loading", "Loading.", "Loading..", "Loading..."};
    int counter = 0;

    private void Start()
    {
        StartCoroutine(textAnimation());
    }

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
