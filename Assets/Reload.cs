using UnityEngine;
using UnityEngine.SceneManagement;

public class Reload : MonoBehaviour
{
    public GameObject objectToShow;
    public GameObject[] specialSubObjects;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReloadScene()
    {
        for (int i = 0; i < 5; i++)
            specialSubObjects[i].SetActive(false);
    }
}
