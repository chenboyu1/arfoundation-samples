using UnityEngine;
using System.Collections;

public class AudioSubtitlePlayer : MonoBehaviour
{
    [System.Serializable]
    public class Subtitle
    {
        public float showTime;        // 幾秒時顯示
        public GameObject textObject; // 對應文字
    }

    public AudioSource audioSource;
    public AudioClip audioClip;
    public Subtitle[] subtitles;

    private bool isPlaying = false;

    void Start()
    {
        HideAllText();
    }

    public void PlayAudioWithSubtitles()
    {
        if (!isPlaying)
        {
            StartCoroutine(PlayRoutine());
        }
    }

    IEnumerator PlayRoutine()
    {
        isPlaying = true;
        HideAllText();

        // 確保 AudioSource 啟用
        if (!audioSource.gameObject.activeInHierarchy)
            audioSource.gameObject.SetActive(true);

        if (!audioSource.enabled)
            audioSource.enabled = true;

        audioSource.clip = audioClip;
        audioSource.Play();

        int index = 0;

        while (audioSource.isPlaying)
        {
            float currentTime = audioSource.time;

            if (index < subtitles.Length && currentTime >= subtitles[index].showTime)
            {
                subtitles[index].textObject.SetActive(true);
                index++;
            }

            yield return null;
        }

        HideAllText();
        isPlaying = false;
    }

    void HideAllText()
    {
        foreach (var sub in subtitles)
        {
            if (sub.textObject != null)
                sub.textObject.SetActive(false);
        }
    }
}
