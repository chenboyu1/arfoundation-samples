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
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

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
        int objectID = ShowObjectInFrontOfCamera.Instance.objectID; //識別哪幅畫作
        Debug.LogWarning("objectID: " + objectID);
        if (objectID == 0)
        {
            userMessage += "這幅作品是「春江花月夜」是樂府《清商曲辭‧吳聲歌曲》的舊題之一，作者是張若虛。這首詩的原文是春江潮水連海平，海上明月共潮生。灩灩隨波千萬里，何處春江無月明？江流宛轉遶芳甸，月照花林皆似霰。空裏流霜不覺飛，汀上白沙看不見。江天一色無纖塵，皎皎空中孤月輪。江畔何人初見月，江月何年初照人？人生代代無窮已，江月年年祇相似。不知江月待何人？但見長江送流水。白雲一片去悠悠，青楓浦上不勝愁。誰家今夜扁舟子，何處相思明月樓？可憐樓上月徘徊，應照離人妝鏡臺。玉戶簾中卷不去，擣衣砧上拂還來。此時相望不相聞，願逐月華流照君。鴻雁長飛光不度，魚龍潛躍水成文。昨夜閒潭夢落花，可憐春半不還家。江水流春去欲盡，江潭落月復西斜。斜月沉沉藏海霧，碣石瀟湘無限路。不知乘月幾人歸，落月搖情滿江樹。";
            Debug.Log("「春江花月夜」是樂府《清商曲辭‧吳聲歌曲》的舊題之一");
        }
        else if (objectID == 1)
        {
            userMessage += "這幅作品是唐寅（1470－1524）致行臺大人餘山先生的書信。這首詩的原文是侍生唐寅頓首再拜。餘山大人行臺。舍弟來參。備知起居清勝。但未知公務畢期。決在何日。祇恐春來雨雪交至。亦可念也。茲有友生盧鈇。因當塘長解夫在彼派為甲長。素是富家子弟。不堪勞苦。早晚之間。萬望清目一二。足見執事平日見厚區區意。奉去乳餅五斤。所充一茶之用。相見在邇。匆匆不悉。侍生唐寅再拜。";
            Debug.Log("這幅作品是唐寅（1470－1524）致行臺大人餘山先生的書信");
        }
        else if (objectID == 2)
        {
            userMessage += "這幅作品是冷淘帖是王鞏（1048－？），向友人敘述已做了冷淘，並為受贈團餅而致謝。這首詩的原文是鞏已作冷淘一口，幸如約也。區區口敘，承惠團餅，珍感之至。有幹示之，聊陳謝誠。無煩報書為懇。 鞏再拜";
            Debug.Log("冷淘帖是王鞏（1048－？），向友人敘述已做了冷淘，並為受贈團餅而致謝。");
        }
        else if (objectID == 10)
        {
            userMessage += "「春江花月夜」是樂府《清商曲辭‧吳聲歌曲》的舊題之一";
            Debug.Log("「春江花月夜」是樂府《清商曲辭‧吳聲歌曲》的舊題之一");
        }
        userMessage += "請加以統整回答問題";
        //string userMessage = "這幅書法作品的作者是誰";
        if (!string.IsNullOrEmpty(userMessage))
        {
            //StartCoroutine(SendChineseSentence(userMessage));
            StartCoroutine(SendChatGPTRequest(userMessage));
        }
        else
        {
            responseText.text = "請輸入訊息！";
        }
    }

    private IEnumerator SendChineseSentence(string sentence)
    {
        string json = $"{{\"sentence\":\"{sentence}\"}}";
        byte[] postData = Encoding.UTF8.GetBytes(json);
        string pythonServerURL = "http://127.0.0.1:5000/analyze";

        using (UnityWebRequest request = new UnityWebRequest(pythonServerURL, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(postData);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string result = request.downloadHandler.text;
                Debug.Log("來自 Python 回應: " + result);

                // 把回應加入原本訊息後發送給 ChatGPT（或其他邏輯）
                string updatedMessage = $"{sentence} 這是資料庫中比對到的敘述，請根據以下內容調整回應加以多做介紹：{result}";
                StartCoroutine(SendChatGPTRequest(updatedMessage));
            }
            else
            {
                Debug.LogError("傳送失敗: " + request.error);
                responseText.text = "資料傳送失敗：" + request.error;
            }
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

    public void StopReply()
    {
        audioSource.Stop();
        responseText.text = "等待回應";
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
