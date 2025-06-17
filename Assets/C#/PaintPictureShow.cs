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

        // 額外：取得圖片尺寸 
        Image img = imageObject.GetComponent<Image>();
        if (img != null && img.sprite != null)
        {
            int width = img.sprite.texture.width;
            int height = img.sprite.texture.height;
            Debug.Log("圖片尺寸：寬 = " + width + "，高 = " + height);
        }
        else
        {
            Debug.LogWarning("圖片尚未設定 Sprite 或 Image 組件未找到");
        }
    }
}
