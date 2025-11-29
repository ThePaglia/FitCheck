using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Deck : MonoBehaviour
{
    public static Deck Instance { get; private set; } //Singleton
    [SerializeField] private ScriptableDeck deck;

    [Header("Visual Settings")]
    [SerializeField] private float heightDecreasePerCard = 0.005f;
    [SerializeField] private float minHeightBeforeInvisible = 0.005f;
    [SerializeField] private MeshRenderer meshRenderer;
    // TODO: Finish the deck class
    private List<Card> drawPile = new List<Card>();
    private List<Card> tablePile = new List<Card>();

    private void Awake()
    {
        if (meshRenderer == null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }
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
                Debug.Log($"Hit: {hit.collider.gameObject.name}");
                if (hit.collider.gameObject == gameObject)
                {
                    Debug.Log("Deck clicked!");
                    ShrinkDeck();
                }
            }
        }
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
