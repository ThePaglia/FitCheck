using System.Collections.Generic;
using UnityEngine;

// TODO: Consider refactoring the UI into a World Space UI
public class PlayerUI : MonoBehaviour
{
    public HandUI handUI;
    public ResourceTokenUI resourceTokenUI;

    public void UpdateHandUI(List<Card> hand)
    {
        handUI.UpdateHandUI(hand);
    }
    public void AnimateCardToHand(Card card)
    {
        handUI.AnimateCardToHand(card);
    }

    public void UpdateResourceTokenUI(List<ResourceToken> resourceTokens)
    {
        resourceTokenUI.UpdateResourceTokenUI(resourceTokens);
    }

    public void AnimateCardToCraftedArea(Card card)
    {
        handUI.AnimateCardToCraftedArea(card);
    }
}
