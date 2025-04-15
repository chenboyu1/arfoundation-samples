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

        /*
         明 唐寅 尺牘
        故書339-8
        紙本

        此作為唐寅（1470－1524）致行臺大人餘山先生的書信，內容中提請其多加照顧友人（或門生）盧鈇（ㄈㄨ），並隨信奉上五斤乳餅，以感謝對方平日的關照。乳餅（或即為乳酪）在明代為常見的日用食品，鄺（ㄎㄨㄤˋ）璠（ㄈㄢˊ）（約1458－1521）《便民圖纂》中便詳細記載了乳餅的製作和保存方式，足見此物之普及。

        信上畫有縱線，以行楷書寫，字跡秀雅而不失力度，字距和行距疏朗有致，整體呈現清新雅致的氣息。
         */

        // 顯示在畫面上
        displayText.text = text;
        displayText.gameObject.SetActive(true);

        // 同步輸出到 Console
        Debug.Log("語音內容: " + text);

        var tts = FindObjectOfType<AndroidTTS>();
        if (tts != null)
        {
            tts.Speak(text);
        }
        else
        {
            Debug.LogWarning("AndroidTTS 未找到，無法播放語音");
        }
    }

    public void ToggleText()
    {
        displayText.gameObject.SetActive(!displayText.gameObject.activeSelf);
    }
}
