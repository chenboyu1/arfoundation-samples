using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ShowText_ten : MonoBehaviour
{
    public AudioSource audioSource;
    public TMP_Text displayText;
    public GameObject dialogPanel;

    public AudioClip clipA;
    public AudioClip clipB;

 
    public int charsPerPage = 100;  // 每頁大約 180 字，可依需求調整
    private List<string> pages = new List<string>();
    private int currentPage = 0;

  
    private string textContentA = @"十界一心，不離當念；能覺此念，立登彼岸。「十界一心」：佛、菩薩、聲聞、緣覺，這是四聖法界；天、人、阿修羅、地獄、餓鬼、畜生，這是六凡法界。合起來，叫十法界。這十法界從什麼地方生出來的？就從我們人現前一念心生出來的。「能覺此念」：這十法界沒有離開你這現前一念，你現前的一念你要明白了，「立登彼岸」：立刻就到彼岸了，就「摩訶般若波羅蜜」了。這法界的眾生，各有各性。豬有豬性，馬有馬性。豬，牠就姓豬；馬，就姓馬；牛，就姓牛。各有各「姓」，那是姓名的姓。這是性格的性，男人有男人的性，女人就有女人性，各有其性。那麼有的歡喜吃甜的，這是有個甜性；有的歡喜吃酸的，就有個酸性；有的歡喜吃辣的，就有一個辣性。啊！有的歡喜吃苦的，那麼我們大家就有一個苦性在這兒，你說是不是呀？我們行苦行。一行苦行，這個修行也是苦行；到了過堂吃飯的時候，也是苦行。行那苦行呢，大家就都不要落到人後邊，要跑到前面去，那麼過堂那個苦行，誰都要跑到前面去，你看是不是？你研究研究，各有各性。樹也有樹的性，花有花的性，草有草的性，各有其性，所以說「法界性」。不是說那個法界有性，是法界的眾生性。你們現在明白了沒有？以前你們都以為是法界性，現在是那法界之中的眾生性，所以才說「應觀法界性」。";
    private string textContentB = @"美國萬佛聖城開山祖師上宣下化老和尚說「十法界不離一念心」。十界一心，不離當念；能覺此念，立登彼岸。「十界一心」：佛、菩薩、聲聞、緣覺，這是四聖法界；天、人、阿修羅、地獄、餓鬼、畜生，這是六凡法界。合起來，叫十法界。這十法界從什麼地方生出來的？就從我們人現前一念心生出來的。「能覺此念」：這十法界沒有離開你這現前一念，你現前的一念你要明白了，「立登彼岸」：立刻就到彼岸了，就「摩訶般若波羅蜜」了。這個彼岸是什麼呢？就是覺悟、不迷惑了，就把無明破了。破無明，那個法身就現出來了。若人欲了知，三世一切佛；　　應觀法界性，一切唯心造。人人皆有佛性，皆堪作佛」，佛是人成，人道圓滿即佛道成。法身本身原無性，若有則不為法界。譬如，你的脾氣比我大，我的脾氣又比你更深，此即惡性之表現。豬有豬性，馬有馬性；男人有男人的性格，女人就有女人的性格。歡喜吃甜的，這是有個甜性；歡喜吃酸的，有個酸性；歡喜吃辣的，就有一個辣性；歡喜吃苦的，就有一個苦性，這個修行也是苦性。樹有樹的性，花有花的性，草有草的性。各有其性。以上所言種種「性」，皆包含於法界「眾生性」內。《華嚴經》上說：「萬法唯心造」，佛就是由你心造成的。你心要是修佛法，就成佛道，你心歡喜菩薩，就行菩薩道，成菩薩。乃至於你心願意墮地獄，你就往地獄那兒跑，將來就墮地獄。所以說「十法界不離一念心」。";


    void Start()
    {
        if (displayText != null)
        {
            displayText.text = "";
            displayText.gameObject.SetActive(false);
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
        }

        if (dialogPanel != null)
        {
            dialogPanel.SetActive(true);
        }
    }

    public void OnClickPlayAudioA()
    {
        PlayClip(clipA);
        SetupPages(textContentA);
        ShowPage(0);
    }

    public void OnClickPlayAudioB()
    {
        PlayClip(clipB);
        SetupPages(textContentB);
        ShowPage(0);
    }


    public void OnClickPauseAudio()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    public void OnClickNextPage()
    {
        if (currentPage < pages.Count - 1)
        {
            currentPage++;
            ShowPage(currentPage);
        }
    }

    //上一頁
    public void OnClickPrevPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ShowPage(currentPage);
        }
    }

    //自動依字數分頁
    private void SetupPages(string content)
    {
        pages.Clear();

        for (int i = 0; i < content.Length; i += charsPerPage)
        {
            int len = Mathf.Min(charsPerPage, content.Length - i);
            pages.Add(content.Substring(i, len));
        }

        currentPage = 0;
    }

    //顯示某頁
    private void ShowPage(int pageIndex)
    {
        if (displayText == null || pages.Count == 0) return;

        currentPage = pageIndex;
        displayText.text = pages[currentPage];
        displayText.gameObject.SetActive(true);
    }

    // 共用播放邏輯
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
