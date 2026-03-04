using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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

    [Header("Pulse Settings")]
    [SerializeField] private float pulseScale = 1.2f;
    [SerializeField] private float pulseSpeed = 0.2f;

    private readonly List<ResourceToken> processedTokens = new List<ResourceToken>();

    [Header("Background Images")]
    [SerializeField] private Image ductTapeBg;
    [SerializeField] private Image glueBg;
    [SerializeField] private Image wireBg;
    [SerializeField] private Image fabricBg;
    [SerializeField] private Image plasticBg;
    [SerializeField] private Image glitterBg;

    [Header("Glitter Rainbow")]
    [SerializeField] private float glitterCycleSeconds = 2.0f;

    private Color[] glitterPalette;
    private float glitterTimer;

    void Awake()
    {
        resourceTokenContainer = GetComponent<Transform>();
        ApplyTypeColorsToBackgrounds();
        BuildGlitterPalette();
    }

    private void Update()
    {
        UpdateGlitterBackground();
    }

    private void ApplyTypeColorsToBackgrounds()
    {
        if (ductTapeBg != null) ductTapeBg.color = GetColorForType(ResourceTokenType.DuctTape);
        if (glueBg != null) glueBg.color = GetColorForType(ResourceTokenType.Glue);
        if (wireBg != null) wireBg.color = GetColorForType(ResourceTokenType.Wire);
        if (fabricBg != null) fabricBg.color = GetColorForType(ResourceTokenType.Fabric);
        if (plasticBg != null) plasticBg.color = GetColorForType(ResourceTokenType.Plastic);
    }

    private void BuildGlitterPalette()
    {
        glitterPalette = new[]
        {
            GetColorForType(ResourceTokenType.DuctTape),
            GetColorForType(ResourceTokenType.Glue),
            GetColorForType(ResourceTokenType.Wire),
            GetColorForType(ResourceTokenType.Fabric),
            GetColorForType(ResourceTokenType.Plastic)
        };
    }

    private void UpdateGlitterBackground()
    {
        if (glitterBg == null || glitterPalette == null || glitterPalette.Length == 0)
        {
            return;
        }

        if (glitterCycleSeconds <= 0.01f)
        {
            glitterBg.color = glitterPalette[0];
            return;
        }

        glitterTimer += Time.deltaTime;
        float progress = Mathf.Repeat(glitterTimer / glitterCycleSeconds, 1f);
        glitterBg.color = EvaluateGlitterColor(progress);
    }

    private Color EvaluateGlitterColor(float progress)
    {
        int paletteCount = glitterPalette.Length;
        float scaled = progress * paletteCount;

        int fromIndex = Mathf.FloorToInt(scaled) % paletteCount;
        int toIndex = (fromIndex + 1) % paletteCount;
        float blend = scaled - Mathf.Floor(scaled);

        return Color.Lerp(glitterPalette[fromIndex], glitterPalette[toIndex], blend);
    }

    public void UpdateResourceTokenUI(List<ResourceToken> resourceTokens)
    {
        HideTokensForUI(resourceTokens);
        AssignTokensCount(resourceTokens);
    }

    // TODO:Understand if it takes the copy or the original token, sometimes it doesn't disappear from the world when picked up
    private void HideTokensForUI(List<ResourceToken> resourceTokens)
    {
        foreach (ResourceToken resourceToken in resourceTokens)
        {
            if (processedTokens.Contains(resourceToken)) continue;

            // Disable 3D rendering
            if (resourceToken.TryGetComponent<MeshRenderer>(out var meshRenderer)) meshRenderer.enabled = false;

            // Disable collider
            if (resourceToken.TryGetComponent<Collider>(out var collider)) collider.enabled = false;

            // Reparent to resource token container (Canvas)
            if (resourceToken.TryGetComponent<RectTransform>(out var tokenRect))
            {
                tokenRect.SetParent(resourceTokenContainer);
            }
            processedTokens.Add(resourceToken);
        }
    }

    private void AssignTokensCount(List<ResourceToken> resourceTokens)
    {
        List<(ResourceTokenType, TextMeshProUGUI)> tokenCounts = new List<(ResourceTokenType, TextMeshProUGUI)>
        {
            (ResourceTokenType.DuctTape, ductTapeText),
            (ResourceTokenType.Glue, glueText),
            (ResourceTokenType.Wire, wireText),
            (ResourceTokenType.Fabric, fabricText),
            (ResourceTokenType.Plastic, plasticText),
            (ResourceTokenType.Glitter, glitterText)
        };

        foreach (var (tokenType, textComponent) in tokenCounts)
        {
            int count = resourceTokens.Count(t => t.resourceTokenType == tokenType);
            string newValue = count.ToString();
            
            // Pulse effect if count changed
            if (textComponent.text != newValue)
            {
                textComponent.text = newValue;
                TokenPulseEffect(textComponent);
            }
        }
    }

    private void TokenPulseEffect(TextMeshProUGUI textComponent)
    {
        if (textComponent == null) return;

        Transform target = textComponent.transform;

        // Stop any existing tween on this target
        target.DOKill();

        // Ensure consistent baseline
        target.localScale = Vector3.one;

        // Pulse once (up then back)
        target.DOScale(Vector3.one * pulseScale, pulseSpeed)
            .SetEase(Ease.InOutSine)
            .SetLoops(2, LoopType.Yoyo);
    }
}
