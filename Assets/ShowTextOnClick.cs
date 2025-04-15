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

        /*
         �� ��G ���|
        �G��339-8
        �ȥ�

        ���@����G�]1470��1524�^�P��O�j�H�l�s���ͪ��ѫH�A���e�����Ш�h�[���U�ͤH�]�Ϊ��͡^�c�y�]�w���^�A���H�H�^�W����Ż�A�H�P�¹�襭�骺���ӡC�Ż�]�ΧY���ŹT�^�b���N���`������έ��~�A�K�]�}�������^�d�]�w�����^�]��1458��1521�^�m�K����ġ�n���K�ԲӰO���F�Ż檺�s�@�M�O�s�覡�A�������������ΡC

        �H�W�e���a�u�A�H�淢�Ѽg�A�r��q���Ӥ����O�סA�r�Z�M��Z���Ԧ��P�A����e�{�M�s���P���𮧡C
         */

        // ��ܦb�e���W
        displayText.text = text;
        displayText.gameObject.SetActive(true);

        // �P�B��X�� Console
        Debug.Log("�y�����e: " + text);

        var tts = FindObjectOfType<AndroidTTS>();
        if (tts != null)
        {
            tts.Speak(text);
        }
        else
        {
            Debug.LogWarning("AndroidTTS �����A�L�k����y��");
        }
    }

    public void ToggleText()
    {
        displayText.gameObject.SetActive(!displayText.gameObject.activeSelf);
    }
}
