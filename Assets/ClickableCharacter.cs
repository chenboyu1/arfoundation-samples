using UnityEngine;
using TMPro;

public class ClickableCharacter : MonoBehaviour
{
    public string speechText = "你好，我是角色！";

    public void Speak()
    {
        Debug.Log(speechText);

        var tts = FindObjectOfType<AndroidTTS>(); // 或 TTSManager
        if (tts != null)
        {
            tts.Speak(speechText);
        }
    }
    /*
    [TextArea]
    public string dialogueText = "你好！歡迎來到我的世界～";
    public TMP_Text displayText; // 顯示文字的 UI Text（例如 Canvas 上的 TMP_Text）

    void OnMouseDown()
    {
        Debug.Log("點擊角色，語音內容：" + dialogueText);

        if (displayText != null)
        {
            displayText.text = dialogueText;
            displayText.gameObject.SetActive(true);
        }

        var tts = FindObjectOfType<AndroidTTS>();
        if (tts != null)
        {
            tts.Speak(dialogueText);
        }
        else
        {
            Debug.LogWarning("找不到 AndroidTTS，無法播放語音");
        }
    }*/
}
