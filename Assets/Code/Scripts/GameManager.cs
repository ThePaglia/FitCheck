using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Deck costumeDeck;
    public List<ResourceToken> tokenBag;
    public List<Player> players;
    public TurnManager turnManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool OnDeckClicked()
    {
        if (turnManager == null)
        {
            Debug.LogWarning("TurnManager not set!");
            return false;
        }

        Player currentPlayer = turnManager.GetCurrentPlayer();

        if (currentPlayer == null)
        {
            Debug.LogWarning("No current player!");
            return false;
        }

        if (!turnManager.CanCurrentPlayerAct())
        {
            Debug.Log("No available action tokens for current player.");
            return false;
        }
        currentPlayer.UseActionToken(); // Drawing costs an action
        currentPlayer.Draw();
        return true;
    }
}