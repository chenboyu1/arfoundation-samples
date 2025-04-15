using UnityEngine;

public class AndroidTTS : MonoBehaviour
{
    private AndroidJavaObject ttsObject;
    private AndroidJavaObject activityContext;

    void Start()
    {
        // ���o Android Activity
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            activityContext = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        }

        // ��l�� TextToSpeech ����
        ttsObject = new AndroidJavaObject("android.speech.tts.TextToSpeech", activityContext, new TTSInitListener());

        // �]�w�y���]�Ҧp�����c��^
        AndroidJavaObject locale = new AndroidJavaObject("java.util.Locale", "zh", "TW");
        ttsObject.Call<int>("setLanguage", locale);

        // �i��G�]�w�y�t�P����
        ttsObject.Call("setSpeechRate", 1.0f);  // ���`�y�t
        ttsObject.Call("setPitch", 1.0f);       // ���`����
    }

    // �y��������
    public void Speak(string message)
    {
        if (ttsObject != null)
        {
            ttsObject.Call<int>("speak", message, 0, null, null);
        }
    }

    // �[�b�쥻�� AndroidTTS.cs ��

    // ���A�i�H�q Inspector �j�w��
    public void SpeakFromUI()
    {
        string message = "�A�n";
        Speak(message);
    }


    // ��l�Ʀ^�I
    class TTSInitListener : AndroidJavaProxy
    {
        public TTSInitListener() : base("android.speech.tts.TextToSpeech$OnInitListener") { }

        void onInit(int status)
        {
            Debug.Log("TTS ��l�Ƨ����A���A�G" + status);
        }
    }
}
