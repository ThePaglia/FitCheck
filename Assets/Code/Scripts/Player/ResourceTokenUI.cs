using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static ResourceToken;

public class ResourceTokenUI : MonoBehaviour
{
    private Transform resourceTokenContainer;
    [Header("Amount Labels")]
    [SerializeField] private TextMeshProUGUI ductTapeText;
    [SerializeField] private TextMeshProUGUI glueText;
    [SerializeField] private TextMeshProUGUI wireText;
    [SerializeField] private TextMeshProUGUI fabricText;
    [SerializeField] private TextMeshProUGUI plasticText;
    [SerializeField] private TextMeshProUGUI glitterText;

    private readonly List<ResourceToken> processedTokens = new List<ResourceToken>();

    void Awake()
    {
        resourceTokenContainer = GetComponent<Transform>();
    }

    public void UpdateResourceTokenUI(List<ResourceToken> resourceTokens)
    {
        // First, ensure all tokens are properly set up for UI
        foreach (ResourceToken resourceToken in resourceTokens)
        {
            if (!processedTokens.Contains(resourceToken))
            {
                // Disable 3D rendering
                MeshRenderer meshRenderer = resourceToken.GetComponent<MeshRenderer>();
                if (meshRenderer != null) meshRenderer.enabled = false;

                Collider collider = resourceToken.GetComponent<Collider>();
                if (collider != null) collider.enabled = false;

                // Reparent to resource token container (Canvas)
                if (resourceToken.TryGetComponent<RectTransform>(out var tokenRect))
                {
                    tokenRect.SetParent(resourceTokenContainer);
                }

                processedTokens.Add(resourceToken);
            }
        }

        // Now count tokens by type
        int ductTape = 0, glue = 0, wire = 0, fabric = 0, plastic = 0, glitter = 0;

        foreach (var token in resourceTokens)
        {
            switch (token.resourceTokenType)
            {
                case ResourceTokenType.DuctTape: ductTape++; break;
                case ResourceTokenType.Glue: glue++; break;
                case ResourceTokenType.Wire: wire++; break;
                case ResourceTokenType.Fabric: fabric++; break;
                case ResourceTokenType.Plastic: plastic++; break;
                case ResourceTokenType.Glitter: glitter++; break;
            }
        }

        // Update UI labels
        if (ductTapeText != null) ductTapeText.text = ductTape.ToString();
        if (glueText != null) glueText.text = glue.ToString();
        if (wireText != null) wireText.text = wire.ToString();
        if (fabricText != null) fabricText.text = fabric.ToString();
        if (plasticText != null) plasticText.text = plastic.ToString();
        if (glitterText != null) glitterText.text = glitter.ToString();
    }
}
