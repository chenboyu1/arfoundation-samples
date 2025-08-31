using UnityEngine;
using TMPro;

public class ShowText1_3 : MonoBehaviour
{
    public AudioSource audioSource;      // 播放語音
    public TMP_Text displayText;         // 對話框內顯示的文字
    public GameObject dialogPanel;       // 對話框 Panel

    private string textContent = @"這首詩描寫山間突起的烏雲驟雨，使湖水迅速漲起淹過平橋，風雨變幻的景象令詩人心中為之震動，進而聯想到錢塘江八月大潮的壯闊。詩中以「山半烏雲」「湖頭綠漲」營造逼近而急驟的動勢，再以「添方寸」點出因景而生的心境波瀾，最後以「爭似錢塘八月潮」將眼前景象推向極致，展現了以小見大、情景交融的藝術特色。";

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
