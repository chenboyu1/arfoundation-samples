using UnityEngine;
using System.IO;

public static class WavUtility
{
    public static byte[] FromAudioClip(AudioClip clip)
    {
        MemoryStream stream = new MemoryStream();
        const int headerSize = 44;

        // Placeholder for header
        for (int i = 0; i < headerSize; i++) stream.WriteByte(0);

        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        short[] intData = new short[samples.Length];
        byte[] bytesData = new byte[samples.Length * 2];

        const float rescaleFactor = 32767; // To convert float to Int16

        for (int i = 0; i < samples.Length; i++)
        {
            intData[i] = (short)(samples[i] * rescaleFactor);
            byte[] byteArr = System.BitConverter.GetBytes(intData[i]);
            byteArr.CopyTo(bytesData, i * 2);
        }

        stream.Write(bytesData, 0, bytesData.Length);

        stream.Seek(0, SeekOrigin.Begin);

        int fileSize = (int)stream.Length - 8;

        // ChunkID "RIFF"
        stream.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"), 0, 4);
        stream.Write(System.BitConverter.GetBytes(fileSize), 0, 4);
        stream.Write(System.Text.Encoding.ASCII.GetBytes("WAVE"), 0, 4);

        // Subchunk1ID "fmt "
        stream.Write(System.Text.Encoding.ASCII.GetBytes("fmt "), 0, 4);
        stream.Write(System.BitConverter.GetBytes(16), 0, 4); // Subchunk1Size
        stream.Write(System.BitConverter.GetBytes((short)1), 0, 2); // AudioFormat
        stream.Write(System.BitConverter.GetBytes((short)clip.channels), 0, 2);
        stream.Write(System.BitConverter.GetBytes(clip.frequency), 0, 4);
        stream.Write(System.BitConverter.GetBytes(clip.frequency * clip.channels * 2), 0, 4); // ByteRate
        stream.Write(System.BitConverter.GetBytes((short)(clip.channels * 2)), 0, 2); // BlockAlign
        stream.Write(System.BitConverter.GetBytes((short)16), 0, 2); // BitsPerSample

        // Subchunk2ID "data"
        stream.Write(System.Text.Encoding.ASCII.GetBytes("data"), 0, 4);
        stream.Write(System.BitConverter.GetBytes(bytesData.Length), 0, 4);

        stream.Position = 0;
        return stream.ToArray();
    }

    // 儲存 WAV 檔案至指定路徑
    public static void FromAudioClipToFile(AudioClip clip, string filePath)
    {
        byte[] wavData = FromAudioClip(clip); // 取得 WAV 資料
        File.WriteAllBytes(filePath, wavData);  // 將資料寫入指定路徑的檔案
    }
}
