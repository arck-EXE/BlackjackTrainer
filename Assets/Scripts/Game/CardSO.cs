using UnityEngine;

public enum CardSuit
{
    Spade,
    Heart,
    Diamond,
    Club
}

[CreateAssetMenu()]
public class CardSO : ScriptableObject
{
    public GameObject Prefab;
    public string Name;
    public int Value;
    public bool IsAce;

    public CardSuit Suit;  // Add this
}
