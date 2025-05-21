using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DealerHand : MonoBehaviour
{
    [SerializeField] private DealerHandVisual _visuals;

    private List<CardSO> _cards;
    private Card _hiddenCard;
    private int _score;

    [SerializeField] private DealerHand _dealerHandInstance;

    public DealerHand DealerHandInstance => _dealerHandInstance;

    public int Score
    {
        get => _score;
    }

    public List<CardSO> GetAllCards()
    {
        return new List<CardSO>(_cards);
    }

    // Method to retrieve dealer's upcard (first card in the hand)
    public CardSO GetDealerUpcard()
    {
        // Check if the dealer hand has at least one card (the upcard)
        if (_dealerHandInstance.GetAllCards().Count > 0)
        {
            return _dealerHandInstance.GetAllCards()[0]; // Return the first card as the upcard
        }
        else
        {
            return null; // No upcard if the dealer has no cards
        }
    }

    public async Task AddCard(CardSO cardSO, bool hidden)
    {
        _cards ??= new();

        _cards.Add(cardSO);
        Card newCardObj = await _visuals.AddCard(cardSO, _cards.Count - 1, hidden);

        if (hidden)
        {
            _hiddenCard = newCardObj;
            // DO NOT count the card yet because it is hidden.
        }
        else
        {
            HandleScore();

            // Count the card now
            CardCounter.Instance.CardDealt(cardSO, "Dealer");
        }
    }

    public void Clear()
    {
        _score = 0;
        _hiddenCard = null;

        _cards?.Clear();
        _visuals.Clear();
    }

    public bool HasBlackjack()
    {
        return GetCardSO(0).Value + GetCardSO(1).Value == 21;
    }

    public CardSO GetCardSO(int index)
    {
        // Out of bounds
        if (index > _cards.Count - 1)
        {
            return null;
        }
        else
        {
            return _cards[index];
        }
    }

    public void ShowHiddenCard()
    {
        if (_hiddenCard == null) return;

        _hiddenCard.Visuals.Turn(CardVisual.ImagePos.Front);

        // Add this: update count properly
        CardCounter.Instance.CardDealt(_hiddenCard.CardSO);

        HandleScore();

}

    private void HandleScore()
    {
        int newScore = 0;

        // Aces with value 1
        List<Card> aces1 = new();
        List<Card> cardObjs = _visuals.CardObjs;

        foreach (var card in cardObjs)
        {
            CardSO cardSO = card.CardSO;
            newScore += cardSO.Value;

            if (newScore > 21)
            {
                List<Card> allAces = cardObjs.FindAll(card => card.CardSO.IsAce);

                foreach (var ace in allAces)
                {
                    // If ace's value wasn't reduced to 1.
                    if (!aces1.Contains(ace))
                    {
                        aces1.Add(ace);
                        newScore -= 10;
                        break;
                    }
                }
            }
        }

        _score = newScore;
        bool isSoft = cardObjs.FindAll(card => card.CardSO.IsAce).Count != aces1.Count;
        _visuals.UpdateScore(_score, isSoft);
    }
}
