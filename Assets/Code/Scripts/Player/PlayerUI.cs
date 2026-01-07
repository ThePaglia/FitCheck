using System.Collections.Generic;
using UnityEngine;

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
}
