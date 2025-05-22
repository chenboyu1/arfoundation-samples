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
        [HideInInspector] 
        public bool isPlaying = false;
    }
    public VideoGroup[] videoGroups;

    public GameObject[] hiddenWhilePlaying;  // 👉 指定要在播放時隱藏的物件（如 Quad）

    private int currentPlayingIndex = -1;

    void Start()
    {
        foreach (var group in videoGroups)
        {
            group.videoPlayer.Stop();
            group.videoPlayer.Prepare(); // 預載影片
            group.rawImage.enabled = false;
            group.isPlaying = false;
        }

        // 確保起始時要顯示的物件都顯示
        SetObjectsActive(hiddenWhilePlaying, true);
    }

    void OnEnable()
    {
        foreach (var group in videoGroups)
        {
            group.videoPlayer.Stop();
            group.videoPlayer.Prepare(); // 預載影片
            Debug.Log("預載影片");
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

        if (videoGroups[index].isPlaying)
        {
            StopVideo(index);
            currentPlayingIndex = -1;
        }
        else
        {
            if (currentPlayingIndex != -1)
            {
                StopVideo(currentPlayingIndex);
            }

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

        // 播放時隱藏指定物件（如 Quad）
        SetObjectsActive(hiddenWhilePlaying, false);
    }

    private void StopVideo(int index)
    {
        var group = videoGroups[index];
        group.videoPlayer.Stop();
        group.rawImage.enabled = false;
        group.isPlaying = false;

        // 停止時還原顯示指定物件
        SetObjectsActive(hiddenWhilePlaying, true);
    }

    private void SetObjectsActive(GameObject[] objects, bool active)
    {
        if (objects == null) return;
        foreach (var obj in objects)
        {
            if (obj != null)
                obj.SetActive(active);
        }
    }
}