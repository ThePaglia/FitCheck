using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Deck : MonoBehaviour
{
    public static Deck Instance { get; private set; } //Singleton
    [SerializeField] private ScriptableDeck deck;
    [SerializeField] private Card cardPrefab;

    [Header("Visual Settings")]
    [SerializeField] private float heightDecreasePerCard = 0.005f;
    [SerializeField] private float minHeightBeforeInvisible = 0.005f;
    [SerializeField] private MeshRenderer meshRenderer;

    public List<Card> drawPile = new List<Card>();
    public List<Card> tablePile = new List<Card>();

    private void Awake()
    {
        if (meshRenderer == null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }
        // Typical singleton declaration
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SpawnFromScriptableDeck();
    }

    private void SpawnFromScriptableDeck()
    {
        if (deck == null || deck.CardsInDeck == null || cardPrefab == null)
        {
            Debug.LogWarning("Deck or Card Prefab is not assigned.");
            return;
        }
        drawPile.Clear();
        foreach (var cardData in deck.CardsInDeck)
        {
            var card = SpawnCard(cardData);
            drawPile.Add(card);
            card.gameObject.SetActive(false); // Initially inactive
        }
        drawPile = Shuffle(drawPile);
    }

    private Card SpawnCard(ScriptableCard data)
    {
        var go = Instantiate(cardPrefab, meshRenderer.transform.position + Vector3.up * 0.1f, Quaternion.identity, transform);
        Card card = go.GetComponent<Card>();
        card.SetUp(data);
        return card;
    }

    private void Update()
    {
        // Use Raycasting to detect mouse clicks on the deck
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    Debug.Log("Deck clicked!");
                    OnDeckClicked();
                }
            }
        }
    }

    private void OnDeckClicked()
    {
        // Notify GameManager to draw card for current player
        bool drew = GameManager.Instance != null && GameManager.Instance.OnDeckClicked();

        if (drew)
        {
            // Then shrink the deck visually
            ShrinkDeck();
        }
    }

    public Card Draw()
    {
        if (drawPile.Count == 0)
        {
            Debug.Log("No more cards to draw.");
            return null;
        }

        Card drawnCard = drawPile[0];
        drawPile.RemoveAt(0);
        drawnCard.gameObject.SetActive(true);

        return drawnCard;
    }

    private void ShrinkDeck()
    {
        Vector3 currentScale = transform.localScale;
        Vector3 currentPosition = transform.position;

        currentScale.y -= heightDecreasePerCard;
        if (currentScale.y < minHeightBeforeInvisible)
        {
            meshRenderer.enabled = false;
        }
        else
        {
            // Adjust position to keep the deck grounded
            currentPosition.y -= heightDecreasePerCard / 2;

            transform.localScale = currentScale;
            transform.position = currentPosition;
        }
    }

    public List<T> Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }
}
