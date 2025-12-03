using System;
using UnityEngine;
using UnityEngine.UI;

public class ImageGallery : MonoBehaviour
{
    [Header("圖片顯示區域")]
    public Image displayImage;   // Unity UI 的 Image，用來顯示圖片

    [Header("圖片集")]
    public Sprite[] images;      // 放入所有要切換的圖片

    private int currentIndex = 0;

    public AudioClip[] clip;
    public AudioSource audioSource;

    void Start()
    {
        if (images.Length > 0)
        {
            ShowImage(currentIndex);
        }
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
        }
    }

    public void NextImage()
    {
        if (images.Length == 0) return;

        currentIndex++;
        if (currentIndex >= images.Length)
            currentIndex = 0;

        ShowImage(currentIndex);
        if (audioSource == null || clip == null) return;

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        audioSource.clip = clip[currentIndex];
        audioSource.Play();
    }

    public void PreviousImage()
    {
        if (images.Length == 0) return;

        currentIndex--;
        if (currentIndex < 0)
            currentIndex = images.Length - 1;

        ShowImage(currentIndex);
        if (audioSource == null || clip == null) return;

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        audioSource.clip = clip[currentIndex];
        audioSource.Play();
    }

    void ShowImage(int index)
    {
        displayImage.sprite = images[index];
    }
}
