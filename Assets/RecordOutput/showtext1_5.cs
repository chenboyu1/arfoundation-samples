using UnityEngine;
using TMPro;

public class ShowText1_5 : MonoBehaviour
{
    public AudioSource audioSource;      // 播放語音
    public TMP_Text displayText;         // 對話框內顯示的文字
    public GameObject dialogPanel;       // 對話框 Panel

    private string textContent = @"這兩句詩描寫山水寺湖的幽靜景致：風吹過山嶺，寺廟的鐘聲隨風傳來，清脆悠遠，帶有空靈的禪意；湖面如鏡，微風搖動，燈火倒映其上，樓閣水影交錯，畫面典雅而柔和。詩中聲與影、動與靜相互呼應，營造出寧靜幽遠、飄逸脫俗的意境，使人彷彿身臨其境，感受山水與人文的和諧之美。";

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
