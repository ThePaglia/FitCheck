using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ScriptableCard;

public class CraftedCardArea : MonoBehaviour
{
    [SerializeField] private float cardSpacing = 0.3f;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Card prefabScale;
    private readonly int columns = System.Enum.GetValues(typeof(CardType)).Length;

    private readonly Dictionary<CardType, Card> craftedCards = new Dictionary<CardType, Card>();

    private void Awake()
    {
        // Initialize the dictionary with all card types
        foreach (CardType type in System.Enum.GetValues(typeof(CardType)))
        {
            craftedCards[type] = null;
        }

        // Set default start position if not configured
        if (startPosition == Vector3.zero)
        {
            startPosition = transform.position;
        }
    }

    public void PlaceCraftedCard(Card card)
    {
        if (card == null || card.CardData == null) return;

        CardType cardType = card.CardData.cardType;

        // Re-enable 3D rendering
        MeshRenderer meshRenderer = card.GetComponent<MeshRenderer>();
        if (meshRenderer != null) meshRenderer.enabled = true;

        Collider collider = card.GetComponent<Collider>();
        if (collider != null) collider.enabled = true;

        // Disable UI Image component
        Image cardImage = card.GetComponent<Image>();
        if (cardImage != null) cardImage.enabled = false;

        // Place old card of the same type back in the Deck
        if (craftedCards[cardType] != null)
        {
            Deck.Instance.ReturnCardToDeck(craftedCards[cardType]);
        }

        // Store new card
        craftedCards[cardType] = card;

        // Calculate centered grid position
        int slotIndex = (int)cardType;
        float gridWidth = (columns - 1) * cardSpacing;
        float offsetX = -gridWidth / 2f;

        Vector3 position = startPosition + new Vector3(slotIndex * cardSpacing + offsetX, 0.1f, 0f);

        // Position and orient the card
        card.transform.SetParent(transform);
        card.transform.SetPositionAndRotation(position, Quaternion.Euler(90f, 180f, 0f));
        card.transform.localScale = prefabScale.transform.localScale;
        card.gameObject.SetActive(true);
    }

    public int GetCraftedCardCount()
    {
        int count = 0;
        foreach (Card card in craftedCards.Values)
        {
            if (card != null) count++;
        }
        return count;
    }

    public int GetTotalCraftedCardPoints()
    {
        int totalPoints = 0;
        foreach (Card card in craftedCards.Values)
        {
            if (card != null && card.CardData != null)
            {
                totalPoints += card.CardData.points;
            }
        }
        return totalPoints;
    }
}