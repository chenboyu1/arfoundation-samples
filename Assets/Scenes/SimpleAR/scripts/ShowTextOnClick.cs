using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShowTextOnClick : MonoBehaviour
{
    public TMP_Text displayText; // ���w�n��ܪ���r
    public Camera arCamera; // �Ψӵo�g�g�u����v��
    public InputActionReference rightTriggerAction; // �k��Ĳ�o���ާ@
    public float maxRaycastDistance = 10f; // �g�u�̤j�Z��
    private PointerEventData pointerEventData;
    private RaycastResult raycastResult;
    private GraphicRaycaster graphicRaycaster; // �ΨӮg�u�˴� UI ������

    void Start()
    {
        displayText.gameObject.SetActive(false); // �@�}�l���ä�r
        // �j�w trigger ���s���U�ƥ�
        pointerEventData = new PointerEventData(EventSystem.current); // �ΨӰl�� UI �ƥ�
        rightTriggerAction.action.performed += ctx => OnTriggerPressed(); // �j�wĲ�o�ƥ�
        rightTriggerAction.action.Enable();
    }

    void Update()
    {
        // �˴��g�u�O�_���� UI ����
        RaycastHit hit;
        Ray ray = arCamera.ScreenPointToRay(new Vector3(arCamera.pixelWidth / 2, arCamera.pixelHeight / 2)); // �q�e�������o�g�g�u
        if (Physics.Raycast(ray, out hit, maxRaycastDistance))
        {
            pointerEventData.position = hit.point;
            ExecuteRaycast();
        }
    }
    void ExecuteRaycast()
    {
        // �ϥήg�u�˴��Ӭd��UI���s
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        if (raycastResults.Count > 0)
        {
            raycastResult = raycastResults[0]; // �^���Ĥ@�ӳQ�˴��쪺 UI ����
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
                button.onClick.Invoke(); // Ĳ�o���s���I���ƥ�
            }
        }
    }

    public void ShowText()
    {
        displayText.gameObject.SetActive(true); // ��ܤ�r
    }
}
