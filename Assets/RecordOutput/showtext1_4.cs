using UnityEngine;
using TMPro;

public class ShowText1_4 : MonoBehaviour
{
    public AudioSource audioSource;      // 播放語音
    public TMP_Text displayText;         // 對話框內顯示的文字
    public GameObject dialogPanel;       // 對話框 Panel

    private string textContent = @"這首詩描寫秋冬景象：荷花已盡，連能擎雨的荷葉也消失，菊花雖殘卻仍挺立霜中。詩人藉此提醒友人，一年四季皆有佳景，不必因花葉凋零而惆悵。因為此刻正是橙子金黃、橘子碧綠之時，充滿收穫與生機。詩意由蕭瑟轉向明朗，展現蘇軾樂觀開朗、善於在平凡中發現美的胸懷，也傳達積極面對人生的態度。";

    void Start()
    {
        // 初始化：顯示文字但對話框先隱藏
        if (displayText != null)
        {
            displayText.text = textContent;
            displayText.gameObject.SetActive(false);//永遠顯示文字
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
            dialogPanel.SetActive(true);//設隱藏對話框
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

        //alogPanel.SetActive(!dialogPanel.activeSelf);
    }
}
