using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using TMPro;
using System;

public class DallePoetryPainter : MonoBehaviour
{
    [Header("詩詞（英文提示語）")]
    [TextArea(3, 5)]
    public string promptText = "A peaceful bamboo forest under the moonlight";

    [Header("貼圖的目標物件")]
    public Renderer targetRenderer;

    [Header("圖像尺寸")]
    public string imageSize = "1024x1024";

    private string chatGptApiKey;
    private string chatGPTJsonFilePath;
    public Button generateButton; // 拖曳按鈕物件
    public TextMeshProUGUI buttonText; // 拖曳按鈕文字物件

    // Start 在開始時讀取 JSON 檔案
    void Start()
    {
        chatGPTJsonFilePath = Path.Combine(Application.streamingAssetsPath, "chatGPT API.json");
        StartCoroutine(LoadApiKey2(chatGPTJsonFilePath, "api_key", OnApiKeyLoaded));
        chatGptApiKey = LoadApiKey(chatGPTJsonFilePath, "api_key");
        if (string.IsNullOrEmpty(chatGptApiKey))
        {
            Debug.LogError("無法讀取 ChatGPT API 金鑰，請檢查 chatGPT API.json 檔案！");
        }
        else
        {
            Debug.Log("成功讀取 ChatGPT API 金鑰");
        }
    }

    // 讀取 API 金鑰
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

    private IEnumerator LoadApiKey2(string filePath, string keyName, Action<string> callback)
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

    // 給按鈕觸發的公開方法
    private IEnumerator GenerateImageFromChinesePoetry()
    {
        yield return TranslatePoetryToPrompt(promptText, (prompt) =>
        {
            StartCoroutine(GenerateImageFromPrompt(prompt));
        });
    }

    IEnumerator TranslatePoetryToPrompt(string chinesePoem, Action<string> onPromptReady)
    {
        string url = "https://api.openai.com/v1/chat/completions";
        var body = new
        {
            model = "gpt-3.5-turbo",
            messages = new object[]
            {
            new { role = "system", content = "你是一位擅長圖像提示語設計的 AI，請將輸入的中文詩詞轉為適合用於圖像生成的英文提示語，將所有中文內容轉換成一千字元以下的英文描述詩中畫面" },
            new { role = "user", content = chinesePoem }
            }
        };
        //你是一位擅長圖像提示語設計的 AI，請將輸入的中文詩詞轉為適合用於圖像生成的英文提示語，盡量精簡描述畫面，控制在 1~2 句話以內。

        string jsonBody = JsonConvert.SerializeObject(body);
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + chatGptApiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string result = request.downloadHandler.text;
            JObject json = JObject.Parse(result);
            string prompt = json["choices"]?[0]?["message"]?["content"]?.ToString();

            if (!string.IsNullOrEmpty(prompt))
            {
                if (prompt.Length > 1000)
                    prompt = prompt.Substring(0, 1000);

                Debug.Log("轉換後的提示語：" + prompt);
                onPromptReady(prompt);
            }
            else
            {
                Debug.LogError("轉換失敗：回傳內容為空");

                if (buttonText != null)
                    buttonText.text = "繪圖";

                if (generateButton != null)
                    generateButton.interactable = true;
            }
        }
        else
        {
            Debug.LogError("轉換失敗：" + request.error + "\n" + request.downloadHandler.text);
            if (buttonText != null)
                buttonText.text = "繪圖";

            if (generateButton != null)
                generateButton.interactable = true;
        }
    }

    IEnumerator GenerateImageFromPrompt(string prompt)
    {
        string url = "https://api.openai.com/v1/images/generations";
        string jsonBody = JsonConvert.SerializeObject(new
        {
            prompt,
            n = 1,
            size = imageSize
        });

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
        UploadHandlerRaw uploadHandlerRaw = new(bodyRaw);
        request.uploadHandler = uploadHandlerRaw;
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + chatGptApiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string result = request.downloadHandler.text;
            string imageUrl = ExtractImageUrl(result);
            Debug.Log("圖片網址：" + imageUrl);
            StartCoroutine(DownloadAndApplyImage(imageUrl));
        }
        else
        {
            Debug.LogError("圖像生成失敗：" + request.error + "\n" + request.downloadHandler.text);
        }

        if (buttonText != null)
        {
            buttonText.text = "繪圖";
        }
        if (generateButton != null)
        {
            generateButton.interactable = true; // 啟用按鈕
        }
    }

    string ExtractImageUrl(string json)
    {
        int startIndex = json.IndexOf("https://");
        int endIndex = json.IndexOf("\"", startIndex);
        if (startIndex != -1 && endIndex > startIndex)
        {
            return json.Substring(startIndex, endIndex - startIndex);
        }
        return null;
    }

    IEnumerator DownloadAndApplyImage(string url)
    {
        UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(url);
        yield return imageRequest.SendWebRequest();

        if (imageRequest.result == UnityWebRequest.Result.Success)
        {
            Texture2D downloadedTexture = DownloadHandlerTexture.GetContent(imageRequest);
            targetRenderer.material.mainTexture = downloadedTexture;
            Debug.Log("圖片已成功套用！");
        }
        else
        {
            Debug.LogError("圖片下載失敗：" + imageRequest.error);
        }
    }

    public void GenerateImageFromPromptButton()
    {
        if (string.IsNullOrEmpty(chatGptApiKey) || targetRenderer == null)
        {
            Debug.LogError("請設定 API 金鑰與貼圖物件！");
            return;
        }

        if (buttonText != null)
        {
            buttonText.text = "生成圖片中...";
        }

        if (generateButton != null)
        {
            generateButton.interactable = false; // 禁用按鈕
        }
        StartCoroutine(GenerateImageFromChinesePoetry());
    }

}
