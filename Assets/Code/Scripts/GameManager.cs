using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class GameManager : MonoBehaviour
{
    public List<Card> costumeDeck;
    public List<Token> tokenBag;
    public List<Player> players;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        costumeDeck = Shuffle(costumeDeck);
        tokenBag = Shuffle(tokenBag);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // TODO: test
    List<T> Shuffle<T>(List<T> list)
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
