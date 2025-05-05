using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using TMPro;
using System;
using UnityEngine.Audio;

public class ChatGPTManager : MonoBehaviour
{
    private string chatGptApiKey;
    private string googleApiKey;
    private string apiUrl = "https://api.openai.com/v1/chat/completions";
    private string googleApiUrl = "https://speech.googleapis.com/v1/speech:recognize?key=";

    private string chatGPTJsonFilePath;
    private string googleJsonFilePath;
    public AudioSource audioSource;

    public TMP_InputField userInput;
    public TMP_Text responseText;

    void Start()
    {
        chatGPTJsonFilePath = Path.Combine(Application.streamingAssetsPath, "chatGPT API.json");
        googleJsonFilePath = Path.Combine(Application.streamingAssetsPath, "AR-MR-google_credentials.json");

        StartCoroutine(LoadApiKey(chatGPTJsonFilePath, "api_key", OnApiKeyLoaded));
        StartCoroutine(LoadApiKey(googleJsonFilePath, "private_key", OnApiKeyLoaded2));

        chatGptApiKey = LoadApiKey(chatGPTJsonFilePath, "api_key");
        googleApiKey = LoadApiKey(googleJsonFilePath, "private_key");

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    #if UNITY_ANDROID && !UNITY_EDITOR
    void OnEnable()
    {
        if (string.IsNullOrEmpty(chatGptApiKey))
        {
            StartCoroutine(LoadApiKey(chatGPTJsonFilePath, "api_key", OnApiKeyLoaded));
        }
        if (string.IsNullOrEmpty(googleApiKey))
        {
            StartCoroutine(LoadApiKey(googleJsonFilePath, "private_key", OnApiKeyLoaded2));
        }
    }
    #endif
    private string LoadApiKey(string filePath, string keyName)
    {
        if (!File.Exists(filePath))
            return null;

        try
        {
            string jsonContent = File.ReadAllText(filePath);
            JObject json = JObject.Parse(jsonContent);
            return json[keyName]?.ToString();
        }
        catch (Exception)
        {
            return null;
        }
    }

    void OnApiKeyLoaded(string apiKey)
    {
        if (!string.IsNullOrEmpty(apiKey))
        {
            chatGptApiKey = apiKey;
        }
    }

    void OnApiKeyLoaded2(string apiKey)
    {
        if (!string.IsNullOrEmpty(apiKey))
        {
            googleApiKey = apiKey;
        }
    }

    private IEnumerator LoadApiKey(string filePath, string keyName, Action<string> callback)
    {
        if (filePath.Contains("://") || filePath.Contains("file://"))
        {
            UnityWebRequest www = UnityWebRequest.Get(filePath);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    JObject json = JObject.Parse(www.downloadHandler.text);
                    callback(json[keyName]?.ToString());
                }
                catch (JsonException)
                {
                    callback(null);
                }
            }
            else
            {
                callback(null);
            }
        }
        else
        {
            callback(null);
        }
    }

    public void SendMessageToChatGPT()
    {
        string userMessage = userInput.text;
        if (!string.IsNullOrEmpty(userMessage))
        {
            StartCoroutine(SendChatGPTRequest(userMessage));
        }
        else
        {
            responseText.text = "請輸入訊息！";
        }
    }

    private IEnumerator SendChatGPTRequest(string message)
    {
        if (string.IsNullOrEmpty(chatGptApiKey))
            yield break;

        responseText.text = "等待 ChatGPT 回應中...";

        string jsonPayload = JsonConvert.SerializeObject(new
        {
            model = "gpt-3.5-turbo",
            messages = new object[]
            {
                new { role = "system", content = "你是語音助理，請使用繁體中文回答。" },
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

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseJson = request.downloadHandler.text;
                var response = JsonConvert.DeserializeObject<ChatGPTResponse>(responseJson);

                if (response.choices.Length > 0)
                {
                    string chatGPTReply = response.choices[0].message.content;
                    responseText.text = chatGPTReply;
                    StartCoroutine(SynthesizeAndPlay(chatGPTReply));
                }
                else
                {
                    responseText.text = "ChatGPT 沒有回應，請稍後再試。";
                }
            }
            else
            {
                responseText.text = "錯誤：" + request.error;
            }
        }
    }

    // 【修改區】TTS 合成並播放（改用 MP3）
    private IEnumerator SynthesizeAndPlay(string text)
    {
        responseText.text = "正在合成語音...";

        string ttsUrl = $"https://texttospeech.googleapis.com/v1/text:synthesize?key={googleApiKey}";

        string languageCode = IsEnglish(text) ? "en-US" : "zh-TW";
        string voiceName = IsEnglish(text) ? "en-US-Wavenet-F" : "cmn-TW-Wavenet-A";

        var ttsRequest = new
        {
            input = new { text = text },
            voice = new
            {
                languageCode = languageCode,
                name = voiceName,
                ssmlGender = "FEMALE"
            },
            audioConfig = new
            {
                audioEncoding = "MP3", // 【改成 MP3】
                speakingRate = 1.2f
            }
        };

        string jsonData = JsonConvert.SerializeObject(ttsRequest);

        UnityWebRequest request = new UnityWebRequest(ttsUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonData));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var ttsResponse = JsonConvert.DeserializeObject<TTSResponse>(request.downloadHandler.text);
            byte[] audioData = Convert.FromBase64String(ttsResponse.audioContent);

            // 【存檔】
            string path = Path.Combine(Application.persistentDataPath, "temp_tts.mp3");
            File.WriteAllBytes(path, audioData);

            // 【用 UnityWebRequestMultimedia 讀成 AudioClip】
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + path, AudioType.MPEG))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                    audioSource.clip = clip;
                    audioSource.Play();
                    Debug.Log("要語音合成的文字是：" + text);
                    responseText.text = text;
                }
                else
                {
                    Debug.LogError("載入 MP3 AudioClip 失敗: " + www.error);
                    responseText.text = "播放失敗！";
                }
            }

            // 【可選】播放完刪除檔案
            // File.Delete(path);
        }
        else
        {
            Debug.LogError("TTS 合成失敗: " + request.error);
            responseText.text = "語音合成失敗：" + request.error;
        }
    }

    private bool IsEnglish(string input)
    {
        int englishCount = 0;
        foreach (char c in input)
        {
            if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
                englishCount++;
        }
        return englishCount > input.Length / 2;
    }

    public IEnumerator SendAudioToGoogleSpeech(byte[] audioData)
    {
        if (string.IsNullOrEmpty(googleApiKey))
            yield break;

        string fullUrl = googleApiUrl + googleApiKey;

        var requestData = new
        {
            config = new
            {
                encoding = "LINEAR16",
                sampleRateHertz = 16000,
                languageCode = "zh-TW"
            },
            audio = new
            {
                content = Convert.ToBase64String(audioData)
            }
        };

        string jsonPayload = JsonConvert.SerializeObject(requestData);

        using (UnityWebRequest request = new UnityWebRequest(fullUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonPayload));
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

// 資料結構
[System.Serializable]
public class TTSResponse
{
    public string audioContent;
}

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
