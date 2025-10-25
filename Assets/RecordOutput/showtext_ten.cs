using UnityEngine;
using TMPro;

public class ShowText_ten: MonoBehaviour
{
    public AudioSource audioSource;      // 播放語音
    public TMP_Text displayText;         // 對話框內顯示的文字
    public GameObject dialogPanel;       // 對話框 Panel

    public AudioClip clipA;              // 音檔 A
    public AudioClip clipB;              // 音檔 B

    private string textContent = @"「十法界不離一念心」意指三世一切佛皆由凡夫之心所成。「人」即指佛，因佛是從人修行而成就的。法界性非指法界本身的性質，而是眾生的內在性格，如脾氣、喜好等，皆為性之展現。一切成就皆由心所造，如來亦不離眾生一念心而成。若心願修佛法，則可證佛道；若歡喜菩薩行，則行菩薩道而成菩薩。因此，十法界皆由一念心所顯。";

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

