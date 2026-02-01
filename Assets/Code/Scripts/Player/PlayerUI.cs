using System.Collections.Generic;
using UnityEngine;

// TODO: Consider refactoring the UI into a World Space UI, when refactored use again one canvas for each player and remove instance
public class PlayerUI : MonoBehaviour
{
    public static PlayerUI Instance { get; private set; }
    public HandUI handUI;
    public ResourceTokenUI resourceTokenUI;

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

    public void UpdateHandUI(List<Card> hand)
    {
        handUI.UpdateHandUI(hand);
    }

    public void ClearHand()
    {
        handUI.ClearHand();
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
