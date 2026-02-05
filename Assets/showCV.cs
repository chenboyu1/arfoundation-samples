using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class showCV : MonoBehaviour
{
    //public TMP_Text Title;
    public TMP_Text displayText;
    //public TMP_Text TitleEN;
    public TMP_Text displayTextEN;
    private List<string> pages = new List<string>();
    private List<string> pagesEN = new List<string>();
    private int currentPage = 0;
    private int currentPageEN = 0;
    private string textContent1 = @"宣化上人簡傳\r\n    法界佛教總會創辦人•萬佛聖城開山祖師\r\n宣化上人，俗姓白。一九一八年生於中國吉林省雙城縣（現屬黑龍江省）人，出生前夕，其母夢見阿彌陀佛大放光明。---PAGE---
        十二歲起，每日早晚向父母叩頭認錯，以報親恩。十九歲母逝，於墓旁結廬守孝三年，人稱「白孝子」；同年禮上常下智老和尚出家，法名安慈，字度輪。---PAGE---
        一九四八年，仰慕禪宗泰斗上虛下雲老和尚之德行，前往廣州南華寺參禮，雲公觀其為法門龍象，遂任命為南華寺戒律學院監學。次年叩別雲公，前往香港弘法，並建立西樂園寺、慈興禪寺、香港佛教講堂等道場。---PAGE---
        一九五六年，雲公於雲居山，以法脈委付上人為溈仰宗第九代嗣法人，賜法號為宣化。---PAGE---
        一九六二年，隻身赴美，初期暫住於一地下室中，待緣而化，自號「墓中僧」。---PAGE---
        一九六八年，機緣成熟，應華盛頓州州立大學三十餘名學生之請，於三藩市佛教講堂，開設「暑假楞嚴講修班」；九十六天結業後，美籍青年五人懇求剃度出家，隨後到臺灣海會寺受具足戒，這是上人在美國建立僧團之始。---PAGE---     
        爾後僧團逐漸壯大，上人因而於一九七六年購置加州佔地四百八十八英畝的州立醫院，改建為萬佛聖城，作為國際性大道場；並在城內相繼成立育良小學、培德中學、法界佛教大學、僧伽居士訓練班等教育機構。---PAGE---

        上人德行感人，又應信眾之請相續成立金輪聖寺、金佛聖寺、金峰聖寺、華嚴聖寺、法界聖城、金聖寺等二十多座道場，徧佈美國、加拿大、亞洲及澳洲等地。---PAGE---
        上人畢生的大願是弘法、譯經、教育、促進宗教交流。上人說：「只要我有一口氣在，就要講經說法。」 因此數十年如一日，說法不輟。---PAGE---
        上人又說：「將佛經翻譯成各國語言文字，把佛法播送到每一個人心裏，這才是永遠的。」因此成立譯經院，訓練弟子翻譯經典。---PAGE---
        上人對教育的看法是：「教育，就是最根本的國防！」因此積極成立中小學等各種教育機構，培育人才。---PAGE---
        上人注重佛教各派，尤其南傳和北傳的團結，以及各宗教間的交流。---PAGE---
        一九八七年，在萬佛聖城舉辦「世界宗教聯席會議」；此後成立法界宗教研究院，常與天主教、基督教等互相交流和理解。---PAGE---
        「我從虛空來，回到虛空去。」一九九五年，大慈悲普度，流血汗，不休息的上人圓寂了。---PAGE---
        上人的一生，就是一部最感人的真經，只要循著上人的足跡前進不懈，每個人都可以繼續上人的未竟之志。";

    private string textContentEN1 = @"     VENERABLE MASTER HSUAN HUA: A BRIEF INTRODUCTION\r\n   Venerable Master Hsuan Hua— The Founder of the Dharma Realm Buddhist Association, and The Founding Patriarch of the Sagely City of Ten Thousand Buddhas\r\nVenerable Master Hua’s lay surname is Bai. He was born in 1918 in the county of Shuangcheng (Twin Cities), the province of Jilin, China. The day before he was born, his mother dreamed about Amitabha Buddha emitting a bright light.---PAGE---
        Since the age of twelve, he bowed to his parents in the morning and at night, repenting for what he did wrong and repaying their kindness. When he was nineteen years old, his mother passed away; he sat by his mother’s grave, where he stayed for three years to observe the mourning as an act of filial respect. Thus, people called him “Filial Son Bai.” In the same year in which he completed his three-year mourning observance, he left the home life under Venerable Master Chang Zhi and was given the Dharma name of An Tse and the ordination name of Du Lun.---PAGE---
        In 1948, out of admiration and reverence for the virtue of Venerable Master Hsu Yun, the foremost figure in the Chan School of his time, Venerable Master Hua left for Nanhua Monastery in Guangzhou to pay homage to Venerable Master Hsu Yun. Venerable Master Hsu Yun, recognizing him as a“dragon-elephant”figure (a Dharma-vessel) of Buddhism, appointed him the dean of Nanhua Precepts Academy. The following year, he bid farewell to Venerable Master Hsu Yun and traveled to Hong Kong to propagate the Dharma, establishing Western Bliss Garden Monastery, Cixing Chan Monastery, Hong Kong Buddhist Lecture Hall, and other branch monasteries.---PAGE---
        In 1956, Venerable Master Hsu Yun who was dwelling on Mount Yunju entrusted and transmitted the Dharma to Venerable Master Hua, who would be the Dharma heir of the ninth generation of the Wei-Yang School.---PAGE---
        In 1962, Venerable Master Hua traveled alone to the United States of America; at first, he temporarily dwelt in a basement, waiting for the ripening of the conditions to teach and transform beings. He called himself a “monk in the grave.”---PAGE---
        In 1968, the conditions had ripened. At the request of more than thirty students from the University of Washington in Seattle, Venerable Master Hua set up the Summer Retreat of Shurangama Studies at the San Francisco Buddhist Lecture Hall. After the conclusion of this ninety six-day retreat, five American youths requested to have their heads shaved and leave the home-life. Soon after, they went to Haihui (SeaVast Assembly) Monastery in Taiwan to receive the complete precepts. This marked the commencement of Venerable Master Hua’s establishment of the Sangha in the United States of America. Henceforth, the Sangha gradually grew.---PAGE---        
        For that reason, in 1976, Venerable Master Hua acquired the Mendocino State Hospital in California, which covered an area of 488 acres. He [then] transformed it into the Sagely City of Ten Thousand Buddhas (CTTB), making it a large international monastery complex. At the City, he also established educational institutions such as Instilling Goodness Elementary School, Developing Virtue Secondary School, Dharma Realm Buddhist University, and the Sangha and Laity Training program, etc.---PAGE---
        
        Venerable Master’s virtuous conduct touched the lives of many, inspiring them; and at their request, Venerable Master Hua established one after another branch monasteries such as Gold Wheel Sagely Monastery, Gold Buddha Sagely Monastery, Gold Summit Sagely Monastery, Avatamsaka Sagely Monastery, Dharma Realm Sagely Monastery, and Gold Sage Monastery. A total of more than twenty branches in the U.S., Canada, Australia, and Asia were established.---PAGE---
        Venerable Master Hua’s lifelong great vows were to propagate the Dharma, translate the Buddhist canon, establish education, and promote interfaith dialogues. Venerable Master Hua said, “As long as I have a single breath left, I will continue to lecture on sutras and speak the Dharma.” Thus, he tirelessly spoke the Dharma for several decades on end.---PAGE---
        Venerable Master Hua also said, “If we can translate the Sutras into the languages of every country and deliver the message of the Buddhadharma into every person’s heart, that will be an everlasting achievement.” Thus, he established the International Translation Institute (ITI), and trained his disciples to translate the sutras.---PAGE---
        Venerable Master Hua’s view on education is: “Education is the most fundamental national defense!” Thus, in order to develop talents, he actively set up various educational institutions, such as the elementary school and the secondary school.---PAGE---
        Venerable Master Hua emphasized on the harmony and unity among various Buddhist traditions, especially that of the Theravada and Mahayana; he also stressed interfaith communication.---PAGE---
        In 1987, he held the World Religions Conference at CTTB, and later set up the Institute for World Religions at Berkeley, where interfaith dialogue activities and understanding of other religions such as Protestantism and Catholicism have been promoted.---PAGE---
        “I came from empty space, and to empty space I shall return.” Venerable Master Hua never rested, shedding blood, sweat, and tears in order to rescue beings with his impartial great kindness and compassion. In 1995, Venerable Master Hua passed into Stillness.---PAGE---
        Venerable Master’s lifetime deeds are a complete set of Sutra which is most touching. As long as [we Buddhist disciples] follow his footsteps, everyone of us can carry on Venerable Master’s unfinished vows.";

    private string textContent2 = @"     宣化上人十八大願\r\n一、願盡虛空徧法界、十方三世一切菩薩等，若有一未成佛時，我誓不取正覺。\r\n\r\n二、願盡虛空徧法界、十方三世一切緣覺等，若有一未成佛時，我誓不取正覺。---PAGE---
        三、願盡虛空徧法界、十方三世一切聲聞等，若有一未成佛時，我誓不取正覺。\r\n\r\n四、願三界諸天人等，若有一未成佛時，我誓不取正覺。---PAGE---
        五、願十方世界一切人等，若有一未成佛時，我誓不取正覺。\r\n\r\n六、願天、人、一切阿修羅等，若有一未成佛時，我誓不取正覺。\r\n---PAGE---
        七、願一切畜生界等，若有一未成佛時，我誓不取正覺。\r\n\r\n八、願一切餓鬼界等，若有一未成佛時，我誓不取正覺。---PAGE---
        九、願一切地獄界等，若有一未成佛，或地獄不空時，我誓不取正覺。\r\n\r\n十、願凡是三界諸天、仙、人、阿修羅、飛潛動植、靈界龍畜、鬼神等眾，曾經皈依我者，若有一未成佛時，我誓不取正覺。---PAGE---
        十一、願將我所應享受一切福樂，悉皆迴向，普施法界眾生。\r\n\r\n十二、願將法界眾生所有一切苦難，悉皆與我一人代受。\r\n---PAGE---
        十三、願分靈無數，普入一切不信佛法眾生心，令其改惡向善，悔過自新，皈依三寶，究竟作佛。\r\n\r\n十四、願一切眾生，見我面，乃至聞我名，悉發菩提心，速得成佛道。\r\n---PAGE---
        十五、願恪遵佛制，實行日中一食。\r\n\r\n十六、願覺諸有情，普攝群機。\r\n\r\n---PAGE---
        十七、願此生即得五眼六通，飛行自在。\r\n\r\n十八、願一切求願，必獲滿足。---PAGE---
        結云：\r\n\r\n    眾生無邊誓願度\r\n    煩惱無盡誓願斷\r\n    法門無量誓願學\r\n    佛道無上誓願成";

    private string textContentEN2 = @"VENERABLE MASTER HSUAN HUA’S\r\n    EIGHTEEN GREAT VOWS\r\n1. I vow that I will not realize right enlightenment as long as even one Bodhisattva in the three periods of time throughout the ten directions of the Dharma Realm, to the very ends of empty space, has yet not become a Buddha.\r\n\r\n2. I vow that I will not realize right enlightenment as long as even one Solitary Sage in the three periods of time throughout the ten directions of the Dharma Realm, to the very ends of empty space, has yet not become a Buddha.---PAGE---
        3. I vow that I will not realize right enlightenment as long as even one Hearer of the Teaching in the three periods of time throughout the ten directions of the Dharma Realm, to the very ends of empty space, has yet not become a Buddha.\r\n\r\n4. I vow that I will not realize right enlightenment as long as even one god in the Three Realms has yet not become a Buddha.---PAGE---
        5. I vow that I will not realize right enlightenment as long as even one human being in the worlds of the ten directions has yet not become a Buddha.\r\n\r\n6. I vow that I will not realize right enlightenment as long as even one asura among people and gods has yet not become a Buddha.---PAGE---
        7. I vow that I will not realize right enlightenment as long as even one animal has yet not become a Buddha.\r\n\r\n8. I vow that I will not realize right enlightenment as long as even one hungry ghost has yet not become a Buddha.---PAGE---
        9. I vow that I will not realize right enlightenment as long as even one being in the hells has yet not become a Buddha.\r\n\r\n10. I vow that I will not realize right enlightenment as long as even one god in the Three Realms who has taken refuge with me has yet not become a Buddha.---PAGE---
        11. I vow to dedicate all the blessings and happiness that I am due to enjoy to all the beings of the Dharma Realm.\r\n\r\n12. I vow to fully take upon myself all the anguish and hardship that all the beings in the Dharma Realm are due to suffer.---PAGE---
        13. I vow to appear in innumerable kinds of bodies in order to reach the minds of all the beings throughout the universe who do not believe in the Buddha’s Dharma, so that I may cause them to correct their faults and become good, to repent and to start anew, to take refuge with the Three Jewels and finally to become Buddhas. \r\n\r\n14. I vow that any being who sees my face or simply hears my name will immediately resolve to awaken and to follow the Path all the way to Buddhahood.---PAGE---
        15. I vow to respectfully observe the Buddha’s instruction. I vow to respectfully observe the Buddha’s instructions and to maintain the practice of eating one meal a day.\r\n\r\n16. I vow to bring all beings everywhere to enlightenment by teaching each in accord with the various capabilities of each.---PAGE---
        17. I vow, in this very life, to open the five spiritual eyes and to gain the six spiritual powers and the freedom to fly.\r\n\r\n18. I vow to make certain that that all my vows are fulfilled.---PAGE---
        To these personal vows he added the universal vows of the Bodhisattva:\r\n\r\n    Living beings are countless, I vow to liberate them all.\r\n    Afflictions are endless, I vow to end them all\r\n    Dharma-methods can’t be numbered; still, I vow to learn them all.\r\n    The Buddha’s path is unsurpassed, I vow to realize it. ";

    private string textContent3 =
        @"    法界佛教總會簡介\r\n法界佛教總會（以下簡稱法總），係宣化上人所創辦的國際性宗教及教育組織，積極致力於佛法的研習、修行、教化和實踐。---PAGE---
        法總凝聚所有四眾弟子的智慧與慈悲之力量，以弘揚佛法、翻譯經典、提倡道德教育、利樂有情為己任，俾使個人、家庭、社會、國家，乃至世界，皆能蒙受佛法的熏習，而漸趨至真、至善、至美的境地。---PAGE---
        每位參與法總的四眾弟子，均矢志奉行上人所倡導的六大宗旨：不爭、不貪、不求、不自私、不自利、不打妄語。僧眾則恪遵佛制：日中一食、衣不離體，並持戒念佛，習教參禪，和合共住，獻身佛教。---PAGE---
        法總自一九五九年成立以來，相繼成立了二十餘座道場，徧佈美、亞、澳洲，以距舊金山北部一一五英里的萬佛聖城為樞紐。各分支道場均遵守上人所立下的嚴謹家風：---PAGE---
        凍死不攀緣，餓死不化緣，\r\n窮死不求緣； 隨緣不變，\r\n不變隨緣，抱定我們三大宗旨。\r\n捨命為佛事，造命為本事，\r\n正命為僧事；即事明理，\r\n明理即事，推行祖師一脈心傳。---PAGE---
        法總的教育機構，有國際譯經學院、法界宗教研究院、僧伽居士訓練班、法界佛教大學、培德中學、育良小學等，除了積極培養弘法、翻譯及教育之傑出人才外，並推展各宗教間之交流與對話，以促進宗教間的團結與合作，共同致力於世界和平之重大責任。---PAGE---
        法總屬下的道場及機構，門戶開放，沒有人我、國籍、宗教的分別，凡是各國各教人士，願致力於仁義道德、追求真理、明心見性者，皆歡迎前來修持，共同研習！";

    private string textContentEN3 = @"THE DHARMA REALM BUDDHISTASSOCIATION:\r\n    A BRIEF INTRODUCTION\r\nThe Dharma Realm Buddhist Association (DRBA) founded by the Venerable Master Hsuan Hua (in the U.S. in 1959) is an international religious and educational organization. Members of DRBA devote themselves to the Buddhadharma’s study, investigation, practice, and dissemination, as well as how the Dharma can be applied in the daily life.---PAGE---
        DRBA unites the strength of all the fourfold assembly’s members— their wisdom and their compassion—to propagate the Buddhadharma, to translate the Buddhist canon, and to promote the virtue-oriented education. DRBA takes as its own responsibility to bring peace and bliss to all beings, bringing harmony to each and every individual, family, society, nation, country, even to the entire world, so that each of them can benefit from the influence of Buddhadharma, and gradually tends to perfection of all good in human nature.---PAGE---
        All of DRBA’s fourfold assembly resolve to follow the Six Guidelines established by the Venerable Master: no fighting, no greed, no seeking, no selfishness, no pursuit of personal advantage, and no lying. The monastics also honor the Buddha’s rules of eating only one meal a day and only before noon and always wearing the precept-sash (kasaya). They uphold the precepts, recite the Buddha’s name, and practice Chan meditation; they live in harmony, and dedicate their lives to Buddhism.---PAGE---
        Since its inception in 1959, DRBA has established over twenty branch temples in America, Asia, Australia—with the City of Ten Thousand Buddhas as its headquarter, which is located about 115 miles north of San Francisco. All these branches follow the strict credo established by the Venerable Master Hua:---PAGE---
        Freezing to death, we do not scheme. Starving to death, we do not beg.\r\nDying of poverty, we ask for nothing.We accord with conditions, but do not change.\r\nWe do not change, yet accord with conditions. We adhere firmly to our three great principles.\r\nWe renounce our lives to do the Buddha’s work. We mold our destinies as our basic duty.\r\nWe rectify our lives to fulfill the Sanghan’s role. Encountering specific matters, we understand the principles.\r\nUnderstanding the principles, we apply them to specific matters. We carry on the single pulse of the patriarchs’ mind-transmission.---PAGE---
        DRBA includes the following educational institutions: International Translation Institute, Institute for World Religions, Sangha and Laity Training Program, Dharma Realm Buddhist University, Developing Virtue Secondary School, and Instilling Goodness Elementary School. In addition to actively developing talents for propagation of the Dharma, translation of Buddhist texts, and ethical education, DRBA also promote interfaith dialogues, aiming at enhancing the unity and cooperation among religions, and work together with them for the peace of the world, taking this as its own important responsibility---PAGE---
        DRBA and all that are under its umbrella are impartially open to those of all ages, faiths, ethnic origins, and nationalities. Anyone— whichever individual it may be—is welcome to join in the pursuit of truth, and of the spiritual paths (understand the mind and see the nature), for the betterment of humankind through cultivation of virtue, especially that of humaneness and righteousness.
    ";
    bool isPaused = false;
    bool isNext = false;
    private void Start()
    {
        isPaused = true;
    }
    IEnumerator RepeatAction(int cnt)
    {
        while (currentPage < cnt)
        {
            ShowPage(currentPage, 0);
            ShowPage(currentPageEN, 1);

            currentPage++;
            currentPageEN++;

            yield return StartCoroutine(WaitWhilePaused(12f));
        }
    }

    public void Showcv()
    {
        // 只啟動一個控制中心
        displayText.gameObject.SetActive(true);
        StartCoroutine(SequenceController());
    }
    IEnumerator WaitWhilePaused(float seconds)
    {
        float timer = 0;
        while (timer < seconds)
        {
            // 1. 如果有人按了「下一頁」，立刻結束這個協程
            if (isNext)
            {
                isNext = false; // 重置開關，讓下次等待正常運作
                yield break;    // 直接跳出迴圈，結束等待
            }

            // 2. 處理暫停邏輯
            if (!isPaused)
            {
                timer += Time.deltaTime;
            }
            yield return null;
        }
    }
    public void Nextpage()
    {
        isNext = true;
    }
    public void TogglePause()
    {
        isPaused = true; // 切換 點一下暫停 / 再點一下恢復
    }
    public void TogglePauseAndStart()
    {
        Debug.Log($"Pasue {isPaused}");
        isPaused = !isPaused; // 切換 點一下暫停 / 再點一下恢復
    }
    IEnumerator SequenceController()
    {
        SetupPagesBySplit(textContent1, ref pages);
        SetupPagesBySplit(textContentEN1, ref pagesEN);
        //yield return StartCoroutine(RepeatAction(7));
        SetupPagesBySplit(textContent2, ref pages);
        SetupPagesBySplit(textContentEN2, ref pagesEN);
        yield return StartCoroutine(RepeatAction(10));
        SetupPagesBySplit(textContent3, ref pages);
        SetupPagesBySplit(textContentEN3, ref pagesEN);
        yield return StartCoroutine(RepeatAction(7));
    }
    private void SetupPagesBySplit(string content, ref List<string> pages)
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
        currentPageEN = 0;
    }

    private void ShowPage(int index, int ver)
    {
        Debug.Log($"翻頁 → Page {index}");
        if (displayText == null || pages.Count == 0) return;
        if (ver == 0)
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

    
}
