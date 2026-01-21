using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class HandUI : MonoBehaviour
{
    private RectTransform handContainer;
    private readonly float cardMoveTime = 0.5f;
    private readonly float fanAngle = 15f;
    private readonly float fanRadius = 500f;
    private readonly List<RaycastResult> raycastResults = new List<RaycastResult>();

    private void Awake()
    {
        handContainer = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame) return;
        if (EventSystem.current == null) return;

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Mouse.current.position.ReadValue()
        };

        raycastResults.Clear();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        // Find first card in hand
        for (int i = 0; i < raycastResults.Count; i++)
        {
            Card card = raycastResults[i].gameObject.GetComponent<Card>();
            if (card != null && IsCardInHand(card))
            {
                GameManager.Instance.OnCardInHandClicked(card);
                return;
            }
        }
    }

    private bool IsCardInHand(Card card)
    {
        // Check if card's transform is a child of handContainer
        return card.transform.IsChildOf(handContainer.transform);
    }

    public void UpdateHandUI(List<Card> hand)
    {
        FanCards(hand);
    }

    public void AnimateCardToHand(Card card)
    {
        RectTransform cardRect = card.GetComponent<RectTransform>();
        if (cardRect == null) return;

        // Store the world position before reparenting
        Vector3 worldStartPos = card.transform.position;

        // Disable 3D rendering
        MeshRenderer meshRenderer = card.GetComponent<MeshRenderer>();
        if (meshRenderer != null) meshRenderer.enabled = false;

        Collider collider = card.GetComponent<Collider>();
        if (collider != null) collider.enabled = false;

        // Enable Image component for UI display
        Image cardImage = card.GetComponent<Image>();
        if (cardImage == null)
        {
            cardImage = card.gameObject.AddComponent<Image>();
        }
        cardImage.enabled = true;

        // Set image from CardData
        if (card.CardData != null && card.CardData.cardImage != null)
        {
            cardImage.sprite = Sprite.Create(
                card.CardData.cardImage,
                new Rect(0, 0, card.CardData.cardImage.width, card.CardData.cardImage.height),
                Vector2.zero
            );
        }

        // Reparent to hand container (Canvas)
        cardRect.SetParent(handContainer);
        cardRect.localScale = Vector3.one;

        // Convert world position to local position
        Vector3 localStartPos = handContainer.TransformPoint(worldStartPos);
        cardRect.position = localStartPos;

        Vector3 targetLocalPos = handContainer.position;

        // Start animation
        MoveCardToHand(cardRect, localStartPos, targetLocalPos); // Doesn't work properly if StartCoroutine is added
    }

    private IEnumerator MoveCardToHand(RectTransform cardRect, Vector3 startWorldPos, Vector3 targetWorldPos)
    {
        float elapsedTime = 0f;

        while (elapsedTime < cardMoveTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / cardMoveTime;

            Vector3 currentWorldPos = Vector3.Lerp(startWorldPos, targetWorldPos, t);
            cardRect.position = currentWorldPos;

            yield return null;
        }

        cardRect.position = targetWorldPos;
    }

    // TODO: Implement animation from hand to crafting area
    public void AnimateCardToCraftedArea(Card card)
    {

    }

    private void FanCards(List<Card> hand)
    {
        if (hand.Count == 0) return;

        int cardCount = hand.Count;
        float totalAngle = fanAngle * (cardCount - 1);
        float startAngle = -totalAngle / 2f;

        for (int i = 0; i < cardCount; i++)
        {
            float angle = startAngle + (fanAngle * i);
            float radian = angle * Mathf.Deg2Rad;

            float x = Mathf.Sin(radian) * fanRadius;
            float y = Mathf.Cos(radian) * fanRadius - fanRadius;

            RectTransform cardRect = hand[i].GetComponent<RectTransform>();

            if (cardRect != null)
            {
                cardRect.sizeDelta = new Vector2(200, 280);
                StartCoroutine(MoveToFanPosition(cardRect, new Vector2(x, y), 0.3f));
                cardRect.localRotation = Quaternion.Euler(0, 0, -angle);
            }
        }
    }

    private IEnumerator MoveToFanPosition(RectTransform cardRect, Vector2 targetPos, float duration)
    {
        Vector2 startPos = cardRect.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            cardRect.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            yield return null;
        }

        cardRect.anchoredPosition = targetPos;
    }
}