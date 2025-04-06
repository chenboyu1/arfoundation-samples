using System;
using System.IO;
using UnityEngine;

public static class WavUtility
{
    public static void FromAudioClipToFile(AudioClip clip, string filePath)
    {
        FileStream fileStream = null;
        BinaryWriter writer = null;

        try
        {
            // ���} FileStream�A�ë��w FileMode �M FileAccess �]�m���i�g�J
            fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);

            // �ˬd���y�O�_�i�g�J
            if (!fileStream.CanWrite)
            {
                Debug.LogError("�L�k�g�J���I");
                return;
            }

            // �Ы� BinaryWriter �g�J�ƾ�
            writer = new BinaryWriter(fileStream);

            int headerSize = 44;
            int fileSize = clip.samples * clip.channels * 2 + headerSize;

            // �g�J����Y
            WriteHeader(writer, clip, fileSize);

            // �g�J���T�ƾ�
            WriteAudioData(writer, clip);

            Debug.Log("WAV �ɮ׫إߦ��\�G" + filePath);
        }
        catch (Exception ex)
        {
            Debug.LogError("�g�J WAV ���ѡG" + ex.Message);
        }
        finally
        {
            // �T�O BinaryWriter �M FileStream ����귽
            if (writer != null)
            {
                writer.Close();
            }

            if (fileStream != null)
            {
                fileStream.Close();
            }
        }
    }

    private static void WriteHeader(BinaryWriter writer, AudioClip clip, int fileSize)
    {
        // �g�J WAV �ɮת����Y
        writer.Write(new char[] { 'R', 'I', 'F', 'F' });
        writer.Write(fileSize - 8);
        writer.Write(new char[] { 'W', 'A', 'V', 'E' });
        writer.Write(new char[] { 'f', 'm', 't', ' ' });
        writer.Write(16); // Subchunk1Size�A�T�w�� 16
        writer.Write((ushort)1); // ���T�榡�A1 ��� PCM
        writer.Write((ushort)clip.channels); // �n�D��
        writer.Write(clip.frequency); // �����W�v
        writer.Write(clip.frequency * clip.channels * 2); // ByteRate = �����W�v * �n�D�� * 2
        writer.Write((ushort)(clip.channels * 2)); // BlockAlign = �n�D�� * �C�Ӽ˥������r�`�ơ]�o�̬O 2�^
        writer.Write((ushort)16); // BitsPerSample�A16 ��`��
        writer.Write(new char[] { 'd', 'a', 't', 'a' }); // data chunk ID
        writer.Write(fileSize - 44); // Subchunk2Size�A�`��ƪ���
    }

    private static void WriteAudioData(BinaryWriter writer, AudioClip clip)
    {
        // ���o���T�˥��üg�J
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        if (samples.Length == 0)
        {
            Debug.LogError("������Ƭ��šA�L�k�x�s�I");
        }
        else
        {
            Debug.Log("������ƨ��o���\�I");
        }
        foreach (float sample in samples)
        {
            short intSample = (short)(sample * short.MaxValue); // �N�B�I�Ƽ˥��ഫ�� 16 ����
            writer.Write(intSample); // �g�J��Ƽ˥�
        }
    }


}
