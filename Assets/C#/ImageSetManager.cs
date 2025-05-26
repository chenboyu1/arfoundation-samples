using UnityEngine;

public class ImageSetNavigator : MonoBehaviour
{
    public Transform imageSetContainer; // 所有圖片組的父物件
    public GameObject[] specialSubObjects; // 特別要控制的副物件，從 Inspector 拉進來

    private GameObject[] imageSets;
    private int currentIndex = 0;
    private int objectID;

    void Start()
    {
        if (ShowObjectInFrontOfCamera.Instance != null)
        {
            objectID = ShowObjectInFrontOfCamera.Instance.objectID;
        }
        else
        {
            Debug.LogWarning("ShowObjectInFrontOfCamera.Instance is null!");
        }

        int childCount = imageSetContainer.childCount;
        imageSets = new GameObject[childCount];

        for (int i = 0; i < childCount; i++)
        {
            imageSets[i] = imageSetContainer.GetChild(i).gameObject;
        }
        currentIndex = Mathf.Clamp(currentIndex, 0, imageSets.Length - 1);
        ShowImageSet(currentIndex);
        Debug.Log("Total image sets: " + imageSets.Length);
    }

    public void ShowNext()
    {
        Debug.Log("ShowNext called");
        currentIndex = (currentIndex + 1) % imageSets.Length;
        ShowImageSet(currentIndex);
    }

    public void ShowPrevious()
    {
        currentIndex = (currentIndex - 1 + imageSets.Length) % imageSets.Length;
        ShowImageSet(currentIndex);
    }

    private void ShowImageSet(int index)
    {
        Debug.Log("Show image set index: " + index);

        for (int i = 0; i < imageSets.Length; i++)
        {
            bool shouldShow = (i == index);

            // 控制整組
            imageSets[i].SetActive(shouldShow);

            // 控制特別指定的副物件（如果有拉進 Inspector）
            if (specialSubObjects.Length > i && specialSubObjects[i] != null)
            {
                specialSubObjects[i].SetActive(shouldShow);
            }
        }

        if (ShowObjectInFrontOfCamera.Instance != null)
        {
            ShowObjectInFrontOfCamera.Instance.objectID = index;
        }
    }
}
