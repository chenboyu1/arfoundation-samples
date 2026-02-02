using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using TMPro;
using System;
using UnityEngine.Audio;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
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
        //SceneManager.LoadScene(0);
        //Invoke("LoadScene", 2);
        //for (int i = 0; i < 5; i++)
            specialSubObjects[0].SetActive(false);
            specialSubObjects[5].SetActive(false);
    }

    public IEnumerator LoadScene()
    {
        for (int i = 0; i < 6; i++)
            specialSubObjects[i].SetActive(false);
        yield return new WaitForSeconds(1f);
    }
}
