using System.Collections.Generic;

public static class StrategyFeedback
{
    private static string GetCardIconWithLabel(int cardValue, CardSuit suit)
    {
        string label;
        string spriteName = suit.ToString(); // Assumes enum names match sprite names exactly

        switch (cardValue)
        {
            case 1:
                label = "Ace";
                break;
            case 11:
                label = "Jack";
                break;
            case 12:
                label = "Queen";
                break;
            case 13:
                label = "King";
                break;
            default:
                label = cardValue.ToString();
                break;
        }

        return $"<b>{label}</b> <sprite name=\"{spriteName}\">";
    }

    public static string GetFeedback(List<CardSO> playerCards, CardSO dealerUpcard, PlayerAction playerAction)
    {
        StrategyAction correctAction = BlackjackStrategy.GetStrategy(playerCards, dealerUpcard.Value);
        int handValue = BlackjackStrategy.GetHandValue(playerCards, out _);
        string dealerCardIcon = GetCardIconWithLabel(dealerUpcard.Value, dealerUpcard.Suit);

        if (Matches(playerAction, correctAction))
        {
            return $"<color=green>GREAT JOB!</color>\nYou chose to <b>{playerAction.ToString().ToLower()}</b> when your score was <b>{handValue}</b> and the dealer's up card was {dealerUpcard.Value}.";
        }

        return $"<color=red>INCORRECT.</color>\nYou chose to <b>{playerAction.ToString().ToLower()}</b> while your score was <b>{handValue}</b> and the dealer's up card was {dealerUpcard.Value}, but the correct move was to <b>{correctAction.ToString().ToLower()}</b>.";
    }



    private static bool Matches(PlayerAction playerAction, StrategyAction correctAction)
    {
        return correctAction switch
        {
            StrategyAction.Hit => playerAction == PlayerAction.Hit,
            StrategyAction.Stand => playerAction == PlayerAction.Stand,
            StrategyAction.Double => playerAction == PlayerAction.Double,
            StrategyAction.Split => playerAction == PlayerAction.Split,
            StrategyAction.DoubleStand => playerAction == PlayerAction.Double || playerAction == PlayerAction.Stand,
            _ => false,
        };
    }
}