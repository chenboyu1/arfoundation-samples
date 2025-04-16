using UnityEngine;
using TMPro;

public class ShowTextOnClick : MonoBehaviour
{
    public AudioSource audioSource;         // ����y��
    public TMP_Text displayText;            // ��ܪ���r���e

    private string textContent = @" �� ��G ���|
���@����G�P��O�j�H�l�s���ͪ��ѫH�A���e�����Ш�h�[���U�ͤH�c�y�A���H�H�^�W����Ż�A�H�P�¹�襭�骺���ӡC�Ż�b���N���`������έ��~�A�K�d���m�K����ġ�n���K�ԲӰO���F�Ż檺�s�@�M�O�s�覡�A�������������ΡC

�H�W�e���a�u�A�H�淢�Ѽg�A�r��q���Ӥ����O�סA�r�Z�M��Z���Ԧ��P�A����e�{�M�s���P���𮧡C";

    void Start()
    {
        // ��l�ơG���ä�r
        if (displayText != null)
        {
            displayText.gameObject.SetActive(false);
        }

        // �۰ʧ� AudioSource
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
        }
        else
        {
            Debug.LogWarning("�Цb Inspector �]�w AudioSource�C");
        }
    }

    void Update()
    {
        // �ھڼ��񪬺A�����r���
        if (audioSource != null && displayText != null)
        {
            if (audioSource.isPlaying)
            {
                displayText.gameObject.SetActive(true);
                displayText.text = textContent;
            }
            else
            {
                displayText.gameObject.SetActive(false);
            }
        }
    }

    public void OnClickPlay()
    {
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource �� null�C");
            return;
        }

        // �I���Ἵ��ΰ���y��
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
            Debug.Log("����y��");
        }
        else
        {
            audioSource.Stop();
            Debug.Log("����y��");
        }
    }
}
