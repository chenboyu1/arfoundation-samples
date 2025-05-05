using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using TMPro;
using System;
using Newtonsoft.Json;
using UnityEngine.UI;


public class GoogleSpeechToText : MonoBehaviour
{
    private string apiKey;
    private string audioFilePath;
    private bool isRecording = false;

    public TMP_InputField userInputField;
    public VoiceRecorder voiceRecorder;
    private AudioClip recordedClip;
    public TextMeshProUGUI btnName; // 按鈕上的文字
    public Button clearButton;

    void Start()
    {
        audioFilePath = Path.Combine(Application.persistentDataPath, "recordedAudio.wav");
        Debug.Log($"音訊檔案儲存路徑：{audioFilePath}");

        // 讀取 API 金鑰
        StartCoroutine(LoadKey("AR-MR-google_credentials.json", "private_key", OnApiKeyLoaded2));

        if (voiceRecorder == null)
        {
            Debug.LogError("voiceRecorder 未綁定，請在 Unity Inspector 確保已設定！1");
        }

        if (GetComponent<AudioSource>() == null)
        {
            gameObject.AddComponent<AudioSource>();
        }

        if (clearButton != null)
        {
            clearButton.onClick.AddListener(ClearInputField);
        }
        else
        {
            Debug.LogWarning("Clear Button 尚未綁定！");
        }
    }
    void OnEnable()
    {
        if (string.IsNullOrEmpty(apiKey))
        {
            StartCoroutine(LoadKey("AR-MR-google_credentials.json", "private_key", OnApiKeyLoaded2));
        }
    }
    // 修改後的 API 金鑰讀取方法
    private IEnumerator LoadKey(string fileName, string keyName, Action<string> callback)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        Debug.LogWarning("101嘗試讀取 JSON 路徑: " + filePath);

        #if UNITY_ANDROID && !UNITY_EDITOR
            UnityWebRequest www = UnityWebRequest.Get(filePath);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string jsonContent = www.downloadHandler.text;
                Debug.LogWarning("102讀取成功");

                try
                {
                    JObject json = JObject.Parse(jsonContent);
                    apiKey = json[keyName]?.ToString();
                Debug.LogWarning("102 APIkey: " + apiKey);
                callback(apiKey);
                }
                catch (JsonException ex)
                {
                    Debug.LogError("103JSON 解析錯誤：" + ex.Message);
                    callback(null);
                }
            }
            else
            {
                Debug.LogError("104載入 JSON 檔案失敗：" + www.error);
                callback(null);
            }
        #else
        if (File.Exists(filePath))
        {
            string jsonContent = File.ReadAllText(filePath);
            Debug.LogWarning("102讀取成功");

            try
            {
                JObject json = JObject.Parse(jsonContent);
                apiKey = json[keyName]?.ToString();
                callback(apiKey);
            }
            catch (JsonException ex)
            {
                Debug.LogError("103JSON 解析錯誤：" + ex.Message);
                callback(null);
            }
        }
        else
        {
            Debug.LogError("104載入 JSON 檔案失敗：檔案不存在");
            callback(null);
        }
        yield break;
        #endif
    }



    void OnApiKeyLoaded2(string Key)
    {
        if (!string.IsNullOrEmpty(Key))
        {
            apiKey = Key;
            Debug.LogWarning("4成功讀取 Google API 金鑰");
        }
        else
        {
            Debug.LogError("5無法讀取 API 金鑰！");
        }
    }

    public void ToggleRecording()
    {
        if (voiceRecorder == null)
        {
            Debug.LogError("6voiceRecorder 未初始化，無法錄音！");
            return;
        }

        if (isRecording)
        {
            voiceRecorder.StopRecording();
            isRecording = false;
            Debug.LogWarning("7錄音結束，開始進行語音辨識...");
            btnName.text = "結束錄音";

            recordedClip = voiceRecorder.GetRecordedClip();
            if (recordedClip != null)
            {
                StartCoroutine(UploadAudio()); // 上傳錄音
                btnName.text = "辨識中";
            }
            else
            {
                Debug.LogError("8錄音片段為空，無法進行語音辨識！");
                btnName.text = "語音";
            }
        }
        else
        {
            voiceRecorder.StartRecording();
            isRecording = true;
            Debug.LogWarning("9開始錄音...");
            btnName.text = "錄音中";
        }
    }

    IEnumerator UploadAudio()
    {
        Debug.LogWarning("=== UploadAudio START ===");
        Debug.LogWarning("API Key: " + apiKey);
        Debug.LogWarning("Audio File Path: " + audioFilePath);
        Debug.LogWarning("File Exists: " + File.Exists(audioFilePath));

        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("API key is missing. Cannot proceed with the request.");
            yield break;
        }

        if (!File.Exists(audioFilePath))
        {
            Debug.LogError("Audio file not found at: " + audioFilePath);
            yield break;
        }

        Debug.LogWarning("Uploading audio. File path: " + audioFilePath);

        byte[] audioData = File.ReadAllBytes(audioFilePath);
        if (audioData == null || audioData.Length == 0)
        {
            Debug.LogError("Audio data is empty. Please check if recording succeeded.");
            yield break;
        }

        string base64Audio = Convert.ToBase64String(audioData);
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

            Debug.LogWarning("Sending speech recognition request...");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = www.downloadHandler.text;
                Debug.LogWarning("Speech recognition successful. Response: " + jsonResponse);
                ProcessSpeechToTextResponse(jsonResponse);
            }
            else
            {
                Debug.LogError("Speech recognition failed: " + www.error);
                Debug.LogError("Error details: " + www.downloadHandler.text);
            }

            if (btnName != null)
            {
                btnName.text = "語音";
            }
        }
    }


    private void ProcessSpeechToTextResponse(string jsonResponse)
    {
        try
        {
            Debug.LogWarning("14開始解析 API 回應...");
            JObject response = JObject.Parse(jsonResponse);
            var results = response["results"];

            if (results != null && results.HasValues)
            {
                string transcript = results[0]["alternatives"][0]["transcript"]?.ToString();
                Debug.LogWarning($"15語音辨識結果：{transcript}");

                if (userInputField != null)
                {
                    userInputField.text += transcript;
                }
                else
                {
                    Debug.LogWarning("16userInputField 尚未指定！");
                }
            }
            else
            {
                Debug.LogWarning("17API 回傳成功，但找不到辨識結果！");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("18解析 API 回應時發生錯誤：" + ex.Message);
        }
    }

    private void ClearInputField()
    {
        if (userInputField != null)
        {
            userInputField.text = "";
        }
    }
}
