using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ResourceTokenBag : MonoBehaviour
{
    public static ResourceTokenBag Instance { get; private set; } //Singleton
    [SerializeField] private ResourceToken resourceTokenPrefab;
    [SerializeField] private Transform pouchContainer;
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
        // Create 7 of each normal token type
        CreateTokens(ResourceToken.ResourceTokenType.DuctTape, 7);
        CreateTokens(ResourceToken.ResourceTokenType.Glue, 7);
        CreateTokens(ResourceToken.ResourceTokenType.Wire, 7);
        CreateTokens(ResourceToken.ResourceTokenType.Fabric, 7);
        CreateTokens(ResourceToken.ResourceTokenType.Plastic, 7);

        // Create 5 glitter tokens
        CreateTokens(ResourceToken.ResourceTokenType.Glitter, 5);

        drawPile = Shuffle(drawPile);
    }

    private void CreateTokens(ResourceToken.ResourceTokenType tokenType, int count)
    {
        for (int i = 0; i < count; i++)
        {
            ResourceToken tokenObj = Instantiate(resourceTokenPrefab, pouchContainer);
            ResourceToken resourceToken = tokenObj.GetComponent<ResourceToken>();
            resourceToken.gameObject.SetActive(false);
            if (resourceToken != null)
            {
                resourceToken.tokenType = tokenType;
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
                GameManager.Instance.OnTokenBagClicked();
            }
        }
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
