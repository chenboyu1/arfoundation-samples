using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using TMPro;
using System;


public class ChatGPTManager : MonoBehaviour
{
    private string chatGptApiKey;
    private string googleApiKey;
    private string apiUrl = "https://api.openai.com/v1/chat/completions";
    private string googleApiUrl = "https://speech.googleapis.com/v1/speech:recognize?key=";

    private string chatGPTJsonFilePath;
    private string googleJsonFilePath;

    public TMP_InputField userInput; // 使用者輸入框
    public TMP_Text responseText;    // 顯示 ChatGPT 回應的 UI 文字
    void Start()
    {
        // 設定 JSON 檔案路徑
        chatGPTJsonFilePath = Path.Combine(Application.streamingAssetsPath, "chatGPT API.json");
        googleJsonFilePath = Path.Combine(Application.streamingAssetsPath, "AR-MR-google_credentials.json");

        // 讀取 API 金鑰
        StartCoroutine(LoadApiKey(chatGPTJsonFilePath, "api_key", OnApiKeyLoaded));
        StartCoroutine(LoadApiKey(googleJsonFilePath, "private_key", OnApiKeyLoaded2));
        // 讀取 API 金鑰
        chatGptApiKey = LoadApiKey(chatGPTJsonFilePath, "api_key");
        googleApiKey = LoadApiKey(googleJsonFilePath, "private_key");

        if (string.IsNullOrEmpty(chatGptApiKey))
        {
            Debug.LogError("無法讀取 ChatGPT API 金鑰，請檢查 chatGPT API.json 檔案！");
        }
        else
        {
            Debug.Log("成功讀取 ChatGPT API 金鑰");
        }

        if (string.IsNullOrEmpty(googleApiKey))
        {
            Debug.LogError("無法讀取 Google Speech-to-Text API 金鑰，請檢查 AR-MR-google_credentials.json 檔案！");
        }
        else
        {
            Debug.Log("成功讀取 Google Speech-to-Text API 金鑰");
        }
    }
    private string LoadApiKey(string filePath, string keyName)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("找不到 JSON 檔案：" + filePath);
            return null;
        }

        try
        {
            string jsonContent = File.ReadAllText(filePath);
            JObject json = JObject.Parse(jsonContent);
            return json[keyName]?.ToString(); // 讀取指定的 Key
        }
        catch (System.Exception ex)
        {
            Debug.LogError("讀取 JSON 檔案錯誤：" + ex.Message);
            return null;
        }
    }
    // 回調函數處理加載的 API Key
    void OnApiKeyLoaded(string apiKey)
    {
        if (!string.IsNullOrEmpty(apiKey))
        {
            chatGptApiKey = apiKey;  // 將金鑰賦值給變數
            Debug.Log("成功讀取 ChatGPT API 金鑰: " + chatGptApiKey);
        }
        else
        {
            //Debug.LogError("無法讀取 ChatGPT API 金鑰！");
        }
    }
    void OnApiKeyLoaded2(string apiKey)
    {
        if (!string.IsNullOrEmpty(apiKey))
        {
            googleApiKey = apiKey;  // 將金鑰賦值給變數
            Debug.Log("成功讀取 google API 金鑰: " + chatGptApiKey);
        }
        else
        {
            //Debug.LogError("無法讀取 ChatGPT API 金鑰！");
        }
    }
    /// <summary>
    /// 讀取指定 JSON 檔案中的 API Key
    /// </summary>
    private IEnumerator LoadApiKey(string filePath, string keyName, Action<string> callback)
    {
        // Android 平台需要使用 UnityWebRequest 來讀取嵌入的文件
        if (filePath.Contains("://") || filePath.Contains("file://"))
        {
            UnityWebRequest www = UnityWebRequest.Get(filePath);
            yield return www.SendWebRequest(); // 等待請求完成

            if (www.result == UnityWebRequest.Result.Success)
            {
                string jsonContent = www.downloadHandler.text;
                // 解析 JSON
                Debug.Log("File content: " + jsonContent);

                try
                {
                    JObject json = JObject.Parse(jsonContent);
                    string apiKey = json[keyName]?.ToString(); // 讀取指定的 Key
                    callback(apiKey); // 調用回調傳遞金鑰
                }
                catch (JsonException ex)
                {
                    Debug.LogError("JSON 解析錯誤：" + ex.Message);
                    callback(null); // 發生錯誤時回調返回 null
                }
            }
            else
            {
                Debug.LogError("Failed to load JSON file: " + www.error);
                callback(null); // 請求失敗時回調返回 null
            }
        }
        else
        {
            callback(null); // 如果路徑不正確，回調返回 null
        }
    }



    public void SendMessageToChatGPT()
    {
        Debug.LogWarning("function call");
        string userMessage = userInput.text; // 讀取輸入框的文字
        if (!string.IsNullOrEmpty(userMessage)) // 確保輸入不是空的
        {
            Debug.LogWarning("send request");
            StartCoroutine(SendChatGPTRequest(userMessage));
        }
        else
        {
            responseText.text = "請輸入訊息！";
        }
    }

    private IEnumerator SendChatGPTRequest(string message)
    {
        string jsonPayload = JsonConvert.SerializeObject(new
        {
            model = "gpt-3.5-turbo",
            messages = new object[]
            {
                new { role = "system", content = "You are a friendly AI assistant." },
                new { role = "user", content = message }
            }
        });

        byte[] postData = Encoding.UTF8.GetBytes(jsonPayload);

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(postData);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + chatGptApiKey);

            yield return request.SendWebRequest();
            Debug.LogWarning("already send");
            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseJson = request.downloadHandler.text;
                Debug.LogWarning("API 回應：" + responseJson);

                try
                {
                    // 解析 API 回應
                    var response = JsonConvert.DeserializeObject<ChatGPTResponse>(responseJson);
                    if (response.choices.Length > 0)
                    {
                        string chatGPTReply = response.choices[0].message.content;
                        responseText.text = chatGPTReply; // 顯示在 UI 上
                    }
                    else
                    {
                        responseText.text = "ChatGPT 沒有回應，請稍後再試。";
                    }
                }
                catch (JsonException jsonEx)
                {
                    Debug.LogError("JSON 解析錯誤：" + jsonEx.Message);
                    responseText.text = "API 回應格式錯誤！";
                }
            }
            else
            {
                Debug.LogError("API 請求失敗: " + request.error);
                responseText.text = "錯誤：" + request.error;
            }
        }
    }

    public IEnumerator SendAudioToGoogleSpeech(byte[] audioData)
    {
        string fullUrl = googleApiUrl + googleApiKey;

        var requestData = new
        {
            config = new
            {
                encoding = "LINEAR16",
                sampleRateHertz = 16000,
                languageCode = "zh-TW" // 可改成你要的語言
            },
            audio = new
            {
                content = System.Convert.ToBase64String(audioData) // 轉成 Base64
            }
        };

        string jsonPayload = JsonConvert.SerializeObject(requestData);
        byte[] postData = Encoding.UTF8.GetBytes(jsonPayload);

        using (UnityWebRequest request = new UnityWebRequest(fullUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(postData);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Google Speech-to-Text API 回應：" + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Google Speech-to-Text API 失敗：" + request.error);
            }
        }
    }
}

// 定義 JSON 解析類別
[System.Serializable]
public class ChatGPTResponse
{
    public Choice[] choices;
}

[System.Serializable]
public class Choice
{
    public Message message;
}

[System.Serializable]
public class Message
{
    public string role;
    public string content;
}
