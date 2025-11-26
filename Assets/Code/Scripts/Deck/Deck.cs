using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public static Deck Instance { get; private set; } //Singleton
    [SerializeField] private ScriptableDeck deck;
    [SerializeField] private Card cardPrefab; //our cardPrefab, of which we will make copies with the different CardData
    // TODO: Finish the deck class
    private List<Card> drawPile = new List<Card>();
    private List<Card> tablePile = new List<Card>();


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
