using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowTextOnClick : MonoBehaviour
{
    public TMP_Text displayText;

    void Start()
    {
        displayText.gameObject.SetActive(false);
    }

    public void ToggleText()
    {
        displayText.gameObject.SetActive(!displayText.gameObject.activeSelf);
    }
}
