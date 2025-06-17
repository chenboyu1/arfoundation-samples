using UnityEngine;
using TMPro;
using NUnit.Framework;

public class ShowText2 : MonoBehaviour
{
    public AudioSource audioSource;         // 播放語音
    public TMP_Text displayText;            // 顯示的文字內容
    public bool showtext = false; //控制文字顯示

    private string textContent = @"王鞏，字定國，魏州人，與蘇軾、黃庭堅等文人時常往來。此札書風相當接近蘇軾，點畫醇厚，幸、拜等字末筆拉伸，行氣連貫，結體自然而不拘泥。
信件內容是向友人敘述已做了冷淘，並為受贈團餅而致謝。冷淘為涼冷的麵製品，唐代已有此類麵食，
杜甫便曾作過〈槐葉冷淘〉一詩。宋時冷淘已為常見的夏季麵食品，可搭配各種佐料食用。團餅為餅狀圓形的茶葉，
北宋中晚期團茶做得極為精緻，社交往來中常贈以團餅致意。";

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
