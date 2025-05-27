using UnityEngine;

public class ImageSetManager : MonoBehaviour
{
    [Header("要控制的畫作組")]
    public GameObject targetImageSet;  // 從 Inspector 拉入要控制的畫作組

    private bool isActive = true;

    public void ToggleImageSetActive()
    {
        if (targetImageSet != null)
        {
            isActive = !isActive;
            targetImageSet.SetActive(isActive);

            Debug.Log("畫作組目前狀態：" + (isActive ? "已啟用" : "已完全關閉"));
        }
        else
        {
            Debug.LogWarning("沒有設定 targetImageSet，請到 Inspector 拉入要控制的畫作組！");
        }
    }

    public void ForceHideImageSet()
    {
        if (targetImageSet != null)
        {
            isActive = false;
            targetImageSet.SetActive(false);

            Debug.Log("畫作組已強制完全關閉");
        }
    }

    public void ForceShowImageSet()
    {
        if (targetImageSet != null)
        {
            isActive = true;
            targetImageSet.SetActive(true);

            Debug.Log("畫作組已強制開啟");
        }
    }
}
