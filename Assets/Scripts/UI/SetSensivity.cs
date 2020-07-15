using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSensivity : MonoBehaviour
{
    public Scrollbar _scrollbar;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Sensivity"))
            _scrollbar.value = PlayerPrefs.GetFloat("Sensivity");
        else
            _scrollbar.value = 0.5f;
    }

    public void Sensivity()
    {
        PlayerPrefs.SetFloat("Sensivity", _scrollbar.value);
        PlayerPrefs.Save();
    }

}
