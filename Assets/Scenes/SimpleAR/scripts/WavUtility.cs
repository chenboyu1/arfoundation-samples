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
            // 打開 FileStream，並指定 FileMode 和 FileAccess 設置為可寫入
            fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);

            // 檢查文件流是否可寫入
            if (!fileStream.CanWrite)
            {
                Debug.LogError("無法寫入文件！");
                return;
            }

            // 創建 BinaryWriter 寫入數據
            writer = new BinaryWriter(fileStream);

            int headerSize = 44;
            int fileSize = clip.samples * clip.channels * 2 + headerSize;

            // 寫入文件頭
            WriteHeader(writer, clip, fileSize);

            // 寫入音訊數據
            WriteAudioData(writer, clip);

            Debug.Log("WAV 檔案建立成功：" + filePath);
        }
        catch (Exception ex)
        {
            Debug.LogError("寫入 WAV 失敗：" + ex.Message);
        }
        finally
        {
            // 確保 BinaryWriter 和 FileStream 釋放資源
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
        // 寫入 WAV 檔案的標頭
        writer.Write(new char[] { 'R', 'I', 'F', 'F' });
        writer.Write(fileSize - 8);
        writer.Write(new char[] { 'W', 'A', 'V', 'E' });
        writer.Write(new char[] { 'f', 'm', 't', ' ' });
        writer.Write(16); // Subchunk1Size，固定為 16
        writer.Write((ushort)1); // 音訊格式，1 表示 PCM
        writer.Write((ushort)clip.channels); // 聲道數
        writer.Write(clip.frequency); // 取樣頻率
        writer.Write(clip.frequency * clip.channels * 2); // ByteRate = 取樣頻率 * 聲道數 * 2
        writer.Write((ushort)(clip.channels * 2)); // BlockAlign = 聲道數 * 每個樣本佔的字節數（這裡是 2）
        writer.Write((ushort)16); // BitsPerSample，16 位深度
        writer.Write(new char[] { 'd', 'a', 't', 'a' }); // data chunk ID
        writer.Write(fileSize - 44); // Subchunk2Size，總資料長度
    }

    private static void WriteAudioData(BinaryWriter writer, AudioClip clip)
    {
        // 取得音訊樣本並寫入
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        if (samples.Length == 0)
        {
            Debug.LogError("錄音資料為空，無法儲存！");
        }
        else
        {
            Debug.Log("錄音資料取得成功！");
        }
        foreach (float sample in samples)
        {
            short intSample = (short)(sample * short.MaxValue); // 將浮點數樣本轉換為 16 位整數
            writer.Write(intSample); // 寫入整數樣本
        }
    }


}
