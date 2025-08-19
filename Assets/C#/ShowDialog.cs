using UnityEngine;
using TMPro;

public class ShowDialog : MonoBehaviour
{
    public GameObject dialogPanel;   // 對話框 Panel
    public TMP_Text dialogText;      // 對話框裡的文字

    [TextArea(3, 10)]
    public string textContent = "顧炎武（字亭林，1613－1682）是明末清初的重要思想家、經學家與史學家，主張“天下興亡，匹夫有責”，強調實學與道德實踐，反對空談與脫離現實的學問。他說：“博學於文，行己有恥，自一身以至於天下國家，皆學之事也。”" +
        "意思是要廣泛學習知識，行事要有羞恥心和自我約束，學問不僅用於自身修養，還應推及家庭、社會乃至國家，體現“修身齊家治國平天下”的理念。" +
        "顧炎武強調學問與德行並重，提倡從個人修養開始，逐步承擔社會與國家的責任，並主張知識應與實際生活結合。這種“知行合一”的精神在現代仍具啟發意義，提醒我們學習不僅為了考試或工作，更應提升品德、對社會有所貢獻。";

    public void OnClickShowDialog()
    {
        dialogPanel.SetActive(true);      // 顯示對話框
        dialogText.text = textContent;    // 顯示文字
    }

    public void OnClickCloseDialog()
    {
        dialogPanel.SetActive(false);     // 關閉對話框
    }
}
