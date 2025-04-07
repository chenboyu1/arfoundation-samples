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
    private AudioClip recordedClip; // 用來存錄音的 AudioClip

    void Start()
    {
        audioFilePath = Path.Combine(Application.persistentDataPath, "recordedAudio.wav");
        Debug.Log($"音訊檔案儲存路徑：{audioFilePath}");

        apiKey = LoadApiKey();
        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("無法讀取 Google API Key，請檢查 JSON 檔案！");
        }
        else
        {
            Debug.Log("成功讀取 API Key！");
        }

        if (voiceRecorder == null)
        {
            Debug.LogError("voiceRecorder 未綁定，請在 Unity Inspector 確保已設定！");
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
            Debug.LogError("Google API Key JSON 檔案未找到：" + filePath);
            return null;
        }

        try
        {
            string jsonContent = File.ReadAllText(filePath);
            JObject json = JObject.Parse(jsonContent);
            string key = json["private_key"]?.ToString();

            if (string.IsNullOrEmpty(key))
            {
                Debug.LogError("Google API Key JSON 檔案內沒有 'private_key' 欄位！");
                return null;
            }

            return key;
        }
        catch (System.Exception ex)
        {
            Debug.LogError("讀取 API Key 時發生錯誤：" + ex.Message);
            return null;
        }
    }

    public void ToggleRecording()
    {
        if (voiceRecorder == null)
        {
            Debug.LogError("voiceRecorder 未初始化，無法錄音！");
            return;
        }

        if (isRecording)
        {
            voiceRecorder.StopRecording();
            isRecording = false;
            Debug.Log("錄音結束，開始進行語音辨識...");

            recordedClip = voiceRecorder.GetRecordedClip(); // 從 VoiceRecorder 取得錄音

            if (recordedClip != null)
            {
                StartCoroutine(UploadAudio()); // 上傳錄音進行語音辨識
            }
            else
            {
                Debug.LogError("錄音片段為空，無法進行語音辨識！");
            }
        }
        else
        {
            voiceRecorder.StartRecording();
            isRecording = true;
            Debug.Log("開始錄音...");
        }
    }

    IEnumerator UploadAudio()
    {
        if (!File.Exists(audioFilePath))
        {
            Debug.LogError("錄音檔案未找到：" + audioFilePath);
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
                content = base64Audio // 確保這裡是 Base64 編碼的音訊數據
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
                Debug.Log("語音辨識結果: " + jsonResponse);
                ProcessSpeechToTextResponse(jsonResponse);
            }
            else
            {
                Debug.LogError("語音辨識失敗：" + www.error);
                Debug.LogError("錯誤詳細訊息：" + www.downloadHandler.text);
            }
        }
    }


    private void ProcessSpeechToTextResponse(string jsonResponse)
    {
        try
        {
            Debug.Log("開始解析 API 回應...");
            JObject response = JObject.Parse(jsonResponse);
            var results = response["results"];

            if (results != null && results.HasValues)
            {
                string transcript = results[0]["alternatives"][0]["transcript"]?.ToString();
                Debug.Log($"語音辨識結果：{transcript}");

                if (userInputField != null)
                {
                    userInputField.text += transcript;
                }
                else
                {
                    Debug.LogWarning("userInputField 尚未指定！");
                }
            }
            else
            {
                Debug.LogWarning("API 回傳成功，但找不到辨識結果！");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("解析 API 回應時發生錯誤：" + ex.Message);
        }
    }
}