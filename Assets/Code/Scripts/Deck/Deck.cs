using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public static Deck Instance { get; private set; } //Singleton
    [SerializeField] private ScriptableDeck deck;
    [SerializeField] private Card _cardPrefab; //our cardPrefab, of which we will make copies with the different CardData
    private List<Card> deckPile;


    public List<T> Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }
}
