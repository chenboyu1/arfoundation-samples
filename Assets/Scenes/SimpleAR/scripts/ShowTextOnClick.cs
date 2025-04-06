using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShowTextOnClick : MonoBehaviour
{
    public TMP_Text displayText; // 指定要顯示的文字
    public Camera arCamera; // 用來發射射線的攝影機
    public InputActionReference rightTriggerAction; // 右手觸發器操作
    public float maxRaycastDistance = 10f; // 射線最大距離
    private PointerEventData pointerEventData;
    private RaycastResult raycastResult;
    private GraphicRaycaster graphicRaycaster; // 用來射線檢測 UI 的元件

    void Start()
    {
        displayText.gameObject.SetActive(false); // 一開始隱藏文字
        // 綁定 trigger 按鈕按下事件
        pointerEventData = new PointerEventData(EventSystem.current); // 用來追蹤 UI 事件
        rightTriggerAction.action.performed += ctx => OnTriggerPressed(); // 綁定觸發事件
        rightTriggerAction.action.Enable();
    }

    void Update()
    {
        // 檢測射線是否擊中 UI 元素
        RaycastHit hit;
        Ray ray = arCamera.ScreenPointToRay(new Vector3(arCamera.pixelWidth / 2, arCamera.pixelHeight / 2)); // 從畫面中間發射射線
        if (Physics.Raycast(ray, out hit, maxRaycastDistance))
        {
            pointerEventData.position = hit.point;
            ExecuteRaycast();
        }
    }
    void ExecuteRaycast()
    {
        // 使用射線檢測來查找UI按鈕
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        if (raycastResults.Count > 0)
        {
            raycastResult = raycastResults[0]; // 擷取第一個被檢測到的 UI 元素
            Debug.LogWarning("Hit: " + raycastResult.gameObject.name);
        }
    }
    void OnTriggerPressed()
    {
        Debug.LogWarning("find button");
        if (raycastResult.gameObject != null)
        {
            Button button = raycastResult.gameObject.GetComponent<Button>();
            if (button != null)
            {
                Debug.LogWarning("showtext button");
                button.onClick.Invoke(); // 觸發按鈕的點擊事件
            }
        }
    }

    public void ShowText()
    {
        displayText.gameObject.SetActive(true); // 顯示文字
    }
}
