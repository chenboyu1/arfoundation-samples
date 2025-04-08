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

        // 檢查是否有麥克風設備可用
        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("未找到可用的麥克風設備！");
        }
    }

    public void StartRecording()
    {
        if (Microphone.devices.Length > 0)
        {
            // 開始錄音
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
        // 確保錄音已經開始
        Microphone.End(null); // 停止錄音

        // 儲存音頻檔案
        if (recordedClip != null)
        {
            SaveWavFile(filePath, recordedClip);
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

    void SaveWavFile(string path, AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogError("無法儲存 WAV：AudioClip 為空！");
            return;
        }

        // 確保 WavUtility 可用並能正確工作
        WavUtility.FromAudioClipToFile(clip, path); // 呼叫正確的儲存方法
    }
}
