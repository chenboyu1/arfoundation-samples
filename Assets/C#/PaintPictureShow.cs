using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ToggleImage : MonoBehaviour
{
    public GameObject imageObject; // 要顯示/隱藏的圖片物件
    public GameObject button_front;
    public GameObject button_back;
    private bool isVisible = false;
    public AudioClip clipA;
    public AudioSource audioSource;

    void Start()
    {
        imageObject.SetActive(false);
        button_front.SetActive(false);
        button_back.SetActive(false);
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
        }
    }

    public void ToggleImageDisplay()
    {
        isVisible = !isVisible;
        imageObject.SetActive(isVisible);
        button_front.SetActive(isVisible);
        button_back.SetActive(isVisible);

    }

    public void OnClickPlayAudioA()
    {
        PlayClip(clipA);
    }

    private void PlayClip(AudioClip clip)
    {
        if (audioSource == null || clip == null) return;

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        audioSource.clip = clip;
        audioSource.Play();
    }
}