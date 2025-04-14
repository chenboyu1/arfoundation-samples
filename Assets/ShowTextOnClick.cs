using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowTextOnClick : MonoBehaviour
{
    public TMP_Text displayText; // ���w�n��ܪ���r

    void Start()
    {
        displayText.gameObject.SetActive(false); // �@�}�l���ä�r
    }

    public void ToggleText()
    {
        // �p�G�{�b�O��ܪ��A�A���@�U�N���áF�Ϥ���M
        displayText.gameObject.SetActive(!displayText.gameObject.activeSelf);
    }
}
