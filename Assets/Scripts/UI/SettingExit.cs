
using UnityEngine;

public class SettingExit : MonoBehaviour
{
    public Animation SettingsPanel;

    public void CloseSettingsTab()
    {
        SettingsPanel.Play("SettingsPanelFadeOut");
    }
}
