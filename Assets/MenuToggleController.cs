using UnityEngine;

public class MenuToggleController : MonoBehaviour
{
    public GameObject explanationMenu;  // 說明群組
    public GameObject voiceMenu;        // 語音群組

    public void ToggleExplanationMenu()
    {
        bool isActive = explanationMenu.activeSelf;
        explanationMenu.SetActive(!isActive);  // 切換顯示
        voiceMenu.SetActive(false);            // 關閉另一群組
    }

    public void ToggleVoiceMenu()
    {
        bool isActive = voiceMenu.activeSelf;
        voiceMenu.SetActive(!isActive);
        explanationMenu.SetActive(false);
    }
}
