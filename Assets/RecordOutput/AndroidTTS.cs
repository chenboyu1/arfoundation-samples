using UnityEngine;

public class AndroidTTS : MonoBehaviour
{
    private AndroidJavaObject ttsObject;
    private AndroidJavaObject activityContext;

    void Start()
    {
        // 取得 Android Activity
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            activityContext = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        }

        // 初始化 TextToSpeech 引擎
        ttsObject = new AndroidJavaObject("android.speech.tts.TextToSpeech", activityContext, new TTSInitListener());

        // 設定語言（例如中文繁體）
        AndroidJavaObject locale = new AndroidJavaObject("java.util.Locale", "zh", "TW");
        ttsObject.Call<int>("setLanguage", locale);

        // 可選：設定語速與音調
        ttsObject.Call("setSpeechRate", 1.0f);  // 正常語速
        ttsObject.Call("setPitch", 1.0f);       // 正常音調
    }

    // 語音播放函數
    public void Speak(string message)
    {
        if (ttsObject != null)
        {
            ttsObject.Call<int>("speak", message, 0, null, null);
        }
    }

    // 加在原本的 AndroidTTS.cs 裡

    // 讓你可以從 Inspector 綁定用
    public void SpeakFromUI()
    {
        string message = "你好";
        Speak(message);
    }


    // 初始化回呼
    class TTSInitListener : AndroidJavaProxy
    {
        public TTSInitListener() : base("android.speech.tts.TextToSpeech$OnInitListener") { }

        void onInit(int status)
        {
            Debug.Log("TTS 初始化完成，狀態：" + status);
        }
    }
}
