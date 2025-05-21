using UnityEngine;
using UnityEngine.UI;
using TMPro; // Only if you're using TextMeshPro

public class FeedbackPopup : MonoBehaviour
{
    public GameObject popupPanel;
    public TextMeshProUGUI feedbackText; // Use Text if not using TMP

    public void ShowPopup(string message)
    {
        feedbackText.text = message;
        popupPanel.SetActive(true);
    }

    public void ClosePopup()
    {
        popupPanel.SetActive(false);
    }
}
