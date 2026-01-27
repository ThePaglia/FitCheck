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
    public List<Card> costumesInPlay = new List<Card>();
    public int handLimit = 3;
    public int resourceTokenLimit = 6;

    private PlayerUI playerUI;
    private CraftedCardArea craftedCardArea;

    private void Awake()
    {
        playerUI = GetComponent<PlayerUI>();
        craftedCardArea = GetComponentInChildren<CraftedCardArea>();
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

    public void DrawCard(Card card)
    {
        if (card != null)
        {
            hand.Add(card);
            playerUI.AnimateCardToHand(card);
            playerUI.UpdateHandUI(hand);
        }
    }

    public void DrawResourceToken(ResourceToken resourceToken)
    {
        if (resourceToken != null)
        {
            resourceTokens.Add(resourceToken);
            playerUI.UpdateResourceTokenUI(resourceTokens);
        }
    }

    private bool CanPayCost(Card card)
    {
        // Map costs to token types (adjust mapping if your costs differ)
        int needDuct = card.CardData.costDuctTape;
        int needGlue = card.CardData.costGlue;
        int needWire = card.CardData.costWire;
        int needFabric = card.CardData.costFabric;
        int needPlastic = card.CardData.costPlastic;

        int haveDuct = resourceTokens.Count(t => t.resourceTokenType == ResourceTokenType.DuctTape);
        int haveGlue = resourceTokens.Count(t => t.resourceTokenType == ResourceTokenType.Glue);
        int haveWire = resourceTokens.Count(t => t.resourceTokenType == ResourceTokenType.Wire);
        int haveFabric = resourceTokens.Count(t => t.resourceTokenType == ResourceTokenType.Fabric);
        int havePlastic = resourceTokens.Count(t => t.resourceTokenType == ResourceTokenType.Plastic);
        int glitter = resourceTokens.Count(t => t.resourceTokenType == ResourceTokenType.Glitter);

        // Helper function to check if we can cover the cost with available tokens and wildcards
        static bool CoverCost(int need, int have, ref int wild)
        {
            int missing = Mathf.Max(0, need - have);
            if (missing > wild) return false;
            wild -= missing;
            return true;
        }

        return CoverCost(needDuct, haveDuct, ref glitter)
            && CoverCost(needGlue, haveGlue, ref glitter)
            && CoverCost(needWire, haveWire, ref glitter)
            && CoverCost(needFabric, haveFabric, ref glitter)
            && CoverCost(needPlastic, havePlastic, ref glitter);
    }

    // TODO: Do more testing
    private bool SpendCost(Card card)
    {
        int needDuct = card.CardData.costDuctTape;
        int needGlue = card.CardData.costGlue;
        int needWire = card.CardData.costWire;
        int needFabric = card.CardData.costFabric;
        int needPlastic = card.CardData.costPlastic;

        List<ResourceToken> tokensToReturn = new List<ResourceToken>();

        // Remove specific tokens first, then glitter for any missing
        bool RemoveTokens(ResourceTokenType type, int countNeeded, ref int glitterLeft)
        {
            int removed = 0;
            for (int i = resourceTokens.Count - 1; i >= 0 && removed < countNeeded; i--)
            {
                if (resourceTokens[i].resourceTokenType == type)
                {
                    tokensToReturn.Add(resourceTokens[i]);
                    resourceTokens.RemoveAt(i);
                    removed++;
                }
            }
            int missing = countNeeded - removed;
            if (missing > 0)
            {
                int glitterUsed = 0;
                for (int i = resourceTokens.Count - 1; i >= 0 && glitterUsed < missing; i--)
                {
                    if (resourceTokens[i].resourceTokenType == ResourceTokenType.Glitter)
                    {
                        tokensToReturn.Add(resourceTokens[i]);
                        resourceTokens.RemoveAt(i);
                        glitterUsed++;
                        glitterLeft--;
                    }
                }
                removed += glitterUsed;
            }
            return removed >= countNeeded;
        }

        int glitterPool = resourceTokens.Count(t => t.resourceTokenType == ResourceTokenType.Glitter);

        bool ok = RemoveTokens(ResourceTokenType.DuctTape, needDuct, ref glitterPool)
               && RemoveTokens(ResourceTokenType.Glue, needGlue, ref glitterPool)
               && RemoveTokens(ResourceTokenType.Wire, needWire, ref glitterPool)
               && RemoveTokens(ResourceTokenType.Fabric, needFabric, ref glitterPool)
               && RemoveTokens(ResourceTokenType.Plastic, needPlastic, ref glitterPool);

        if (ok)
        {
            // Return all spent resource token to the bag
            foreach (var resourceToken in tokensToReturn)
            {
                ResourceTokenBag.Instance.ReturnResourceTokenToBag(resourceToken);
            }
        }
        return ok;
    }

    public bool TryCraftCard(Card card)
    {
        if (card == null) return false;
        if (!hand.Contains(card)) return false;
        if (!CanPayCost(card)) return false;
        if (!SpendCost(card)) return false;

        hand.Remove(card);
        craftedCardArea.PlaceCraftedCard(card);
        costumesInPlay.Add(card);
        playerUI.UpdateResourceTokenUI(resourceTokens);
        playerUI.UpdateHandUI(hand);

        return true;
    }
}
