using UnityEngine;
using TMPro;

public class ShowText1_4 : MonoBehaviour
{
    public AudioSource audioSource;      // 播放語音
    public TMP_Text displayText;         // 對話框內顯示的文字
    public GameObject dialogPanel;       // 對話框 Panel

    public AudioClip clipA;              // 音檔 A
    public AudioClip clipB;              // 音檔 B

    private string textContent = @"這首詩描寫秋冬景象：荷花已盡，連能擎雨的荷葉也消失，菊花雖殘卻仍挺立霜中。詩人藉此提醒友人，一年四季皆有佳景，不必因花葉凋零而惆悵。因為此刻正是橙子金黃、橘子碧綠之時，充滿收穫與生機。詩意由蕭瑟轉向明朗，展現蘇軾樂觀開朗、善於在平凡中發現美的胸懷，也傳達積極面對人生的態度。";

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

