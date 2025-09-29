using UnityEngine;
using TMPro;

public class ShowText1_2: MonoBehaviour
{
    public AudioSource audioSource;      // 播放語音
    public TMP_Text displayText;         // 對話框內顯示的文字
    public GameObject dialogPanel;       // 對話框 Panel

    public AudioClip clipA;              // 音檔 A
    public AudioClip clipB;              // 音檔 B

    private string textContent = @"「論書當欲心先正，學道豈容氣不平」寓意書法與修身同理。書寫之前須先端正心境，心正則筆正；而學道修行，亦當以氣度平和為本。此語強調藝由心生、書如其人，提醒人們唯有內心澄明，方能於筆墨與人生道路中達到真正境界。";

    void Start()
    {
        // 初始化文字
        if (displayText != null)
        {
            displayText.text = textContent;
            displayText.gameObject.SetActive(false);
        }

        // 初始化音源
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
        }

        // 對話框預設顯示
        if (dialogPanel != null)
        {
            dialogPanel.SetActive(true);
        }
    }

    // 播放音檔 A
    public void OnClickPlayAudioA()
    {
        PlayClip(clipA);
    }

    // 播放音檔 B
    public void OnClickPlayAudioB()
    {
        PlayClip(clipB);
    }

    // 暫停語音
    public void OnClickPauseAudio()
    {
        if (audioSource == null) return;

        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }


    // 顯示/隱藏對話框（文字永遠顯示）
    public void OnClickShowDialog()
    {
        if (dialogPanel == null) return;

        //alogPanel.SetActive(!dialogPanel.activeSelf);
    }

    // 共用的播放邏輯
    private void PlayClip(AudioClip clip)
    {
        if (audioSource == null || clip == null) return;

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        audioSource.clip = clip;
        audioSource.Play();
    }
}

