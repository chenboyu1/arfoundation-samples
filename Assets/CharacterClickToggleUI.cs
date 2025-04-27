using TMPro;
using UnityEngine;
using UnityEngine.UI;  // 引入 UI 系統

public class CharacterClickToggleUI : MonoBehaviour
{
    public GameObject inputFieldUI; 
    public GameObject voiceButtonUI; 
    public GameObject clearButtonUI; 
    public GameObject responseBoxUI; 
    public GameObject enterButton;

    private bool isUIVisible = false;

    void Start()
    {
        // 在遊戲開始時隱藏 UI 元素
        inputFieldUI.SetActive(false);
        voiceButtonUI.SetActive(false);
        clearButtonUI.SetActive(false);
        responseBoxUI.SetActive(false);
        enterButton.SetActive(false);
    }

    // 當角色被點擊時呼叫
    public void ToggleUI()
    {
        isUIVisible = !isUIVisible;
        Debug.Log("UI Visible: " + isUIVisible);  // 顯示當前狀態

        inputFieldUI.SetActive(isUIVisible);
        voiceButtonUI.SetActive(isUIVisible);
        clearButtonUI.SetActive(isUIVisible);
        responseBoxUI.SetActive(isUIVisible);
        enterButton.SetActive(isUIVisible);
    }
}
