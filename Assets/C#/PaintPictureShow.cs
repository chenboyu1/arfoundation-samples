using UnityEngine;
using UnityEngine.UI;

public class ToggleImage : MonoBehaviour
{
    public GameObject imageObject; // 要顯示/隱藏的圖片物件
    private bool isVisible = false;

    void Start()
    {
        imageObject.SetActive(false);
    }

    public void ToggleImageDisplay()
    {
        isVisible = !isVisible;
        imageObject.SetActive(isVisible);

    }
}
