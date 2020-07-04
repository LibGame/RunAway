using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingButton : MonoBehaviour
{
    public Animation SettingPanelAnimation;
    public GameObject SettingPanel;

    public void OpenSettingPanel()
    {
        SettingPanel.SetActive(true);
        SettingPanelAnimation.Play("SettingsPanel");
    }
    
}
