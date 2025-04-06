using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using TMPro;

public class GoogleSpeechToText : MonoBehaviour
{
    private string apiKey;
    private string audioFilePath;
    private bool isRecording = false;

    public TMP_InputField userInputField;
    public VoiceRecorder voiceRecorder;
    private AudioClip recordedClip; // �ΨӦs������ AudioClip

    void Start()
    {
        audioFilePath = Path.Combine(Application.persistentDataPath, "recordedAudio.wav");
        Debug.Log($"���T�ɮ��x�s���|�G{audioFilePath}");

        apiKey = LoadApiKey();
        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("�L�kŪ�� Google API Key�A���ˬd JSON �ɮסI");
        }
        else
        {
            Debug.Log("���\Ū�� API Key�I");
        }

        if (voiceRecorder == null)
        {
            Debug.LogError("voiceRecorder ���j�w�A�Цb Unity Inspector �T�O�w�]�w�I");
        }

        if (GetComponent<AudioSource>() == null)
        {
            gameObject.AddComponent<AudioSource>();
        }
    }

    private string LoadApiKey()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "AR-MR-google_credentials.json");

        if (!File.Exists(filePath))
        {
            Debug.LogError("Google API Key JSON �ɮץ����G" + filePath);
            return null;
        }

        try
        {
            string jsonContent = File.ReadAllText(filePath);
            JObject json = JObject.Parse(jsonContent);
            string key = json["private_key"]?.ToString();

            if (string.IsNullOrEmpty(key))
            {
                Debug.LogError("Google API Key JSON �ɮפ��S�� 'private_key' ���I");
                return null;
            }

            return key;
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Ū�� API Key �ɵo�Ϳ��~�G" + ex.Message);
            return null;
        }
    }

    public void ToggleRecording()
    {
        if (voiceRecorder == null)
        {
            Debug.LogError("voiceRecorder ����l�ơA�L�k�����I");
            return;
        }

        if (isRecording)
        {
            voiceRecorder.StopRecording();
            isRecording = false;
            Debug.Log("���������A�}�l�i��y������...");

            recordedClip = voiceRecorder.GetRecordedClip(); // �q VoiceRecorder ���o����

            if (recordedClip != null)
            {
                StartCoroutine(UploadAudio()); // �W�ǿ����i��y������
            }
            else
            {
                Debug.LogError("�������q���šA�L�k�i��y�����ѡI");
            }
        }
        else
        {
            voiceRecorder.StartRecording();
            isRecording = true;
            Debug.Log("�}�l����...");
        }
    }

    IEnumerator UploadAudio()
    {
        if (!File.Exists(audioFilePath))
        {
            Debug.LogError("�����ɮץ����G" + audioFilePath);
            yield break;
        }

        byte[] audioData = File.ReadAllBytes(audioFilePath);
        string base64Audio = System.Convert.ToBase64String(audioData);

        string url = $"https://speech.googleapis.com/v1/speech:recognize?key={apiKey}";

        string jsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(new
        {
            config = new
            {
                encoding = "LINEAR16",
                sampleRateHertz = 16000,
                languageCode = "zh-TW"
            },
            audio = new
            {
                content = base64Audio // �T�O�o�̬O Base64 �s�X�����T�ƾ�
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
                Debug.Log("�y�����ѵ��G: " + jsonResponse);
                ProcessSpeechToTextResponse(jsonResponse);
            }
            else
            {
                Debug.LogError("�y�����ѥ��ѡG" + www.error);
                Debug.LogError("���~�ԲӰT���G" + www.downloadHandler.text);
            }
        }
    }


    private void ProcessSpeechToTextResponse(string jsonResponse)
    {
        try
        {
            Debug.Log("�}�l�ѪR API �^��...");
            JObject response = JObject.Parse(jsonResponse);
            var results = response["results"];

            if (results != null && results.HasValues)
            {
                string transcript = results[0]["alternatives"][0]["transcript"]?.ToString();
                Debug.Log($"�y�����ѵ��G�G{transcript}");

                if (userInputField != null)
                {
                    userInputField.text += transcript;
                }
                else
                {
                    Debug.LogWarning("userInputField �|�����w�I");
                }
            }
            else
            {
                Debug.LogWarning("API �^�Ǧ��\�A���䤣����ѵ��G�I");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("�ѪR API �^���ɵo�Ϳ��~�G" + ex.Message);
        }
    }
}