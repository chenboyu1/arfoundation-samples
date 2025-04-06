using UnityEngine;
using UnityEngine.InputSystem;

public class ShowTextOnXRInput : MonoBehaviour
{
    public InputActionProperty showTextAction; // �o�̳s����A���s�W������ action
    public ShowTextOnClick textController;     // �����r��ܪ� script

    void OnEnable()
    {
        showTextAction.action.Enable();
        showTextAction.action.performed += OnButtonPressed;
    }

    void OnDisable()
    {
        showTextAction.action.performed -= OnButtonPressed;
        showTextAction.action.Disable();
    }

    void OnButtonPressed(InputAction.CallbackContext context)
    {
        Debug.Log("A ��Q���U�I");
        textController.ShowText();
    }
}
