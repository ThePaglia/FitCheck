using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static ResourceToken;

public class ResourceTokenArea : MonoBehaviour
{
    [SerializeField] private float spacing = 0.2f;
    [SerializeField] private float stackHeight = 0.05f;
    [SerializeField] private int initialResourceTokenCount = 6;
    [SerializeField] private ResourceToken prefabScale;
    private readonly int columns = System.Enum.GetNames(typeof(ResourceTokenType)).Length;

    private readonly Dictionary<ResourceTokenType, List<ResourceToken>> resourceTokensByType =
        new Dictionary<ResourceTokenType, List<ResourceToken>>();

    private List<Vector3> colPositions = new List<Vector3>();

    private void Start()
    {
        InitializeColumnPositions();
        PlaceInitialResourceTokens();
    }

    private void InitializeColumnPositions()
    {
        // Initialize dictionary for each token type
        foreach (ResourceTokenType type in System.Enum.GetValues(typeof(ResourceTokenType)))
        {
            resourceTokensByType[type] = new List<ResourceToken>();
        }

        // Calculate column positions (centered)
        float totalWidth = (columns - 1) * spacing;
        float startX = -totalWidth / 2f;

        for (int i = 0; i < columns; i++)
        {
            Vector3 columnPos = transform.position + new Vector3(startX + (i * spacing), 0f, 0f);
            colPositions.Add(columnPos);
        }
    }

    private void PlaceInitialResourceTokens()
    {
        // Draw 6 random resource tokens and place them in front of the bag
        for (int i = 0; i < initialResourceTokenCount; i++)
        {
            ResourceToken resourceToken = ResourceTokenBag.Instance.Draw();

            if (resourceToken != null)
            {
                PlaceResourceTokenInItsColumn(resourceToken);
            }
        }
    }

    private void PlaceResourceTokenInItsColumn(ResourceToken resourceToken)
    {
        ResourceTokenType resourceTokenType = resourceToken.resourceTokenType;
        int colIndex = (int)resourceTokenType;

        // Add to tracking list
        resourceTokensByType[resourceTokenType].Add(resourceToken);

        // Get stack height
        int stackCount = resourceTokensByType[resourceTokenType].Count - 1;
        Vector3 columnPos = colPositions[colIndex];
        Vector3 stackPos = columnPos + new Vector3(0f, stackCount * stackHeight, 0f);

        resourceToken.transform.SetParent(transform);
        resourceToken.transform.position = stackPos;
        resourceToken.transform.localScale = prefabScale.transform.localScale;
        resourceToken.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame) return;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out var hit))
        {
            ResourceToken resourceToken = hit.collider.GetComponentInParent<ResourceToken>();
            if (resourceToken != null && IsTokenInPlay(resourceToken))
            {
                if (GameManager.Instance != null && GameManager.Instance.OnResourceTokenInPlayAreaClicked(resourceToken))
                {
                    RemoveTokenFromColumn(resourceToken);

                    // Draw replacement and add to same column
                    ResourceToken replacement = ResourceTokenBag.Instance.Draw();
                    if (replacement != null)
                    {
                        PlaceResourceTokenInItsColumn(replacement);
                    }
                }
            }
        }
    }

    private bool IsTokenInPlay(ResourceToken resourceToken)
    {
        foreach (var list in resourceTokensByType.Values)
        {
            if (list.Contains(resourceToken)) return true;
        }
        return false;
    }

    private void RemoveTokenFromColumn(ResourceToken resourceToken)
    {
        ResourceTokenType tokenType = resourceToken.resourceTokenType;
        if (resourceTokensByType[tokenType].Contains(resourceToken))
        {
            resourceTokensByType[tokenType].Remove(resourceToken);

            // Restack remaining tokens in this column
            RestackColumn(tokenType);
        }
    }

    private void RestackColumn(ResourceTokenType tokenType)
    {
        int columnIndex = (int)tokenType;
        Vector3 columnPos = colPositions[columnIndex];

        for (int i = 0; i < resourceTokensByType[tokenType].Count; i++)
        {
            Vector3 stackPos = columnPos + new Vector3(0f, i * stackHeight, 0f);
            resourceTokensByType[tokenType][i].transform.position = stackPos;
        }
    }
}
