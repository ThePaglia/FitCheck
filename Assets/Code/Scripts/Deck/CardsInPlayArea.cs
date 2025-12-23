using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardsInPlayArea : MonoBehaviour
{
    [SerializeField] private float columnSpacing = 0.23f;
    [SerializeField] private float rowSpacing = 0.3f;
    [SerializeField] private int initialCardCount = 4;
    [SerializeField] private int columns = 4;
    [SerializeField] private Card prefabScale;

    private List<Card> cardsInPlay = new List<Card>();

    private void Start()
    {
        PlaceInitialCards();
    }

    private void PlaceInitialCards()
    {
        // Draw 4 random cards and place them in front of the deck
        for (int i = 0; i < initialCardCount; i++)
        {
            Card card = Deck.Instance.Draw();

            if (card != null)
            {
                PlaceCard(card, i);
                cardsInPlay.Add(card);
            }
        }
    }

    private void PlaceCard(Card card, int i)
    {
        int col = i % columns;
        int row = i / columns;

        // Calculate total grid width and height
        int totalRows = Mathf.CeilToInt((float)initialCardCount / columns);
        float gridWidth = (columns - 1) * columnSpacing;
        float gridHeight = (totalRows - 1) * rowSpacing;

        // Center offset
        float offsetX = -gridWidth / 2f;
        float offsetZ = -gridHeight;

        Vector3 pos = transform.position + new Vector3(col * columnSpacing + offsetX, 0.1f, row * rowSpacing + offsetZ);

        card.transform.SetParent(transform);
        card.transform.position = pos;
        card.transform.rotation = Quaternion.Euler(90f, 180f, 0f);
        card.transform.localScale = prefabScale.transform.localScale;
        card.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame) return;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out var hit))
        {
            Card card = hit.collider.GetComponentInParent<Card>();
            if (card != null && cardsInPlay.Contains(card))
            {
                int slot = cardsInPlay.IndexOf(card);
                // Route to GameManager for rules
                if (GameManager.Instance != null && GameManager.Instance.OnCardInPlayAreaClicked(card))
                {
                    // Remove from grid and optionally refill the slot
                    cardsInPlay.RemoveAt(slot);

                    Card replacement = Deck.Instance.Draw();
                    if (replacement != null)
                    {
                        PlaceCard(replacement, slot);
                        cardsInPlay.Insert(slot, replacement);
                    }
                }
            }
        }
    }
}
