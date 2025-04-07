using System.IO;
using UnityEngine;
using Newtonsoft.Json.Linq;

public static class GoogleApiKeyLoader
{
    private static string apiKey;

    public static string GetApiKey()
    {
        if (!string.IsNullOrEmpty(apiKey))
            return apiKey; // �p�G�w�gŪ���L�A�N������^

        string filePath = Path.Combine(Application.streamingAssetsPath, "AR-MR-google_credentials.json");

        if (!File.Exists(filePath))
        {
            Debug.LogError("Google API Key JSON �ɮץ����G" + filePath);
            return null;
        }

        string jsonContent = File.ReadAllText(filePath);
        JObject json = JObject.Parse(jsonContent);
        apiKey = json["api_key"]?.ToString(); // Ū�� "api_key" ���

        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("Google API Key JSON �ɮפ��S�� 'api_key' ���I");
        }

        return apiKey;
    }
}
