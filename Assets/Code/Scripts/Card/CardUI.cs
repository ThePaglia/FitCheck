using UnityEngine;

public class CardUI : MonoBehaviour
{
    // TODO: Add a 3d model card and set every component
    private Card card;
    private void Awake()
    {
        card = GetComponent<Card>();
        SetCardUI();
    }

    //calls Awake every time the inspector/editor gets refreshed
    //- lets you see changes also in editor no need to start game
    private void OnValidate()
    {
        Awake();
    }

    public void SetCardUI()
    {
        if (card != null && card.CardData != null)
        {
            // SetCardTexts();
            // SetCardImage();
        }
    }
}
