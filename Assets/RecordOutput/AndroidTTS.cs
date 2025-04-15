using UnityEngine;

public class AndroidTTS : MonoBehaviour
{
    private AndroidJavaObject tts;
    private AndroidJavaObject activity;

    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        // 拿到 Unity 的 Activity
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        }

        // 建立 TextToSpeech 物件
        tts = new AndroidJavaObject("android.speech.tts.TextToSpeech", activity, new TTSInitListener());
#endif
    }

    public void Speak(string message)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (tts != null)
        {
            tts.Call<int>("speak", message, 0, null, null);
        }
#endif
    }

    // 初始化回呼監聽器
    private class TTSInitListener : AndroidJavaProxy
    {
        public TTSInitListener() : base("android.speech.tts.TextToSpeech$OnInitListener") { }

        void onInit(int status)
        {
            Debug.Log("TTS 初始化完成，狀態: " + status);
        }
    }
}
