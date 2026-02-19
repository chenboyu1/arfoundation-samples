using UnityEngine;

public class ToggleMultiText : MonoBehaviour
{
    public GameObject[] textObjects; // 多段文字物件
    private bool isVisible = false;

    void Start()
    {
        foreach (GameObject obj in textObjects)
        {
            obj.SetActive(false); // 確保一開始為隱藏狀態
        }
    }

    public void ToggleTextGroup()
    {
        isVisible = !isVisible;
        Debug.Log("Toggled! New visible: " + isVisible);

        foreach (GameObject obj in textObjects)
        {
            Debug.Log("Setting " + obj.name + " active: " + isVisible);
            obj.SetActive(isVisible);
        }
    }

}
