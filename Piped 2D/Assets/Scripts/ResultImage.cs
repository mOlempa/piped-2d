using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultImage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Image resultImage = GetComponent<Image>();
        resultImage.SetNativeSize();
        gameObject.transform.localScale = Vector3.one * 0.5f;
    }
}
