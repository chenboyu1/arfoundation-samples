using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic; // �ޤJ Input System


public class ChatGPTManager : MonoBehaviour
{
    private string chatGptApiKey;
    private string googleApiKey;
    private string apiUrl = "https://api.openai.com/v1/chat/completions";
    private string googleApiUrl = "https://speech.googleapis.com/v1/speech:recognize?key=";

    private string chatGPTJsonFilePath;
    private string googleJsonFilePath;

    public TMP_InputField userInput; // �ϥΪ̿�J��
    public TMP_Text responseText;    // ��� ChatGPT �^���� UI ��r

    public Camera arCamera; // �Ψӵo�g�g�u����v��
    public InputActionReference rightTriggerAction; // �k��Ĳ�o���ާ@
    public float maxRaycastDistance = 10f; // �g�u�̤j�Z��
    private PointerEventData pointerEventData;
    private RaycastResult raycastResult;
    void Start()
    {
        // �j�w trigger ���s���U�ƥ�
        pointerEventData = new PointerEventData(EventSystem.current); // �ΨӰl�� UI �ƥ�
        rightTriggerAction.action.performed += ctx => OnTriggerPressed(); // �j�wĲ�o�ƥ�
        rightTriggerAction.action.Enable();
        // �]�w JSON �ɮ׸��|
        chatGPTJsonFilePath = Path.Combine(Application.streamingAssetsPath, "chatGPT API.json");
        googleJsonFilePath = Path.Combine(Application.streamingAssetsPath, "AR-MR-google_credentials.json");

        // Ū�� API ���_
        StartCoroutine(LoadApiKey(chatGPTJsonFilePath, "api_key", OnApiKeyLoaded));
        StartCoroutine(LoadApiKey(googleJsonFilePath, "private_key", OnApiKeyLoaded2));
        
    }
    void Update()
    {
        // �˴��g�u�O�_���� UI ����
        RaycastHit hit;
        Ray ray = arCamera.ScreenPointToRay(new Vector3(arCamera.pixelWidth / 2, arCamera.pixelHeight / 2)); // �q�e�������o�g�g�u
        if (Physics.Raycast(ray, out hit, maxRaycastDistance))
        {
            pointerEventData.position = hit.point;
            ExecuteRaycast();
        }
    }

    void ExecuteRaycast()
    {
        // �ϥήg�u�˴��Ӭd��UI���s
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        if (raycastResults.Count > 0)
        {
            raycastResult = raycastResults[0]; // �^���Ĥ@�ӳQ�˴��쪺 UI ����
            Debug.LogWarning("APIHit: " + raycastResult.gameObject.name);
        }
    }

    void OnTriggerPressed()
    {
        if (raycastResult.gameObject != null)
        {
            Button button = raycastResult.gameObject.GetComponent<Button>();
            if (button != null)
            {
                Debug.LogWarning("Chat button");
                //button.onClick.Invoke(); // Ĳ�o���s���I���ƥ�
            }
        }
    }
    // �^�ը�ƳB�z�[���� API Key
    void OnApiKeyLoaded(string apiKey)
    {
        if (!string.IsNullOrEmpty(apiKey))
        {
            chatGptApiKey = apiKey;  // �N���_��ȵ��ܼ�
            Debug.Log("���\Ū�� ChatGPT API ���_: " + chatGptApiKey);
        }
        else
        {
            Debug.LogError("�L�kŪ�� ChatGPT API ���_�I");
        }
    }
    void OnApiKeyLoaded2(string apiKey)
    {
        if (!string.IsNullOrEmpty(apiKey))
        {
            googleApiKey = apiKey;  // �N���_��ȵ��ܼ�
            Debug.Log("���\Ū�� google API ���_: " + chatGptApiKey);
        }
        else
        {
            Debug.LogError("�L�kŪ�� ChatGPT API ���_�I");
        }
    }
    /// <summary>
    /// Ū�����w JSON �ɮפ��� API Key
    /// </summary>
    private IEnumerator LoadApiKey(string filePath, string keyName, Action<string> callback)
    {
        // Android ���x�ݭn�ϥ� UnityWebRequest ��Ū���O�J�����
        if (filePath.Contains("://") || filePath.Contains("file://"))
        {
            UnityWebRequest www = UnityWebRequest.Get(filePath);
            yield return www.SendWebRequest(); // ���ݽШD����

            if (www.result == UnityWebRequest.Result.Success)
            {
                string jsonContent = www.downloadHandler.text;
                // �ѪR JSON
                Debug.Log("File content: " + jsonContent);

                try
                {
                    JObject json = JObject.Parse(jsonContent);
                    string apiKey = json[keyName]?.ToString(); // Ū�����w�� Key
                    callback(apiKey); // �եΦ^�նǻ����_
                }
                catch (JsonException ex)
                {
                    Debug.LogError("JSON �ѪR���~�G" + ex.Message);
                    callback(null); // �o�Ϳ��~�ɦ^�ժ�^ null
                }
            }
            else
            {
                Debug.LogError("Failed to load JSON file: " + www.error);
                callback(null); // �ШD���Ѯɦ^�ժ�^ null
            }
        }
        else
        {
            callback(null); // �p�G���|�����T�A�^�ժ�^ null
        }
    }



    public void SendMessageToChatGPT()
    {
        Debug.LogWarning("function call");
        string userMessage = userInput.text; // Ū����J�ت���r
        if (!string.IsNullOrEmpty(userMessage)) // �T�O��J���O�Ū�
        {
            Debug.LogWarning("send request");
            StartCoroutine(SendChatGPTRequest(userMessage));
        }
        else
        {
            responseText.text = "�п�J�T���I";
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
                Debug.LogWarning("API �^���G" + responseJson);

                try
                {
                    // �ѪR API �^��
                    var response = JsonConvert.DeserializeObject<ChatGPTResponse>(responseJson);
                    if (response.choices.Length > 0)
                    {
                        string chatGPTReply = response.choices[0].message.content;
                        responseText.text = chatGPTReply; // ��ܦb UI �W
                    }
                    else
                    {
                        responseText.text = "ChatGPT �S���^���A�еy��A�աC";
                    }
                }
                catch (JsonException jsonEx)
                {
                    Debug.LogError("JSON �ѪR���~�G" + jsonEx.Message);
                    responseText.text = "API �^���榡���~�I";
                }
            }
            else
            {
                Debug.LogError("API �ШD����: " + request.error);
                responseText.text = "���~�G" + request.error;
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
                languageCode = "zh-TW" // �i�令�A�n���y��
            },
            audio = new
            {
                content = System.Convert.ToBase64String(audioData) // �ন Base64
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
                Debug.Log("Google Speech-to-Text API �^���G" + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Google Speech-to-Text API ���ѡG" + request.error);
            }
        }
    }
}

// �w�q JSON �ѪR���O
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
