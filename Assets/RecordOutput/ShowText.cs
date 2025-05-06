using UnityEngine;
using TMPro;

public class ShowText : MonoBehaviour
{
    public AudioSource audioSource;         // 播放語音
    public TMP_Text displayText;            // 顯示的文字內容

    private string textContent = @" 「春江花月夜」是樂府《清商曲辭‧吳聲歌曲》的舊題之一，最早的創作者不知歸誰。「吳聲歌曲」流行於長江下游的江南一帶，以六朝古都建康為中心，盛行於古代吳地，內容多是歌詠男女情愛，源流可追溯至春秋戰國時期的民歌。

魏、晉以降，吳歌被南朝樂府官署採入清商曲，將質樸的歌唱加上了管弦伴奏。隋煬帝曾用這個題名作了二首詩。到了張若虛手裡，《春江花月夜》突發異采，突破情愛、思鄉的格局。他發揮吳歌特有的沉鬱底蘊，充分展現吳歌情意深沉、語意雙關等特色，追索生命之謎，創造了不朽的藝術生命。";

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
