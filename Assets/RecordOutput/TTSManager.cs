using TMPro;
using UnityEngine;
using System.Threading;

#if UNITY_ANDROID && !UNITY_EDITOR
using AndroidJavaObject = UnityEngine.AndroidJavaObject;
using AndroidJavaClass = UnityEngine.AndroidJavaClass;
#endif

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
using System.Speech.Synthesis;
#endif

public class TTSManager : MonoBehaviour
{
#if UNITY_ANDROID && !UNITY_EDITOR
    private AndroidJavaObject ttsObject;
    private AndroidJavaObject unityActivity;

    void Start()
    {
        // 只在 Android 平台上執行 TTS 初始化
        if (Application.platform == RuntimePlatform.Android)
        {
            try
            {
                using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    ttsObject = new AndroidJavaObject("android.speech.tts.TextToSpeech", unityActivity, new TTSListener());
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Android TTS 初始化失敗：" + ex.Message);
            }
        }
    }

    public void Speak(string text)
    {
        if (ttsObject != null)
        {
            try
            {
                ttsObject.Call<int>("speak", text, 0, null, null);
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Android 語音播放失敗：" + ex.Message);
            }
        }
        else
        {
            Debug.LogWarning("Android TTS 尚未初始化，無法播放語音");
        }
    }

    private class TTSListener : AndroidJavaProxy
    {
        public TTSListener() : base("android.speech.tts.TextToSpeech$OnInitListener") { }

        void onInit(int status)
        {
            Debug.Log("Android TTS Initialized with status: " + status);
        }
    }

#elif UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
    private SpeechSynthesizer synth;

    void Start()
    {
        try
        {
            synth = new SpeechSynthesizer();
            synth.SetOutputToDefaultAudioDevice(); // 確保使用預設音訊設備
            synth.Rate = 0;  // 可以調整語速
            synth.Volume = 100; // 音量
        }
        catch (System.Exception ex)
        {
            Debug.LogError("SpeechSynthesizer 初始化失敗：" + ex.Message);
        }
    }

    public void Speak(string text)
    {
        if (synth == null)
        {
            Debug.LogWarning("SpeechSynthesizer 尚未初始化，無法播放語音");
            return;
        }

        // 使用非主執行緒播放語音
        Thread t = new Thread(() =>
        {
            try
            {
                synth.Speak(text);
            }
            catch (System.Exception e)
            {
                Debug.LogError("語音播放失敗：" + e.Message);
            }
        });
        t.Start();
    }

#else
    void Start()
    {
        try
        {
            // 嘗試創建SpeechSynthesizer實例
            synth = new SpeechSynthesizer();

            // 如果無法創建，這裡會拋出異常
            if (synth == null)
            {
                Debug.LogError("SpeechSynthesizer 實例創建失敗！");
                return;
            }

            synth.SetOutputToDefaultAudioDevice(); // 設置默認音訊設備
            synth.Rate = 0;  // 可以調整語速
            synth.Volume = 100; // 設置音量
        }
        catch (System.Exception ex)
        {
            Debug.LogError("SpeechSynthesizer 初始化失敗：" + ex.Message);
        }
    }

    public void Speak(string text)
    {
        Debug.Log("TTSManager: 無法播放語音，因為目前平台不支援");
    }
#endif
}
