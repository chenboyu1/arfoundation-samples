using UnityEngine;
using UnityEngine.UI;

public class ToggleImage : MonoBehaviour
{
    public GameObject imageObject; // 要顯示/隱藏的圖片物件
    public GameObject button_front;
    public GameObject button_back;
    private bool isVisible = false;

    void Start()
    {
        imageObject.SetActive(false);
        button_front.SetActive(false);
        button_back.SetActive(false);
    }

    public void ToggleImageDisplay()
    {
        isVisible = !isVisible;
        imageObject.SetActive(isVisible);
        button_front.SetActive(isVisible);
        button_back.SetActive(isVisible);

    }
}
