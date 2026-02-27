using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ShowText_ten : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text displayText;
    public TMP_Text displayTextEN;
    public GameObject dialogPanel;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clipA;
    public AudioClip clipB;

    // 目前頁面資料
    private List<string> pages = new List<string>();
    private List<string> pagesEN = new List<string>();
    private int currentPage = 0;
    private int currentPageEN = 0;

    // 每頁對應的開始時間（秒）
    [SerializeField]
    public List<float> pageTimesA = new List<float>()
    {
        0f, 12f, 40f, 53f, 108f, 157f, 170f, 190f, 197f, 210f, 225f, 244f
    };

    [SerializeField]
    public List<float> pageTimesB = new List<float>()
    {
        0f, 18f, 38f, 52f, 73f, 94f, 119f, 144f, 168f, 186f, 212f
    };

    private List<float> currentPageTimes;

    // 宣化
    private string textContentA = @"十界一心，不離當念；能覺此念，立登彼岸。---PAGE---
「十界一心」：佛、菩薩、聲聞、緣覺，這是四聖法界；天、人、阿修羅、地獄、餓鬼、畜生，這是六凡法界。合起來，叫十法界。
---PAGE---
這十法界從什麼地方生出來的？就從我們人現前一念心生出來的。
---PAGE---
「能覺此念」：這十法界沒有離開你這現前一念，你現前的一念你要明白了，「立登彼岸」：立刻就到彼岸了，就「摩訶般若波羅蜜」了。
---PAGE---
這法界的眾生，各有各性。豬有豬性，馬有馬性。豬，牠就姓豬；馬，就姓馬；牛，就姓牛。
---PAGE---
各有各「姓」，那是姓名的姓。這是性格的性，男人有男人的性，女人就有女人性，各有其性。
---PAGE---
那麼有的歡喜吃甜的，這是有個甜性；有的歡喜吃酸的，就有個酸性；有的歡喜吃辣的，就有一個辣性。
---PAGE---
啊！有的歡喜吃苦的，那麼我們大家就有一個苦性在這兒，你說是不是呀？
---PAGE---
我們行苦行。一行苦行，這個修行也是苦行；到了過堂吃飯的時候，也是苦行。
---PAGE---
行那苦行呢，大家就都不要落到人後邊，要跑到前面去，那麼過堂那個苦行，誰都要跑到前面去，你看是不是？
---PAGE---
你研究研究，各有各性。樹也有樹的性，花有花的性，草有草的性，各有其性，所以說「法界性」。不是說那個法界有性，是法界的眾生性。
---PAGE---
你們現在明白了沒有？以前你們都以為是法界性，現在是那法界之中的眾生性，所以才說「應觀法界性」。";

    private string textContentAEN = @"Ten Dharma Realms in a single thought, We find them only here and now; Awaken to this very thought: Just then we reach the other shore.
---PAGE---
Ten Dharma Realms in a single thought; Buddhas, Bodhisattvas, Solitary Sages, and Voice Hearers are the four Dharma Realms of Sages. Gods, humans, asuras, animals, hungry ghosts, and hell-beings are the six Dharma Realms of ordinary beings. Together, they make up the ten Dharma Realms.
---PAGE---
Where do the ten Dharma Realms come from? 
They come from the present thought in our minds.
---PAGE---
Therefore, the ten Dharma Realms are not outside of your experience but are in this immediate thought. Awaken to this very thought: Just then we reach the other shore. 
What is the other shore?
Awakening—you are simply no longer confused.
---PAGE---
Take a look at the nature of the Dharma Realm.
Actually, this phrase refers to the nature of the living beings in the Dharma Realm. Every living being of the Dharma Realm has its own nature.  Pigs have the nature of pigs; horses have the nature of horses.
---PAGE---
Men have the nature of men; women have the nature of women. Each has his or her own nature.
---PAGE---
Those who like to eat sweet food have a sweet nature; those who like to eat sour food have a sour nature; those who like hot, spicy food have a hot and spicy nature;
---PAGE---
those who like bitter food—like all of us here, we all have a bitter nature. 
Do you agree? 
---PAGE---
We practice bitter ascetic practices. Cultivation is an ascetic practice, and even going to the dining hall to eat is part of the ascetic practice.
---PAGE---
However, in this ascetic practice of eating, none of you want to be at the end; all of you race toward the front.      
Do you agree?
---PAGE---
When we look into it, we find that everything has its own nature: trees have the nature of trees; flowers have the nature of flowers; grass has the nature of grass.
So “the nature of the Dharma Realm” refers to the nature of each living being in the Dharma Realm and not the Dharma Realm’s nature.
---PAGE---
Do you understand now? In the past, you thought that it was the Dharma Realm that had its own nature. Now, you know that it is the living beings in the Dharma Realm who have their own nature.";
    // 法師
    private string textContentB = @"美國萬佛聖城開山祖師上宣下化老和尚說「十法界不離一念心」。十界一心，不離當念；能覺此念，立登彼岸。
---PAGE---
「十界一心」：佛、菩薩、聲聞、緣覺，這是四聖法界；天、人、阿修羅、地獄、餓鬼、畜生，這是六凡法界。
---PAGE---
合起來，叫十法界。這十法界從什麼地方生出來的？就從我們人現前一念心生出來的。
---PAGE---
「能覺此念」：這十法界沒有離開你這現前一念，你現前的一念你要明白了，「立登彼岸」：立刻就到彼岸了，就「摩訶般若波羅蜜」了。
---PAGE---
這個彼岸是什麼呢？就是覺悟、不迷惑了，就把無明破了。破無明，那個法身就現出來了。若人欲了知，三世一切佛；
---PAGE---
應觀法界性，一切唯心造。人人皆有佛性，皆堪作佛」，佛是人成，人道圓滿即佛道成。法身本身原無性，若有則不為法界。
---PAGE---
譬如，你的脾氣比我大，我的脾氣又比你更深，此即惡性之表現。豬有豬性，馬有馬性；男人有男人的性格，女人就有女人的性格。
---PAGE---
歡喜吃甜的，這是有個甜性；歡喜吃酸的，有個酸性；歡喜吃辣的，就有一個辣性；歡喜吃苦的，這個修行也是苦性。
---PAGE---
樹有樹的性，花有花的性，草有草的性。各有其性。以上所言種種「性」，皆包含於法界「眾生性」內。
---PAGE---
《華嚴經》上說：「萬法唯心造」，佛就是由你心造成的。你心要是修佛法，就成佛道，你心歡喜菩薩，就行菩薩道，成菩薩。
---PAGE---
乃至於你心願意墮地獄，你就往地獄那兒跑，將來就墮地獄。所以說「十法界不離一念心」。
";

    private bool isAudioPlaying = false;

    void Start()
    {
        if (displayText != null)
        {
            displayText.text = "";
            displayText.gameObject.SetActive(false);
        }

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource != null)
            audioSource.playOnAwake = false;

        if (dialogPanel != null)
            dialogPanel.SetActive(true);
    }

    void Update()
    {
        if (!isAudioPlaying || audioSource == null || currentPageTimes == null)
            return;

        float t = audioSource.time;

        // 防呆：頁數 & 時間表必須一致
        if (pages.Count != currentPageTimes.Count)
        {
            Debug.LogError($"頁數({pages.Count}) 與 pageTimes({currentPageTimes.Count}) 不一致！");
            return;
        }

        // 核心翻頁邏輯（可連續補跳）
        while (
            currentPage < currentPageTimes.Count - 1 &&
            t >= currentPageTimes[currentPage + 1]
        )
        {
            currentPage++;
            ShowPage(currentPage, 0);
            ShowPage(currentPage, 1);
            Debug.Log($"翻頁 → Page {currentPage} at {t:F2}s");
        }

        // 播放結束（只會進來一次）
        if (!audioSource.isPlaying)
        {
            isAudioPlaying = false;
            currentPage = 0;
            ShowPage(0, 0);
            ShowPage(0, 1);
            Debug.Log("播放結束 → 回第一頁");
        }
    }


    // 播放 A
    public void OnClickPlayAudioA()
    {
        SetupPagesBySplit(textContentA, ref pages, 0);
        SetupPagesBySplit(textContentAEN, ref pagesEN, 1);
        currentPageTimes = pageTimesA;
        PlayClip(clipA);
    }

    // 播放 B
    public void OnClickPlayAudioB()
    {
        SetupPagesBySplit(textContentB, ref pages, 0);
        currentPageTimes = pageTimesB;
        PlayClip(clipB);
    }

    // 暫停
    public void OnClickPauseAudio()
    {
        if (audioSource != null && audioSource.isPlaying)
            audioSource.Pause();
        isAudioPlaying = false;
    }

    // 手動下一頁
    public void OnClickNextPage()
    {
        if (currentPage < pages.Count - 1)
        {
            currentPage++;
            ShowPage(currentPage, 0);
        }
    }

    // 手動上一頁
    public void OnClickPrevPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ShowPage(currentPage, 0);
        }
    }

    // 依 ---PAGE--- 分頁
    private void SetupPagesBySplit(string content, ref List<string> pages, int language)
    {
        pages.Clear();

        string[] splitPages = content.Split(
            new string[] { "---PAGE---" },
            System.StringSplitOptions.RemoveEmptyEntries
        );

        foreach (string p in splitPages)
        {
            pages.Add(p.Trim());
        }

        currentPage = 0;
        ShowPage(0, language);
    }

    private void ShowPage(int index, int language)
    {
        if (displayText == null || pages.Count == 0) return;

        if (language == 0)
        {
            displayText.text = pages[index];
            displayText.gameObject.SetActive(true);
        }
        else
        {
            displayTextEN.text = pagesEN[index];
            displayTextEN.gameObject.SetActive(true);
        }
    }

    private void PlayClip(AudioClip clip)
    {
        if (audioSource == null || clip == null) return;

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.time = 0f;
        audioSource.Play();
        isAudioPlaying = true;

        currentPage = 0;
        ShowPage(0, 0);
        ShowPage(0, 1);
    }
}