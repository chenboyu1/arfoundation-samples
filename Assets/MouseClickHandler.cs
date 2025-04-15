using UnityEngine;

public class MouseClickHandler : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ек┴ф
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                ClickableCharacter speech = hit.collider.GetComponent<ClickableCharacter>();
                if (speech != null)
                {
                    speech.Speak();
                }
            }
        }
    }
}
