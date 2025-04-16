using UnityEngine;
using TMPro;

public class ShowTextOnClick : MonoBehaviour
{
    public AudioSource audioSource;         // 播放語音
    public TMP_Text displayText;            // 顯示的文字內容

    private string textContent = @" 明 唐寅 尺牘
此作為唐寅致行臺大人餘山先生的書信，內容中提請其多加照顧友人盧鈇，並隨信奉上五斤乳餅，以感謝對方平日的關照。乳餅在明代為常見的日用食品，鄺璠的《便民圖纂》中便詳細記載了乳餅的製作和保存方式，足見此物之普及。

信上畫有縱線，以行楷書寫，字跡秀雅而不失力度，字距和行距疏朗有致，整體呈現清新雅致的氣息。";

    void Start()
    {
        // 初始化：隱藏文字
        if (displayText != null)
        {
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
        if (audioSource != null && displayText != null)
        {
            if (audioSource.isPlaying)
            {
                displayText.gameObject.SetActive(true);
                displayText.text = textContent;
            }
            else
            {
                displayText.gameObject.SetActive(false);
            }
        }
    }

    public void OnClickPlay()
    {
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource 為 null。");
            return;
        }

        // 點擊後播放或停止語音
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
            Debug.Log("播放語音");
        }
        else
        {
            audioSource.Stop();
            Debug.Log("停止語音");
        }
    }
}
