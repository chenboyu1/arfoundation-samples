using System.IO;
using UnityEngine;

public class VoiceRecorder : MonoBehaviour
{
    private AudioClip recordedClip;
    private string filePath;

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "recordedAudio.wav");
        Debug.Log("錄音檔案儲存路徑：" + filePath);

        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("未找到可用的麥克風設備！");
        }
    }

    public void StartRecording()
    {
        if (Microphone.devices.Length > 0)
        {
            recordedClip = Microphone.Start(null, true, 30, 16000);
            Debug.Log("錄音中...");
            if (Microphone.IsRecording(null))
            {
                Debug.Log("有在錄音");
            }
            else
            {
                Debug.LogError("錄音失敗！");
            }
        }
        else
        {
            Debug.LogError("無法啟動錄音：未找到麥克風設備！");
        }
    }

    public void StopRecording()
    {
        Microphone.End(null);

        if (recordedClip != null)
        {
            WavUtility.FromAudioClipToFile(recordedClip, filePath); // 這裡直接使用新的方法
            Debug.Log("錄音完成，音檔儲存於：" + filePath);
        }
        else
        {
            Debug.LogError("錄音未成功，無法儲存音檔！");
        }
    }

    public AudioClip GetRecordedClip()
    {
        return recordedClip;
    }
}
