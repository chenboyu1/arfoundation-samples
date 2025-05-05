using UnityEngine;
using System.IO;

public static class WavUtility
{
    public static AudioClip ToAudioClip(byte[] data, int sampleRate = 16000)
    {
        float[] floatData = ConvertByteToFloat(data);
        AudioClip audioClip = AudioClip.Create("TTS_AudioClip", floatData.Length, 1, sampleRate, false);
        audioClip.SetData(floatData, 0);
        return audioClip;
    }

    private static float[] ConvertByteToFloat(byte[] array)
    {
        int len = array.Length / 2;
        float[] floatArr = new float[len];
        for (int i = 0; i < len; i++)
        {
            short sample = (short)(array[i * 2] | (array[i * 2 + 1] << 8));
            floatArr[i] = sample / 32768.0f;
        }
        return floatArr;
    }

    public static byte[] FromAudioClip(AudioClip clip)
    {
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        byte[] bytesData = new byte[samples.Length * 2];
        for (int i = 0; i < samples.Length; i++)
        {
            short value = (short)(samples[i] * 32767);
            bytesData[i * 2] = (byte)(value & 0xFF);
            bytesData[i * 2 + 1] = (byte)((value >> 8) & 0xFF);
        }

        return bytesData;
    }

    public static void FromAudioClipToFile(AudioClip clip, string filePath)
    {
        byte[] wavData = FromAudioClip(clip);

        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        {
            // WAV 標頭
            int headerSize = 44;
            int fileSize = wavData.Length + headerSize - 8;

            fileStream.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"), 0, 4);
            fileStream.Write(System.BitConverter.GetBytes(fileSize), 0, 4);
            fileStream.Write(System.Text.Encoding.ASCII.GetBytes("WAVE"), 0, 4);
            fileStream.Write(System.Text.Encoding.ASCII.GetBytes("fmt "), 0, 4);
            fileStream.Write(System.BitConverter.GetBytes(16), 0, 4); // Subchunk1Size
            fileStream.Write(System.BitConverter.GetBytes((short)1), 0, 2); // PCM
            fileStream.Write(System.BitConverter.GetBytes((short)clip.channels), 0, 2);
            fileStream.Write(System.BitConverter.GetBytes(clip.frequency), 0, 4);
            int byteRate = clip.frequency * clip.channels * 2;
            fileStream.Write(System.BitConverter.GetBytes(byteRate), 0, 4);
            short blockAlign = (short)(clip.channels * 2);
            fileStream.Write(System.BitConverter.GetBytes(blockAlign), 0, 2);
            fileStream.Write(System.BitConverter.GetBytes((short)16), 0, 2); // bits per sample
            fileStream.Write(System.Text.Encoding.ASCII.GetBytes("data"), 0, 4);
            fileStream.Write(System.BitConverter.GetBytes(wavData.Length), 0, 4);

            // 音訊資料
            fileStream.Write(wavData, 0, wavData.Length);
        }
    }
}
