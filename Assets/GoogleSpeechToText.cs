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
    }

    // 修改後的 API 金鑰讀取方法
    private IEnumerator LoadKey(string fileName, string keyName, Action<string> callback)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

#if UNITY_ANDROID && !UNITY_EDITOR
    //filePath = "jar:file://" + filePath;
#endif

        Debug.LogWarning("101嘗試讀取 JSON 路徑: " + filePath);

        UnityWebRequest www = UnityWebRequest.Get(filePath);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string jsonContent = www.downloadHandler.text;
            Debug.LogWarning("102讀取成功，內容：" + jsonContent);

            try
            {
                JObject json = JObject.Parse(jsonContent);
                string apiKey = json[keyName]?.ToString();
                callback(apiKey); // 回傳 API key
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
    }



    void OnApiKeyLoaded2(string Key)
    {
        if (!string.IsNullOrEmpty(Key))
        {
            apiKey = Key;
            Debug.LogWarning("4成功讀取 Google API 金鑰: " + apiKey);
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

            recordedClip = voiceRecorder.GetRecordedClip();
            if (recordedClip != null)
            {
                StartCoroutine(UploadAudio()); // 上傳錄音
            }
            else
            {
                Debug.LogError("8錄音片段為空，無法進行語音辨識！");
            }
            btnName.text = "語音";
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
        if (!File.Exists(audioFilePath))
        {
            Debug.LogError("10錄音檔案未找到：" + audioFilePath);
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
                Debug.LogWarning("11語音辨識結果: " + jsonResponse);
                ProcessSpeechToTextResponse(jsonResponse);
            }
            else
            {
                Debug.LogError("12語音辨識失敗：" + www.error);
                Debug.LogError("13錯誤詳細訊息：" + www.downloadHandler.text);
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
}
