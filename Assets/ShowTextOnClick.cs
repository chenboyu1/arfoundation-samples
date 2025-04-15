using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowTextOnClick : MonoBehaviour
{
    public TMP_Text displayText;

    void Start()
    {
        displayText.gameObject.SetActive(false);
    }

    public void OnClickPlay()
    {
        Debug.Log("按鈕觸發了！");


        string text = @"你好";

        displayText.text = text;
        displayText.gameObject.SetActive(true);

        var tts = FindObjectOfType<TTSManager>();
        if (tts != null)
        {
            tts.Speak(text);
        }
        else
        {
            Debug.LogWarning("TTSManager 未找到，無法播放語音");
        }
    }




    public void ToggleText()
    {
        displayText.gameObject.SetActive(!displayText.gameObject.activeSelf);
    }
}
