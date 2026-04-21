using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ResourceToken;

public class Player : MonoBehaviour
{
    public Transform cameraPosition;
    public List<ActionToken> actionTokens = new List<ActionToken>(2);
    public List<ResourceToken> resourceTokens = new List<ResourceToken>();
    public List<Card> hand = new List<Card>();
    public int HandLimit { get; private set; } = 3;
    public int ResourceTokenLimit { get; private set; } = 6;
    public CraftedCardArea CraftedCardArea { get; set; }
    public PlayerUI PlayerUI { get; private set; }

    private void Awake()
    {
        PlayerUI = PlayerUI.Instance;
        CraftedCardArea = GetComponentInChildren<CraftedCardArea>();
    }

    public bool HasAvailableAction()
    {
        return actionTokens.Any(token => token.IsAvailable);
    }

    public void UseActionToken()
    {
        var availableToken = actionTokens.FirstOrDefault(token => token.IsAvailable);
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

    public void DrawCard(Card card)
    {
        if (card != null)
        {
            hand.Add(card);
            PlayerUI.AnimateCardToHand(card);
            PlayerUI.UpdateHandUI(hand);
        }
    }

    public void DrawResourceToken(ResourceToken resourceToken)
    {
        if (resourceToken != null)
        {
            resourceTokens.Add(resourceToken);
            PlayerUI.UpdateResourceTokenUI(resourceTokens);
        }
    }

    // Check if we can cover the cost with available tokens and wildcards
    private bool CoverCost(int cost, int available, ref int wild)
    {
        int missing = Mathf.Max(0, cost - available);
        if (missing > wild) return false;
        wild -= missing;
        return true;
    }

    private bool CanPayCost(Card card)
    {
        List<(int cost, int available)> tokenCosts = new List<(int cost, int available)>
        {
            (card.CardData.costDuctTape, resourceTokens.Count(t => t.resourceTokenType == ResourceTokenType.DuctTape)),
            (card.CardData.costGlue, resourceTokens.Count(t => t.resourceTokenType == ResourceTokenType.Glue)),
            (card.CardData.costWire, resourceTokens.Count(t => t.resourceTokenType == ResourceTokenType.Wire)),
            (card.CardData.costFabric, resourceTokens.Count(t => t.resourceTokenType == ResourceTokenType.Fabric)),
            (card.CardData.costPlastic, resourceTokens.Count(t => t.resourceTokenType == ResourceTokenType.Plastic))
        };

        int glitter = resourceTokens.Count(t => t.resourceTokenType == ResourceTokenType.Glitter);

        return tokenCosts.All(token => CoverCost(token.cost, token.available, ref glitter));
    }

    private int RemoveTokensByType(ResourceTokenType type, int countCost, ref int glitterLeft, ref List<ResourceToken> tokensToReturn)
    {
        int removed = 0;
        for (int i = resourceTokens.Count - 1; i >= 0 && removed < countCost; i--)
        {
            if (resourceTokens[i].resourceTokenType == type)
            {
                if (resourceTokens[i].resourceTokenType == ResourceTokenType.Glitter)
                {
                    glitterLeft--;
                }
                tokensToReturn.Add(resourceTokens[i]);
                resourceTokens.RemoveAt(i);
                removed++;
            }
        }
        return removed;
    }

    // Remove specific tokens first, then glitter for any missing
    private bool RemoveTokens(ResourceTokenType type, int countCost, ref int glitterLeft, ref List<ResourceToken> tokensToReturn)
    {
        int removed = RemoveTokensByType(type, countCost, ref glitterLeft, ref tokensToReturn);
        int missing = countCost - removed;

        if (missing > 0)
        {
            int glitterUsed = RemoveTokensByType(ResourceTokenType.Glitter, missing, ref glitterLeft, ref tokensToReturn);
            removed += glitterUsed;
        }
        return removed >= countCost;
    }

    private bool SpendCost(Card card)
    {
        List<(ResourceTokenType type, int cost)> tokenCosts = new List<(ResourceTokenType type, int cost)>
            {
                (ResourceTokenType.DuctTape, card.CardData.costDuctTape),
                (ResourceTokenType.Glue, card.CardData.costGlue),
                (ResourceTokenType.Wire, card.CardData.costWire),
                (ResourceTokenType.Fabric, card.CardData.costFabric),
                (ResourceTokenType.Plastic, card.CardData.costPlastic)
            };
        List<ResourceToken> tokensToReturn = new List<ResourceToken>();
        int glitterPool = resourceTokens.Count(t => t.resourceTokenType == ResourceTokenType.Glitter);

        foreach (var (type, cost) in tokenCosts)
        {
            if (!RemoveTokens(type, cost, ref glitterPool, ref tokensToReturn))
            {
                return false;
            }
        }

        foreach (var resourceToken in tokensToReturn)
        {
            ResourceTokenBag.Instance.ReturnResourceTokenToBag(resourceToken);
        }

        return true;
    }

    public bool TryCraftCard(Card card)
    {
        if (card == null) return false;
        if (!hand.Contains(card)) return false;
        if (!CanPayCost(card)) return false;
        if (!SpendCost(card)) return false;

        hand.Remove(card);
        CraftedCardArea.PlaceCraftedCard(card);
        PlayerUI.UpdateResourceTokenUI(resourceTokens);
        PlayerUI.UpdateHandUI(hand);

        return true;
    }
}
