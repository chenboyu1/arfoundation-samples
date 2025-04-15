using UnityEngine;
using TMPro;

public class ClickableCharacter : MonoBehaviour
{
    public string speechText = "�A�n�A�ڬO����I";

    public void Speak()
    {
        Debug.Log(speechText);

        var tts = FindObjectOfType<AndroidTTS>(); // �� TTSManager
        if (tts != null)
        {
            tts.Speak(speechText);
        }
    }
    /*
    [TextArea]
    public string dialogueText = "�A�n�I�w��Ө�ڪ��@�ɡ�";
    public TMP_Text displayText; // ��ܤ�r�� UI Text�]�Ҧp Canvas �W�� TMP_Text�^

    void OnMouseDown()
    {
        Debug.Log("�I������A�y�����e�G" + dialogueText);

        if (displayText != null)
        {
            displayText.text = dialogueText;
            displayText.gameObject.SetActive(true);
        }

        var tts = FindObjectOfType<AndroidTTS>();
        if (tts != null)
        {
            tts.Speak(dialogueText);
        }
        else
        {
            Debug.LogWarning("�䤣�� AndroidTTS�A�L�k����y��");
        }
    }*/
}
