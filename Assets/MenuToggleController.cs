using TMPro;
using UnityEngine;

public class MenuToggleController : MonoBehaviour
{
    public GameObject explanationMenu;  // 說明群組
    public GameObject voiceMenu;        // 語音群組
    public GameObject dialogPanel;       // 對話框 Panel
    public TMP_Text displayText;         // 對話框內顯示的文字

    public void ToggleExplanationMenu()
    {
        bool isActive = explanationMenu.activeSelf;
        explanationMenu.SetActive(!isActive);  // 切換顯示
        displayText.gameObject.SetActive(!isActive);
        voiceMenu.SetActive(false);            // 關閉另一群組
    }

    public void ToggleVoiceMenu()
    {
        bool isActive = voiceMenu.activeSelf;
        voiceMenu.SetActive(!isActive);
        explanationMenu.SetActive(false);
        dialogPanel.SetActive(true);
        displayText.gameObject.SetActive(false);
    }

    public void CloseAll()
    {
        explanationMenu.SetActive(false);
        voiceMenu.SetActive(false);
        displayText.gameObject.SetActive(false);
    }
}
