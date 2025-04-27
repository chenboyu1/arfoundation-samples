using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System.IO;

public class VoiceChatObject : MonoBehaviour
{
    public AudioSource audioSource;
    public string googleApiKey = "https://speech.googleapis.com/v1/speech:recognize?key=";
    public string openAIApiKey = "https://api.openai.com/v1/chat/completions";

    private bool isProcessing = false;
    private string recognizedText = "";

    private void OnTriggerEnter(Collider other)
    {
        if (!isProcessing)
        {
            isProcessing = true;
            StartCoroutine(HandleVoiceChat());
        }
    }

    IEnumerator HandleVoiceChat()
    {
        yield return StartCoroutine(StartRecordingAndRecognize());
        yield return StartCoroutine(SendToChatGPTAndSpeak());

        isProcessing = false;
    }

    IEnumerator StartRecordingAndRecognize()
    {
        Debug.Log("開始錄音...");
        int maxDuration = 5; // 錄音長度（秒）
        string micDevice = Microphone.devices.Length > 0 ? Microphone.devices[0] : null;
        AudioClip recording = Microphone.Start(micDevice, false, maxDuration, 16000);

        yield return new WaitForSeconds(maxDuration);

        Microphone.End(micDevice);

        Debug.Log("錄音結束，開始辨識...");
        byte[] audioData = WavUtility.FromAudioClip(recording);

        string url = $"https://speech.googleapis.com/v1p1beta1/speech:recognize?key={googleApiKey}";
        string base64Audio = System.Convert.ToBase64String(audioData);

        string json = @"
        {
            'config': {
                'encoding':'LINEAR16',
                'sampleRateHertz':16000,
                'languageCode':'zh-TW'
            },
            'audio': {
                'content':'" + base64Audio + @"'
            }
        }";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json.Replace("'", "\""));
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            recognizedText = ExtractRecognizedText(request.downloadHandler.text);
            Debug.Log("辨識結果：" + recognizedText);
        }
        else
        {
            Debug.LogError("語音辨識失敗：" + request.error);
        }
    }

    IEnumerator SendToChatGPTAndSpeak()
    {
        if (string.IsNullOrEmpty(recognizedText))
        {
            Debug.LogWarning("沒有辨識到任何文字。");
            yield break;
        }

        string url = "https://api.openai.com/v1/chat/completions";
        string json = @"
        {
            'model': 'gpt-3.5-turbo',
            'messages': [
                { 'role': 'user', 'content': '" + recognizedText + @"' }
            ]
        }";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json.Replace("'", "\""));
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + openAIApiKey);

        yield return request.SendWebRequest();

        string reply = "";

        if (request.result == UnityWebRequest.Result.Success)
        {
            reply = ExtractChatGPTReply(request.downloadHandler.text);
            Debug.Log("ChatGPT回應：" + reply);
        }
        else
        {
            Debug.LogError("ChatGPT API錯誤：" + request.error);
            yield break;
        }

        // 再用 Google TTS 唸出來
        yield return StartCoroutine(PlayTTS(reply));
    }

    IEnumerator PlayTTS(string text)
    {
        string url = $"https://speech.googleapis.com/v1p1beta1/speech:recognize?key={googleApiKey}";

        string json = @"
        {
            'input': {
                'text': '" + text + @"'
            },
            'voice': {
                'languageCode': 'zh-TW',
                'name': 'zh-TW-Wavenet-B'
            },
            'audioConfig': {
                'audioEncoding': 'MP3'
            }
        }";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json.Replace("'", "\""));
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string result = request.downloadHandler.text;
            string base64Audio = ExtractAudioContent(result);
            byte[] audioData = System.Convert.FromBase64String(base64Audio);

            string filePath = Path.Combine(Application.persistentDataPath, "tts_output.mp3");
            File.WriteAllBytes(filePath, audioData);

            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + filePath, AudioType.MPEG))
            {
                yield return www.SendWebRequest();
                if (www.result == UnityWebRequest.Result.Success)
                {
                    AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                    audioSource.clip = clip;
                    audioSource.Play();
                }
                else
                {
                    Debug.LogError("播放 TTS 錯誤：" + www.error);
                }
            }
        }
        else
        {
            Debug.LogError("TTS請求錯誤：" + request.error);
        }
    }

    string ExtractRecognizedText(string json)
    {
        int transcriptIndex = json.IndexOf("\"transcript\": \"");
        if (transcriptIndex == -1) return "";
        int start = transcriptIndex + 14;
        int end = json.IndexOf("\"", start);
        return json.Substring(start, end - start);
    }

    string ExtractChatGPTReply(string json)
    {
        int contentIndex = json.IndexOf("\"content\": \"");
        if (contentIndex == -1) return "";
        int start = contentIndex + 12;
        int end = json.IndexOf("\"", start);
        return json.Substring(start, end - start);
    }

    string ExtractAudioContent(string json)
    {
        int start = json.IndexOf("audioContent") + 16;
        int end = json.IndexOf("\"", start);
        return json.Substring(start, end - start);
    }
}
