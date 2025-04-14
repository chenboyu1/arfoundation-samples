using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowTextOnClick : MonoBehaviour
{
    public TMP_Text displayText; // 指定要顯示的文字

    void Start()
    {
        displayText.gameObject.SetActive(false); // 一開始隱藏文字
    }

    public void ToggleText()
    {
        // 如果現在是顯示狀態，按一下就隱藏；反之亦然
        displayText.gameObject.SetActive(!displayText.gameObject.activeSelf);
    }
}
