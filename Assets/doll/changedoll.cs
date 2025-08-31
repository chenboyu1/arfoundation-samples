using UnityEngine;

public class changedoll : MonoBehaviour
{
    public GameObject objectA;  // 第一個物件
    public GameObject objectB;  // 第二個物件

    void Start()
    {
        objectA.SetActive(true);  // 一開始顯示 objectA
        objectB.SetActive(false); // 一開始隱藏 objectB
    }

    public void Toggle()
    {
        if (objectA.activeSelf)
        {
            objectA.SetActive(false);
            objectB.SetActive(true);
        }
        else
        {
            objectA.SetActive(true);
            objectB.SetActive(false);
        }
    }
}
