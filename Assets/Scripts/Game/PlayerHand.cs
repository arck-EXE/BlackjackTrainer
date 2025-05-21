using System.Collections.Generic;
using UnityEngine;
using BlackjackNamespace;
using System.Threading.Tasks;
using TMPro;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private PlayerHandVisual _visuals;
    [SerializeField] private ChipField _chipField;

    //[SerializeField] private TMP_Text _strategyText;

    private void Start()
    {
        //_strategyText = GameObject.FindGameObjectWithTag("StrategyText").GetComponent<TMP_Text>();
    }

    private List<CardSO> _cards;
    public HandState _state;
    private int _score;

    public bool HasAce() => _cards.Exists(card => card.IsAce);

    public List<CardSO> GetAllCards()
    {
        return new List<CardSO>(_cards);
    }

    public List<CardSO> GetCards()
    {
        return _cards;
    }

    public HandState State {
        get => _state;
    }

    public int Score {
        get => _score;
    }

    public ChipField ChipField {
        get => _chipField;
    }

    public async Task AddCard(CardSO cardSO, GameAction actionType)
    {
        _cards ??= new();

        _cards.Add(cardSO);

        // Count the card for the counter
        CardCounter.Instance.CardDealt(cardSO, "Player");

        await _visuals.AddCard(cardSO, _cards.Count - 1, actionType);

        HandleScore();

        if (actionType == GameAction.Hit)
        {
            CardSO dealerUpcard = HandsManager.Instance.DealerHand.GetCardSO(0);
            if (dealerUpcard != null)
            {
                Debug.Log($"Dealer upcard: {dealerUpcard.name}, Value: {dealerUpcard.Value}, IsAce: {dealerUpcard.IsAce}");
            }

            if (dealerUpcard != null && _cards.Count > 0)
            {
                int dealerValue = dealerUpcard.Value == 1 ? 11 : dealerUpcard.Value;
                StrategyAction recommendedAction = BlackjackStrategy.GetStrategy(_cards, dealerValue);
                //_strategyText.text = $"[Strategy After Hit] Recommended action for player hand (score: {Score}) vs dealer upcard ({dealerUpcard.Value}): {recommendedAction}";
            }
        }
    }


    public void RemoveCard(int index) {
        // Out of bounds
        if (index > _cards.Count - 1) return;

        CardSO cardSO = GetCardSO(index);

        if (cardSO != null) {
            _cards.Remove(cardSO);
            _visuals.RemoveCard(index);

            HandleScore();
        }
    }

    public CardSO GetCardSO(int index) {
        // Out of bounds
        if (index > _cards.Count-1) {
            return null;
        } else {
            return _cards[index];
        }
    }

    public void ChangeState(HandState newState) {
        _state = newState;
        _visuals.UpdateVisuals(newState);
    }

    public bool HasBlackjack() {
        return GetCardSO(0).Value + GetCardSO(1).Value == 21;
    }

    private void HandleScore() {
        int newScore = 0;

        // Aces with value 1
        List<Card> aces1 = new();
        List<Card> cardObjs = _visuals.CardObjs;

        foreach (var card in cardObjs) {
            CardSO cardSO = card.CardSO;
            newScore += cardSO.Value;

            if (newScore > 21) {
                List<Card> allAces = cardObjs.FindAll(card => card.CardSO.IsAce);

                foreach (var ace in allAces) {
                    // If ace's value wasnt reduced to 1.
                    if (!aces1.Contains(ace)) {
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

    public List<GameAction> GetAvaliableGameActions() {
        // Player can always hit and stand.
        List<GameAction> avaliableActions = new() {
            GameAction.Hit,
            GameAction.Stand,
        };

        if (_cards.Count == 2) {
            avaliableActions.Add(GameAction.DoubleDown);

            if (GetCardSO(0).Value == GetCardSO(1).Value) {
                avaliableActions.Add(GameAction.Split);
            }
        }

        return avaliableActions;
    }
}
