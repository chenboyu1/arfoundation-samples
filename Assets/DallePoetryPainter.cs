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
    [Header("貼圖的目標 UI Image 元件")]
    public Image targetImage;

    [Header("生成按鈕元件")]
    public Button generateButton;
    public TextMeshProUGUI buttonText;

    [Header("詩詞（中文提示語）")]
    [TextArea(3, 5)]
    public string promptText = "A peaceful bamboo forest under the moonlight";

    [Header("圖像尺寸")]
    public string imageSize = "1024x1024";

    [Header("進度條元件")]
    public Slider progressBar;

    [Header("額外控制按鈕")]
    public Button clearImageButton;
    public Button toggleImageButton; // 用來切換顯示/隱藏圖片
    public TextMeshProUGUI toggleButtonText; // 用來顯示「隱藏圖片」/「顯示圖片」文字

    private string chatGptApiKey;
    private string chatGPTJsonFilePath;

    // Start 在開始時讀取 JSON 檔案
    void Start()
    {
        progressBar.gameObject.SetActive(false); //進度條隱藏

        clearImageButton.gameObject.SetActive(false);
        toggleImageButton.gameObject.SetActive(false);

        if (clearImageButton != null)
            clearImageButton.onClick.AddListener(ClearImage);

        if (toggleImageButton != null)
            toggleImageButton.onClick.AddListener(ToggleImageVisibility);
        
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
        targetImage.color = new Color(1, 1, 1, 0); // RGBA 中 A=0 代表完全透明
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

    //1. 按下按鈕後觸發，開始整個流程。
    public void GenerateImageFromPromptButton()
    {
        progressBar.value = 0f;
        progressBar.gameObject.SetActive(true);

        if (string.IsNullOrEmpty(chatGptApiKey) || targetImage == null)
        {
            Debug.LogError("請設定 API 金鑰與貼圖物件！");
            return;
        }

        buttonText.text = "繪圖中"; //這裡按下按鈕就改字
        generateButton.interactable = false; // 禁用按鈕
        StartCoroutine(GenerateImageFromChinesePoetry());
        
    }

    //2. 準備開始翻譯
    private IEnumerator GenerateImageFromChinesePoetry()
    {
        yield return TranslatePoetryToPrompt(promptText, (prompt) =>
        {
            StartCoroutine(GenerateImageFromPrompt(prompt, () =>
            {
                buttonText.text = "繪圖";
                generateButton.interactable = true;
            }));
        });
    }

    //3. 把中文詩詞送給 ChatGPT 翻成英文提示語
    IEnumerator TranslatePoetryToPrompt(string chinesePoem, Action<string> onPromptReady)
    {
        string url = "https://api.openai.com/v1/chat/completions";
        var body = new
        {
            model = "gpt-3.5-turbo",
            messages = new object[]
            {
            new { role = "system", content = "你是一位擅長圖像提示語設計的 AI，請將輸入的中文詩詞轉為適合用於圖像生成的英文提示語，背景請設定為東方古代風格，整體畫面風格古典唯美，將所有描述詩中畫面的中文內容轉換英文並控制在900~1000字元以內，盡量精簡描述畫面。" },
            new { role = "user", content = chinesePoem }
            }
        };
        //你是一位擅長圖像提示語設計的 AI，請將輸入的中文詩詞轉為適合用於圖像生成的英文提示語，盡量精簡描述畫面，控制在 1~2 句話以內。
        //你是一位擅長圖像提示語設計的 AI，請將輸入的中文詩詞轉為適合用於圖像生成的英文提示語，將所有中文內容轉換成一千字元以下的英文描述詩中畫面"
        //你是一位擅長圖像提示語設計的 AI，請將輸入的中文詩詞轉為適合用於圖像生成的英文提示語，將所有描述詩中畫面的中文內容轉換英文並控制在900~1000字元以內，盡量精簡描述畫面。

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
            if (string.IsNullOrEmpty(prompt))
            {
                Debug.LogError("翻譯失敗，回傳內容為空");
            }
            else
            {
                Debug.Log("翻譯後的英文提示語：" + prompt);
                onPromptReady?.Invoke(prompt);
            }
        }
        else
        {
            Debug.LogError("轉換失敗：" + request.error + "\n" + request.downloadHandler.text);
        }
        progressBar.value = 0.33f;
    }

    //4. 把英文提示語送給 DALL·E 生成圖片。
    IEnumerator GenerateImageFromPrompt(string prompt, Action onComplete)
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

        progressBar.value = 0.66f;
        onComplete?.Invoke();
    }

    //5. 從生成結果中提取圖片網址。
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
    

    //6. 用網址去下載圖片，並且把圖片貼到指定的 Renderer 上。
    IEnumerator DownloadAndApplyImage(string url)
    {
        UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(url);
        yield return imageRequest.SendWebRequest();

        if (imageRequest.result == UnityWebRequest.Result.Success)
        {
            Texture2D downloadedTexture = DownloadHandlerTexture.GetContent(imageRequest);
            Rect rect = new Rect(0, 0, downloadedTexture.width, downloadedTexture.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            Sprite newSprite = Sprite.Create(downloadedTexture, rect, pivot);

            targetImage.sprite = newSprite;
            targetImage.SetNativeSize(); // 可選：依照圖片原尺寸自動調整 Image 大小
            targetImage.color = new Color(1, 1, 1, 1); // 設為完全不透明

            Debug.Log("圖片已成功套用到 UI Image！");
        }
        else
        {
            Debug.LogError("圖片下載失敗：" + imageRequest.error);
        }

        progressBar.value = 1f;
        yield return new WaitForSeconds(1f); // 可選：短暫顯示完成
        progressBar.gameObject.SetActive(false); // 關閉進度條
                                                 
        clearImageButton.gameObject.SetActive(true);// 顯示按鈕
        toggleImageButton.gameObject.SetActive(true);
        toggleButtonText.text = "隱藏圖片";
    }

    private bool isImageVisible = true;

    // 清除圖片（設為空白）
    public void ClearImage()
    {
        targetImage.sprite = null;
        targetImage.color = new Color(1, 1, 1, 0); // 確保圖片也不可見
        clearImageButton.gameObject.SetActive(false);
        toggleImageButton.gameObject.SetActive(false);
        Debug.Log("圖片已清除");
    }

    // 切換圖片顯示與隱藏
    public void ToggleImageVisibility()
    {
        isImageVisible = !isImageVisible;
        targetImage.color = new Color(1, 1, 1, isImageVisible ? 1f : 0f);
        clearImageButton.gameObject.SetActive(isImageVisible);
        toggleButtonText.text = isImageVisible ? "隱藏圖片" : "顯示圖片";
    }
}
