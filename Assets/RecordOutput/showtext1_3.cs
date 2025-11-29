using UnityEngine;
using TMPro;

public class ShowText1_3 : MonoBehaviour
{
    public AudioSource audioSource;      // 播放語音
    public TMP_Text displayText;         // 對話框內顯示的文字
    public GameObject dialogPanel;       // 對話框 Panel

    public AudioClip clipA;              // 音檔 A
    public AudioClip clipB;              // 音檔 B
    public AudioClip clipC;              // 音檔 C（新增）

    private string textContent = @"這首詩描寫山間突起的烏雲驟雨，使湖水迅速漲起淹過平橋，風雨變幻的景象令詩人心中為之震動，進而聯想到錢塘江八月大潮的壯闊。詩中以「山半烏雲」「湖頭綠漲」營造逼近而急驟的動勢，再以「添方寸」點出因景而生的心境波瀾，最後以「爭似錢塘八月潮」將眼前景象推向極致，展現了以小見大、情景交融的藝術特色。";

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

    // 播放音檔 C（新增）
    public void OnClickPlayAudioC()
    {
        PlayClip(clipC);
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

    // 顯示/隱藏對話框
    public void OnClickShowDialog()
    {
        if (dialogPanel == null) return;

        // dialogPanel.SetActive(!dialogPanel.activeSelf);
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
