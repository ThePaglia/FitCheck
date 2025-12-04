using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public HandUI handUI;

    public void UpdateHandUI(List<Card> hand)
    {
        handUI.UpdateHandUI(hand);
    }
}
