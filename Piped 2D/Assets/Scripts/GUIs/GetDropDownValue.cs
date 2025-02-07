using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetDropDownValue : MonoBehaviour
{
    [SerializeField]
    TMP_Dropdown languageDropDown;

    public void changeLanguage()
    {
        print(languageDropDown.value);
    }
}
