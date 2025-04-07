using System.IO;
using UnityEngine;
using Newtonsoft.Json.Linq;

public static class GoogleApiKeyLoader
{
    private static string apiKey;

    public static string GetApiKey()
    {
        if (!string.IsNullOrEmpty(apiKey))
            return apiKey; // 如果已經讀取過，就直接返回

        string filePath = Path.Combine(Application.streamingAssetsPath, "AR-MR-google_credentials.json");

        if (!File.Exists(filePath))
        {
            Debug.LogError("Google API Key JSON 檔案未找到：" + filePath);
            return null;
        }

        string jsonContent = File.ReadAllText(filePath);
        JObject json = JObject.Parse(jsonContent);
        apiKey = json["api_key"]?.ToString(); // 讀取 "api_key" 欄位

        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("Google API Key JSON 檔案內沒有 'api_key' 欄位！");
        }

        return apiKey;
    }
}
