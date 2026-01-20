using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandUI : MonoBehaviour
{
    private RectTransform handContainer;
    private float cardMoveTime = 0.5f;
    private float fanAngle = 15f;
    private float fanRadius = 500f;

    private void Awake()
    {
        handContainer = GetComponent<RectTransform>();
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
        MoveCardToHand(cardRect, localStartPos, targetLocalPos);
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