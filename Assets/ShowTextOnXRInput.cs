using UnityEngine;
using UnityEngine.InputSystem;

public class ShowTextOnXRInput : MonoBehaviour
{
    public InputActionProperty showTextAction; // 這裡連結到你剛剛新增的那個 action
    public ShowTextOnClick textController;     // 控制文字顯示的 script

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
        Debug.Log("A 鍵被按下！");
        textController.ShowText();
    }
}
