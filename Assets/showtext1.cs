using UnityEngine;
using TMPro;
using NUnit.Framework;

public class ShowText1 : MonoBehaviour
{
    public AudioSource audioSource;         // 播放語音
    public TMP_Text displayText;            // 顯示的文字內容
    public bool showtext = false; //控制文字顯示

    private string textContent = @" 《春江花月夜》是唐代詩人張若虛創作的七言歌行，最早收錄於郭茂倩編撰的《樂府詩集》中。此詩沿用陳隋樂府舊題，運用富有生活氣息的清麗之筆，以江為場景，以月為主體，描繪了一幅幽美邈遠的春江月夜圖。

全詩共三十六句，每四句一換韻，通篇融詩情、畫意、哲理為一體，意境空明，想像奇特，語言自然雋永，韻律宛轉悠揚，為歷代文人墨客吟詠唱誦，被聞一多譽為「詩中的詩，頂峰上的頂峰」。";

    void Start()
    {
        // 初始化：隱藏文字
        if (displayText != null)
        {
            showtext = false;
            displayText.gameObject.SetActive(false);
        }

        // 自動抓 AudioSource
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
        }
        else
        {
            Debug.LogWarning("請在 Inspector 設定 AudioSource。");
        }
    }

    void Update()
    {
        // 根據播放狀態控制文字顯示
        /*if (audioSource != null && displayText != null)
        {
            if (!showtext)
            {
                displayText.gameObject.SetActive(true);
                displayText.text = textContent;
                showtext = true;
            }
            else
            {
                displayText.gameObject.SetActive(false);
                showtext = false;
            }
        }*/
    }

    public void OnClickPlay()
    {
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource 為 null。");
            return;
        }
 
        // 點擊後播放或停止語音
        if (!showtext)
        {
            audioSource.Play();
            displayText.gameObject.SetActive(true);
            displayText.text = textContent;
            showtext = true;
            Debug.Log("播放語音");
        }
        else
        {
            audioSource.Stop();
            Debug.Log("停止語音");
            displayText.gameObject.SetActive(false);
            showtext = false;
        }
    }
}
