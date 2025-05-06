using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoToggle : MonoBehaviour
{
    [System.Serializable]
    public class VideoGroup
    {
        public string name;
        public VideoPlayer videoPlayer;
        public RawImage rawImage;
        [HideInInspector] public bool isPlaying = false;
    }

    public VideoGroup[] videoGroups;

    private int currentPlayingIndex = -1;

    void Start()
    {
        // 一開始全部隱藏並停止
        foreach (var group in videoGroups)
        {
            group.videoPlayer.Stop();
            group.rawImage.enabled = false;
            group.isPlaying = false;
        }
    }

    public void ToggleVideoByIndex(int index)
    {
        if (index < 0 || index >= videoGroups.Length)
        {
            Debug.LogWarning("無效的影片索引");
            return;
        }

        // 如果點到的是正在播放的影片，則關閉它
        if (videoGroups[index].isPlaying)
        {
            StopVideo(index);
            currentPlayingIndex = -1;
        }
        else
        {
            // 關閉其他播放中的影片
            if (currentPlayingIndex != -1)
            {
                StopVideo(currentPlayingIndex);
            }

            // 播放選取的影片
            PlayVideo(index);
            currentPlayingIndex = index;
        }
    }

    private void PlayVideo(int index)
    {
        var group = videoGroups[index];
        group.rawImage.enabled = true;
        group.videoPlayer.Play();
        group.isPlaying = true;
    }

    private void StopVideo(int index)
    {
        var group = videoGroups[index];
        group.videoPlayer.Stop();
        group.rawImage.enabled = false;
        group.isPlaying = false;
    }
}
