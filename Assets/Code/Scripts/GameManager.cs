using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
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

    private bool CheckTurnManagerAndCurrentPlayer()
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
        return true;
    }

    public bool OnDeckClicked()
    {
        if (!CheckTurnManagerAndCurrentPlayer())
        {
            return false;
        }

        Player currentPlayer = turnManager.GetCurrentPlayer();
        if (currentPlayer.hand.Count >= currentPlayer.handLimit)
        {
            Debug.Log("Hand limit reached. Cannot draw more cards.");
            return false;
        }

        currentPlayer.UseActionToken(); // Drawing costs an action
        Card drawnCard = Deck.Instance.Draw();
        currentPlayer.DrawCard(drawnCard);

        return true;
    }

    public bool OnTokenBagClicked()
    {
        if (!CheckTurnManagerAndCurrentPlayer())
        {
            return false;
        }

        Player currentPlayer = turnManager.GetCurrentPlayer();
        if (currentPlayer.resourceTokens.Count >= currentPlayer.resourceTokenLimit)
        {
            Debug.Log("Resource token limit reached. Cannot draw more tokens.");
            return false;
        }

        currentPlayer.UseActionToken(); // Drawing costs an action
        currentPlayer.DrawResourceToken();

        return true;
    }

    public bool OnCardInPlayAreaClicked(Card card)
    {
        if (!CheckTurnManagerAndCurrentPlayer())
        {
            return false;
        }

        Player currentPlayer = turnManager.GetCurrentPlayer();
        if (currentPlayer.hand.Count >= currentPlayer.handLimit)
        {
            Debug.Log("Hand limit reached. Cannot draw more cards.");
            return false;
        }
        currentPlayer.UseActionToken(); // Drawing costs an action
        currentPlayer.DrawCard(card);

        return true;
    }
}