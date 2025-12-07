using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<ActionToken> actionTokens = new List<ActionToken>(2);
    public List<ResourceToken> resourceTokens = new List<ResourceToken>();
    public List<Card> hand = new List<Card>();
    public List<Card> costumesInPlay = new List<Card>();

    private PlayerUI playerUI;

    private void Awake()
    {
        playerUI = GetComponent<PlayerUI>();
    }

    public bool HasAvailableAction()
    {
        return actionTokens.Any(token => token.isAvailable);
    }

    public void UseActionToken()
    {
        var availableToken = actionTokens.FirstOrDefault(token => token.isAvailable);
        if (availableToken != null)
        {
            availableToken.Consume();
        }
        else
        {
            throw new InvalidOperationException("No available action tokens to use.");
        }
    }

    public void RefreshActionTokens()
    {
        foreach (var token in actionTokens)
        {
            token.Refresh();
        }
    }

    public void Draw()
    {
        Card card = Deck.Instance.DrawCard();
        if (card != null)
        {
            hand.Add(card);
            playerUI.AnimateCardToHand(card);
            playerUI.UpdateHandUI(hand);
        }
    }
}
