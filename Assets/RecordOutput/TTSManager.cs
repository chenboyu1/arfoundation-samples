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
        // �u�b Android ���x�W���� TTS ��l��
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
                Debug.LogError("Android TTS ��l�ƥ��ѡG" + ex.Message);
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
                Debug.LogError("Android �y�����񥢱ѡG" + ex.Message);
            }
        }
        else
        {
            Debug.LogWarning("Android TTS �|����l�ơA�L�k����y��");
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
            synth.SetOutputToDefaultAudioDevice(); // �T�O�ϥιw�]���T�]��
            synth.Rate = 0;  // �i�H�վ�y�t
            synth.Volume = 100; // ���q
        }
        catch (System.Exception ex)
        {
            Debug.LogError("SpeechSynthesizer ��l�ƥ��ѡG" + ex.Message);
        }
    }

    public void Speak(string text)
    {
        if (synth == null)
        {
            Debug.LogWarning("SpeechSynthesizer �|����l�ơA�L�k����y��");
            return;
        }

        // �ϥΫD�D���������y��
        Thread t = new Thread(() =>
        {
            try
            {
                synth.Speak(text);
            }
            catch (System.Exception e)
            {
                Debug.LogError("�y�����񥢱ѡG" + e.Message);
            }
        });
        t.Start();
    }

#else
    void Start()
    {
        try
        {
            // ���ճЫ�SpeechSynthesizer���
            synth = new SpeechSynthesizer();

            // �p�G�L�k�ЫءA�o�̷|�ߥX���`
            if (synth == null)
            {
                Debug.LogError("SpeechSynthesizer ��ҳЫإ��ѡI");
                return;
            }

            synth.SetOutputToDefaultAudioDevice(); // �]�m�q�{���T�]��
            synth.Rate = 0;  // �i�H�վ�y�t
            synth.Volume = 100; // �]�m���q
        }
        catch (System.Exception ex)
        {
            Debug.LogError("SpeechSynthesizer ��l�ƥ��ѡG" + ex.Message);
        }
    }

    public void Speak(string text)
    {
        Debug.Log("TTSManager: �L�k����y���A�]���ثe���x���䴩");
    }
#endif
}
