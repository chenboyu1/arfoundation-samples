using UnityEngine;
using TMPro;

public class ShowText1 : MonoBehaviour
{
    public AudioSource audioSource;      // 播放語音
    public TMP_Text displayText;         // 對話框內顯示的文字
    public GameObject dialogPanel;       // 對話框 Panel

    public AudioClip clipA;              // 音檔 A
    public AudioClip clipB;              // 音檔 B

    private string textContent = @"這段話出自清代顧炎武，強調學問應兼顧知識與行誼。他指出，求學當廣博於典籍文章，而更重要的是在行為上知恥自律。學問不僅止於個人修養，還應推及家庭、國家，乃至天下社會，皆是學之所關。此語體現「經世致用」與「知行合一」的精神，提醒人們學問須落實於品格與責任，方能成為安身立命、治理國家之本。";

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

