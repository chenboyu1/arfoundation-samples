using UnityEngine;

public class AndroidTTS : MonoBehaviour
{
    private AndroidJavaObject tts;
    private AndroidJavaObject activity;

    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        // ���� Unity �� Activity
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        }

        // �إ� TextToSpeech ����
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

    // ��l�Ʀ^�I��ť��
    private class TTSInitListener : AndroidJavaProxy
    {
        public TTSInitListener() : base("android.speech.tts.TextToSpeech$OnInitListener") { }

        void onInit(int status)
        {
            Debug.Log("TTS ��l�Ƨ����A���A: " + status);
        }
    }
}
