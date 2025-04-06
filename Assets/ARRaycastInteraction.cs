using UnityEngine;
using TMPro;  // 引用 TextMesh Pro 库

public class ARRaycastInteraction : MonoBehaviour
{
    public Camera arCamera;  // 用來發射射線的 AR 相機
    public TMP_InputField inputField;  // 使用 TMP_InputField 代替 InputField
    private TouchScreenKeyboard keyboard;  // 虛擬鍵盤

    void Update()
    {
        // 發射射線，從 AR 相機的位置向前方發射
        Ray ray = new Ray(arCamera.transform.position, arCamera.transform.forward);
        RaycastHit hit;

        // 檢測射線是否與場景中的物體碰撞
        if (Physics.Raycast(ray, out hit))
        {
            // 檢查射線是否碰到 InputField（使用 TMP_InputField）
            if (hit.collider.CompareTag("InputField"))
            {
                // 當控制器觸發時（假設是 Fire1 按鈕，對應 Trigger）
                if (Input.GetButtonDown("Fire1"))
                {
                    // 顯示虛擬鍵盤，並讓用戶進行文字輸入
                    keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
                    inputField.Select();  // 選中該 TMP_InputField
                }
            }
        }
    }
}
