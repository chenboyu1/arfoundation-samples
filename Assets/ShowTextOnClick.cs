using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShowTextOnClick : MonoBehaviour
{
    public TMP_Text displayText; // ���w�n��ܪ���r

    void Start()
    {
        displayText.gameObject.SetActive(false); // �@�}�l���ä�r
    }
    public void ShowText()
    {
        displayText.gameObject.SetActive(true); // ��ܤ�r
    }
}
