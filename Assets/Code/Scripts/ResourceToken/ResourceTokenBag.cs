using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static ResourceToken;

public class ResourceTokenBag : MonoBehaviour
{
    public static ResourceTokenBag Instance { get; private set; } //Singleton
    [SerializeField] private ResourceToken resourceTokenPrefab;
    [SerializeField] private int initialNormalResourceTokenCount = 7;
    [SerializeField] private int initialGlitterResourceTokenCount = 5;

    private List<ResourceToken> drawPile = new List<ResourceToken>();

    private void Awake()
    {
        // Typical singleton declaration
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeTokenBag();
    }

    private void InitializeTokenBag()
    {
        // Create 7 of each normal token type, 35 total
        CreateTokens(ResourceTokenType.DuctTape, initialNormalResourceTokenCount);
        CreateTokens(ResourceTokenType.Glue, initialNormalResourceTokenCount);
        CreateTokens(ResourceTokenType.Wire, initialNormalResourceTokenCount);
        CreateTokens(ResourceTokenType.Fabric, initialNormalResourceTokenCount);
        CreateTokens(ResourceTokenType.Plastic, initialNormalResourceTokenCount);

        // Create 5 glitter tokens
        CreateTokens(ResourceTokenType.Glitter, initialGlitterResourceTokenCount);

        drawPile = Shuffle(drawPile);
    }

    private void CreateTokens(ResourceTokenType resourceTokenType, int count)
    {
        for (int i = 0; i < count; i++)
        {
            ResourceToken tokenObj = Instantiate(resourceTokenPrefab, transform);
            ResourceToken resourceToken = tokenObj.GetComponent<ResourceToken>();
            resourceToken.gameObject.SetActive(false);
            if (resourceToken != null)
            {
                resourceToken.resourceTokenType = resourceTokenType;
                drawPile.Add(resourceToken);
            }
        }
    }

    private void Update()
    {
        // Use Raycasting to detect mouse clicks on the deck
        if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame) return;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                Debug.Log("Resource Token Bag clicked!");
                GameManager.Instance.OnResourceTokenBagClicked();
            }
        }
    }

    public void ReturnResourceTokenToBag(ResourceToken resourceToken)
    {
        if (resourceToken == null) return;

        resourceToken.gameObject.SetActive(false);
        resourceToken.transform.SetParent(transform);
        drawPile.Add(resourceToken);
        drawPile = Shuffle(drawPile);
    }

    public ResourceToken Draw()
    {
        if (drawPile.Count == 0)
        {
            Debug.LogWarning("No more tokens in the token bag to draw.");
            return null;
        }
        ResourceToken drawnToken = drawPile[0];
        drawPile.RemoveAt(0);
        drawnToken.gameObject.SetActive(true);
        return drawnToken;
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
