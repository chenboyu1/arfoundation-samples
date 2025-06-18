using UnityEngine;
using UnityEngine.UI;

public class ToggleImage : MonoBehaviour
{
    public GameObject imageObject; // 指定要顯示/隱藏的圖片

    private bool isVisible = false;

    void Start()
    {
        imageObject.SetActive(isVisible);
    }

    public void ToggleImageDisplay()
    {
        isVisible = !isVisible;
        imageObject.SetActive(isVisible);
    }
}