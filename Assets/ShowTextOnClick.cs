using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShowTextOnClick : MonoBehaviour
{
    public TMP_Text displayText; // 指定要顯示的文字

    void Start()
    {
        displayText.gameObject.SetActive(false); // 一開始隱藏文字
    }
    public void ShowText()
    {
        displayText.gameObject.SetActive(true); // 顯示文字
    }
}
