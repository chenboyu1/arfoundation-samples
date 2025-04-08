using System.IO;
using UnityEngine;

public class VoiceRecorder : MonoBehaviour
{
    private AudioClip recordedClip;
    private string filePath;

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "recordedAudio.wav");
        Debug.Log("�����ɮ��x�s���|�G" + filePath);

        // �ˬd�O�_�����J���]�ƥi��
        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("�����i�Ϊ����J���]�ơI");
        }
    }

    public void StartRecording()
    {
        if (Microphone.devices.Length > 0)
        {
            // �}�l����
            recordedClip = Microphone.Start(null, true, 30, 16000);
            Debug.Log("������...");
            if (Microphone.IsRecording(null))
            {
                Debug.Log("���b����");
            }
            else
            {
                Debug.LogError("�������ѡI");
            }
        }
        else
        {
            Debug.LogError("�L�k�Ұʿ����G�������J���]�ơI");
        }
    }

    public void StopRecording()
    {
        // �T�O�����w�g�}�l
        Microphone.End(null); // �������

        // �x�s���W�ɮ�
        if (recordedClip != null)
        {
            SaveWavFile(filePath, recordedClip);
            Debug.Log("���������A�����x�s��G" + filePath);
        }
        else
        {
            Debug.LogError("���������\�A�L�k�x�s���ɡI");
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
            Debug.LogError("�L�k�x�s WAV�GAudioClip ���šI");
            return;
        }

        // �T�O WavUtility �i�Ψïॿ�T�u�@
        WavUtility.FromAudioClipToFile(clip, path); // �I�s���T���x�s��k
    }
}
