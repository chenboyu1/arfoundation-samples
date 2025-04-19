using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using TMPro;
using System;
using Newtonsoft.Json;

public class GoogleSpeechToText : MonoBehaviour
{
    private string apiKey;
    private string audioFilePath;
    private bool isRecording = false;

    public TMP_InputField userInputField;
    public VoiceRecorder voiceRecorder;
    private AudioClip recordedClip;
    public TextMeshProUGUI btnName;
    void Start()
    {
        audioFilePath = Path.Combine(Application.persistentDataPath, "recordedAudio.wav");
        Debug.Log($"���T�ɮ��x�s���|�G{audioFilePath}");

        // Ū�� API ���_
        StartCoroutine(LoadKey("AR-MR-google_credentials.json", "private_key", OnApiKeyLoaded2));

        if (voiceRecorder == null)
        {
            Debug.LogError("voiceRecorder ���j�w�A�Цb Unity Inspector �T�O�w�]�w�I1");
        }

        if (GetComponent<AudioSource>() == null)
        {
            gameObject.AddComponent<AudioSource>();
        }
    }

    // �ק�᪺ API ���_Ū����k
    private IEnumerator LoadKey(string fileName, string keyName, Action<string> callback)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

#if UNITY_ANDROID && !UNITY_EDITOR
    //filePath = "jar:file://" + filePath;
#endif

        Debug.LogWarning("101����Ū�� JSON ���|: " + filePath);

        UnityWebRequest www = UnityWebRequest.Get(filePath);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string jsonContent = www.downloadHandler.text;
            Debug.LogWarning("102Ū�����\�A���e�G" + jsonContent);

            try
            {
                JObject json = JObject.Parse(jsonContent);
                string apiKey = json[keyName]?.ToString();
                callback(apiKey); // �^�� API key
            }
            catch (JsonException ex)
            {
                Debug.LogError("103JSON �ѪR���~�G" + ex.Message);
                callback(null);
            }
        }
        else
        {
            Debug.LogError("104���J JSON �ɮץ��ѡG" + www.error);
            callback(null);
        }
    }



    void OnApiKeyLoaded2(string Key)
    {
        if (!string.IsNullOrEmpty(Key))
        {
            apiKey = Key;
            Debug.LogWarning("4���\Ū�� Google API ���_: " + apiKey);
        }
        else
        {
            Debug.LogError("5�L�kŪ�� API ���_�I");
        }
    }

    public void ToggleRecording()
    {
        if (voiceRecorder == null)
        {
            Debug.LogError("6voiceRecorder ����l�ơA�L�k�����I");
            return;
        }

        if (isRecording)
        {
            voiceRecorder.StopRecording();
            isRecording = false;
            Debug.LogWarning("7���������A�}�l�i��y������...");

            recordedClip = voiceRecorder.GetRecordedClip();
            if (recordedClip != null)
            {
                StartCoroutine(UploadAudio()); // �W�ǿ���
            }
            else
            {
                Debug.LogError("8�������q���šA�L�k�i��y�����ѡI");
            }
            btnName.text = "�y��";
        }
        else
        {
            voiceRecorder.StartRecording();
            isRecording = true;
            Debug.LogWarning("9�}�l����...");
            btnName.text = "������";
        }
    }

    IEnumerator UploadAudio()
    {
        if (!File.Exists(audioFilePath))
        {
            Debug.LogError("10�����ɮץ����G" + audioFilePath);
            yield break;
        }

        byte[] audioData = File.ReadAllBytes(audioFilePath);
        string base64Audio = System.Convert.ToBase64String(audioData);

        string url = $"https://speech.googleapis.com/v1/speech:recognize?key={apiKey}";

        string jsonRequest = JsonConvert.SerializeObject(new
        {
            config = new
            {
                encoding = "LINEAR16",
                sampleRateHertz = 16000,
                languageCode = "zh-TW"
            },
            audio = new
            {
                content = base64Audio
            }
        });

        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            byte[] jsonToSend = System.Text.Encoding.UTF8.GetBytes(jsonRequest);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = www.downloadHandler.text;
                Debug.LogWarning("11�y�����ѵ��G: " + jsonResponse);
                ProcessSpeechToTextResponse(jsonResponse);
            }
            else
            {
                Debug.LogError("12�y�����ѥ��ѡG" + www.error);
                Debug.LogError("13���~�ԲӰT���G" + www.downloadHandler.text);
            }
        }
    }

    private void ProcessSpeechToTextResponse(string jsonResponse)
    {
        try
        {
            Debug.LogWarning("14�}�l�ѪR API �^��...");
            JObject response = JObject.Parse(jsonResponse);
            var results = response["results"];

            if (results != null && results.HasValues)
            {
                string transcript = results[0]["alternatives"][0]["transcript"]?.ToString();
                Debug.LogWarning($"15�y�����ѵ��G�G{transcript}");

                if (userInputField != null)
                {
                    userInputField.text += transcript;
                }
                else
                {
                    Debug.LogWarning("16userInputField �|�����w�I");
                }
            }
            else
            {
                Debug.LogWarning("17API �^�Ǧ��\�A���䤣����ѵ��G�I");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("18�ѪR API �^���ɵo�Ϳ��~�G" + ex.Message);
        }
    }
}
