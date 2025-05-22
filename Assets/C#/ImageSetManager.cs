using UnityEngine;

public class ImageSetNavigator : MonoBehaviour
{
    public Transform imageSetContainer; // 所有圖片組的共同父物件
    private GameObject[] imageSets;
    private int currentIndex = 0;
    int objectID = ShowObjectInFrontOfCamera.Instance.objectID; //識別哪幅畫作

    void Start()
    {
        // 自動從 imageSetContainer 拿到所有子物件
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
            imageSets[i].SetActive(i == index);
        }
        ShowObjectInFrontOfCamera.Instance.objectID = index;
    }
}
