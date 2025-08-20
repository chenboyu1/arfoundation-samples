using UnityEngine;
using TMPro;

public class ShowText1 : MonoBehaviour
{
    public AudioSource audioSource;      // 播放語音
    public TMP_Text displayText;         // 對話框內顯示的文字
    public GameObject dialogPanel;       // 對話框 Panel

    private string textContent = @"顧炎武（字亭林，1613－1682）是明末清初的重要思想家、經學家與史學家，主張“天下興亡，匹夫有責”，強調實學與道德實踐，反對空談與脫離現實的學問。他說：“博學於文，行己有恥，自一身以至於天下國家，皆學之事也。”意思是要廣泛學習知識，行事要有羞恥心和自我約束，學問不僅用於自身修養，還應推及家庭、社會乃至國家，體現“修身齊家治國平天下”的理念。顧炎武強調學問與德行並重，提倡從個人修養開始，逐步承擔社會與國家的責任，並主張知識應與實際生活結合。這種“知行合一”的精神在現代仍具啟發意義，提醒我們學習不僅為了考試或工作，更應提升品德、對社會有所貢獻。";

    void Start()
    {
        // 初始化：顯示文字但對話框先隱藏
        if (displayText != null)
        {
            displayText.text = textContent;
            displayText.gameObject.SetActive(true); // 永遠顯示文字
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
        }

        if (dialogPanel != null)
        {
            dialogPanel.SetActive(false); // 預設隱藏對話框
        }
    }

    // 按下播放語音按鈕
    public void OnClickPlayAudio()
    {
        if (audioSource == null) return;

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    // 按下暫停語音按鈕
    public void OnClickPauseAudio()
    {
        if (audioSource == null) return;

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    // 顯示/隱藏對話框（文字永遠顯示）
    public void OnClickShowDialog()
    {
        if (dialogPanel == null) return;

        dialogPanel.SetActive(!dialogPanel.activeSelf);
    }
}
