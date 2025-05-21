using TMPro;
using UnityEngine;

public class CardCounter : MonoBehaviour
{
    public static CardCounter Instance { get; private set; }

    private int _runningCount = 0;
    private int _cardsDealt = 0;

    [SerializeField] private Shoe _shoe;

    [SerializeField] private TMP_Text runningCountText;
    [SerializeField] private TMP_Text trueCountText;

    private int runningCount;
    private float trueCount;

    public int GetRunningCount() => runningCount;
    public float GetTrueCount() => trueCount;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public int RunningCount => _runningCount;

    public float TrueCount
    {
        get
        {
            int decksRemaining = Mathf.Max(1, (_shoe.CurrentShoe.Count / 52));
            return (float)_runningCount / decksRemaining;
        }
    }

    public void ResetCounter()
    {
        _runningCount = 0;
        _cardsDealt = 0;
    }

    public void CardDealt(CardSO card, string source = "Unknown")
    {
        _cardsDealt++;
        int countValue = GetCardCountValue(card);
        _runningCount += countValue;

       // Debug.Log($"[CardCounter] Source: {source}, Dealt: {card.Name}, Value: {card.Value}, Count Value: {countValue}, Running Count: {_runningCount}, True Count: {TrueCount:F2}");

        if (runningCountText != null) runningCountText.text = $"RunningCount : {_runningCount}";
       else Debug.Log("runningCountText is null");

        if (trueCountText != null)
            trueCountText.text = $"TRUE COUNT: {TrueCount:F2}";
        else
            Debug.Log("trueCountText is null");
    }


    private int GetCardCountValue(CardSO card)
    {
        if (card.Value >= 2 && card.Value <= 6) return +1;
        if (card.Value >= 10 || card.IsAce) return -1;
        return 0; // 7, 8, 9 are neutral
    }

    public void DisplayCounts()
    {
        int running = GetRunningCount();
        float trueCount = GetTrueCount();

        Debug.Log($"[DisplayCounts] Called. Running: {running}, True: {trueCount:F2}");

        if (runningCountText != null)
        {
            runningCountText.text = $"Running: {running}";
            Debug.Log($"Updated runningCountText to: Running: {running}");
        }
        else
        {
            Debug.LogWarning("runningCountText is NULL");
        }

        if (trueCountText != null)
        {
            trueCountText.text = $"True: {trueCount:F2}";
            Debug.Log($"Updated trueCountText to: True: {trueCount:F2}");
        }
        else
        {
            Debug.LogWarning("trueCountText is NULL");
        }
    }
}
