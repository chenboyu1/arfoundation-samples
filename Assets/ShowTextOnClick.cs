using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowTextOnClick : MonoBehaviour
{
    public TMP_Text displayText;

    void Start()
    {
        displayText.gameObject.SetActive(false);
    }

    public void OnClickPlay()
    {
        Debug.Log("���sĲ�o�F�I");


        string text = @"�A�n";

        displayText.text = text;
        displayText.gameObject.SetActive(true);

        var tts = FindObjectOfType<TTSManager>();
        if (tts != null)
        {
            tts.Speak(text);
        }
        else
        {
            Debug.LogWarning("TTSManager �����A�L�k����y��");
        }
    }




    public void ToggleText()
    {
        displayText.gameObject.SetActive(!displayText.gameObject.activeSelf);
    }
}
