using System.Collections.Generic;

public static class BlackjackStrategy
{
    public static StrategyAction GetStrategy(List<CardSO> playerCards, int dealerUpcard)
    {
        int handValue = GetHandValue(playerCards, out bool isSoft);
        bool isPair = playerCards.Count == 2 && playerCards[0].Value == playerCards[1].Value;

        if (playerCards.Count == 2)
        {
            if (isPair)
            {
                return GetPairStrategy(playerCards[0].Value, dealerUpcard);
            }

            if (isSoft)
            {
                return GetSoftTotalStrategy(handValue, dealerUpcard);
            }
        }

        return GetHardTotalStrategy(handValue, dealerUpcard);
    }

    private static StrategyAction GetPairStrategy(int pairValue, int dealerUpcard)
    {
        // A = 11
        switch (pairValue)
        {
            case 11: return StrategyAction.Split; // AA
            case 10: return StrategyAction.Stand;
            case 9:
                if (dealerUpcard == 7 || dealerUpcard >= 10) return StrategyAction.Stand;
                return StrategyAction.Split;
            case 8: return StrategyAction.Split;
            case 7: return (dealerUpcard <= 7) ? StrategyAction.Split : StrategyAction.Hit;
            case 6: return (dealerUpcard <= 6) ? StrategyAction.Split : StrategyAction.Hit;
            case 5: return GetHardTotalStrategy(10, dealerUpcard); // Treat as 10
            case 4: return (dealerUpcard == 5 || dealerUpcard == 6) ? StrategyAction.Split : StrategyAction.Hit;
            case 3:
            case 2: return (dealerUpcard <= 7) ? StrategyAction.Split : StrategyAction.Hit;
            default: return StrategyAction.None;
        }
    }

    private static StrategyAction GetSoftTotalStrategy(int handValue, int dealerUpcard)
    {
        switch (handValue)
        {
            case 20:
            case 19: return StrategyAction.Stand;
            case 18:
                if (dealerUpcard >= 9 || dealerUpcard == 2) return StrategyAction.Hit;
                if (dealerUpcard == 3 || dealerUpcard == 4 || dealerUpcard == 5 || dealerUpcard == 6)
                    return StrategyAction.DoubleStand;
                return StrategyAction.Stand;
            case 17:
            case 16:
            case 15:
            case 14:
                if (dealerUpcard >= 4 && dealerUpcard <= 6) return StrategyAction.Double;
                return StrategyAction.Hit;
            case 13:
            case 12:
                if (dealerUpcard >= 5 && dealerUpcard <= 6) return StrategyAction.Double;
                return StrategyAction.Hit;
            default: return StrategyAction.None;
        }
    }

    private static StrategyAction GetHardTotalStrategy(int handValue, int dealerUpcard)
    {
        switch (handValue)
        {
            case >= 17: return StrategyAction.Stand;
            case 16:
            case 15:
            case 14:
            case 13:
                return (dealerUpcard >= 2 && dealerUpcard <= 6) ? StrategyAction.Stand : StrategyAction.Hit;
            case 12:
                return (dealerUpcard >= 4 && dealerUpcard <= 6) ? StrategyAction.Stand : StrategyAction.Hit;
            case 11: return StrategyAction.Double;
            case 10: return (dealerUpcard <= 9) ? StrategyAction.Double : StrategyAction.Hit;
            case 9: return (dealerUpcard >= 3 && dealerUpcard <= 6) ? StrategyAction.Double : StrategyAction.Hit;
            case <= 8: return StrategyAction.Hit;
            default:         }
    }

    public static int GetHandValue(List<CardSO> cards, out bool isSoft)
    {
        int total = 0;
        int aces = 0;
        isSoft = false;

        foreach (var card in cards)
        {
            total += card.Value;
            if (card.IsAce) aces++;
        }

        while (total > 21 && aces > 0)
        {
            total -= 10;
            aces--;
        }

        if (aces > 0) isSoft = true;

        return total;
    }
}
