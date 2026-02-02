using System;
using TMPro;
using UnityEngine;

public class showCV : MonoBehaviour
{
    public TMP_Text displayText;
    public TMP_Text displayTextEN;
    public GameObject frame;
    private string textContent1 = "法界佛教總會創辦人•萬佛聖城開山祖師\r\n\r\n\r\n宣化上人，俗姓白。一九一八年生於中國吉" +
        "林省雙城縣（現屬黑龍江省）人，出生前夕，其母夢見阿彌陀佛大放光明。\r\n\r\n" +
        "十二歲起，每日早晚向父母叩頭認錯，以報親恩。十九歲母逝，於墓旁結廬守孝三年，人稱「白孝子」；" +
        "同年禮上常下智老和尚出家，法名安慈，字度輪。\r\n\r\n\r\n\r\n\r\n" +
        "一九四八年，仰慕禪宗泰斗上虛下雲老和尚之德行，前往廣州南華寺參禮，雲公觀其為法門龍象，" +
        "遂任命為南華寺戒律學院監學。次年叩別雲公，前往香港弘法，並建立西樂園寺、慈興禪寺、香港佛教講堂等道場。\r\n\r\n\r\n\r\n" +
        "一九五六年，雲公於雲居山，以法脈委付上人為溈仰宗第九代嗣法人，賜法號為宣化。\r\n\r\n" +
        "一九六二年，隻身赴美，初期暫住於一地下室中，待緣而化，自號「墓中僧」。\r\n\r\n\r\n" +
        "一九六八年，機緣成熟，應華盛頓州州立大學三十餘名學生之請，於三藩市佛教講堂，開設「暑假楞嚴講修班」；" +
        "九十六天結業後，美籍青年五人懇求剃度出家，隨後到臺灣海會寺受具足戒，這是上人在美國建立僧團之始。\r\n\r\n" +
        
        "爾後僧團逐漸壯大，上人因而於一九七六年購置加州佔地四百八十八英畝的州立醫院，改建為萬佛聖城，作為國際性大道場；並在城內相繼成立育良小學、培德中學、法界佛\r\n" +
        "教大學、僧伽居士訓練班等教育機構。\r\n\r\n";
    private string textContentEN1 = "Venerable Master Hsuan Hua— The Founder of the Dharma Realm Buddhist Association, and The Founding Patriarch of the Sagely City of Ten Thousand Buddhas\r\n\r\n" +
        "Venerable Master Hua’s lay surname is Bai. He was born in 1918 in the county of Shuangcheng (Twin Cities), the province of Jilin, China. The day before he was born, his mother dreamed about Amitabha Buddha emitting a bright light.\r\n\r\n" +
        "Since the age of twelve, he bowed to his parents in the morning and at night, repenting for what he did wrong and repaying their kindness. When he was nineteen years old, his mother passed away; he sat by his mother’s grave, where he stayed for " +
        "three years to observe the mourning as an act of filial respect. Thus, people called him “Filial Son Bai.” In the same year in which he completed his three-year mourning observance, he left the home life under Venerable Master Chang Zhi and was given the Dharma name of An Tse and the ordination name of Du Lun.\r\n\r\n" +
        "In 1948, out of admiration and reverence for the virtue of Venerable Master Hsu Yun, the foremost figure in the Chan School of his time, Venerable Master Hua left for Nanhua Monastery in Guangzhou to pay homage to Venerable Master Hsu Yun. Venerable Master Hsu Yun, recognizing him as a“dragon-elephant” figure " +
        "(a Dharma-vessel) of Buddhism, appointed him the dean of Nanhua Precepts Academy. The following year, he bid farewell to Venerable Master Hsu Yun and traveled to Hong Kong to propagate the Dharma, establishing Western Bliss Garden Monastery, Cixing Chan Monastery, Hong Kong Buddhist Lecture Hall, and other branch monasteries.\r\n\r\n" +
        "In 1956, Venerable Master Hsu Yun who was dwelling on Mount Yunju entrusted and transmitted the Dharma to Venerable Master Hua, who would be the Dharma heir of the ninth generation of the Wei-Yang School.\r\n\r\n" +
        "In 1962, Venerable Master Hua traveled alone to the United States of America; at first, he temporarily dwelt in a basement, waiting for the ripening of the conditions to teach and transform beings. He called himself a “monk in the grave.”\r\n\r\n" +
        "In 1968, the conditions had ripened. At the request of more than thirty students from the University of Washington in Seattle, Venerable Master Hua set up the Summer Retreat of Shurangama Studies at the San Francisco Buddhist Lecture Hall. After the conclusion of this ninety six-day retreat, five American youths requested to have their heads shaved and leave the home-life. Soon after, they went to Haihui (SeaVast Assembly) Monastery in Taiwan to receive the complete precepts. This marked the commencement of Venerable Master Hua’s establishment of the Sangha in the United States of America.\r\n\r\n" +
        "Henceforth, the Sangha gradually grew.\r\n\r\n" +
        
        " For that reason, in 1976, Venerable Master Hua acquired the Mendocino State Hospital in California, which covered an area of 488 acres. He [then] transformed it into the Sagely City of Ten Thousand Buddhas (CTTB), making it a large international monastery" +
        "complex. At the City, he also established educational institutions such as Instilling Goodness Elementary School, Developing Virtue Secondary School, Dharma Realm Buddhist University, and the Sangha and Laity Training program, etc.\r\n\r\n";
    void Start()
    {
        frame.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Showcv()
    {
        frame.SetActive(true);
        displayText.text = textContent1;
        displayText.gameObject.SetActive(true);
        displayTextEN.text = textContentEN1;
        displayText.gameObject.SetActive(true);
    }
}
